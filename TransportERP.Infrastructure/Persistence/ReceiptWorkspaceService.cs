using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Accounting;
using TransportERP.Contracts.Core;

namespace TransportERP.Infrastructure.Persistence;

// Called only after the authenticated API has checked the corresponding receipt permission.
public sealed partial class ReceiptWorkspaceService(TransportErpDbContext db)
{
    public const string SettingsKey = "accounting.receipts.configuration";
    public async Task<ReceiptConfiguration> SettingsAsync(Guid companyId, CancellationToken ct = default)
    {
        var value = await db.CompanySettings.AsNoTracking().SingleOrDefaultAsync(x =>
            x.CompanyId == companyId && x.Key == SettingsKey && x.Status == "ACTIVE", ct);
        return value == null ? new("DIRECT_BANK", null, null, null, [], [], [], []) :
            JsonSerializer.Deserialize<ReceiptConfiguration>(value.ValueJson) ?? throw new InvalidOperationException("إعدادات القبض غير صالحة.");
    }

    public async Task<ReceiptDocument> GetAsync(OperationContext scope, Guid id, CancellationToken ct = default)
    {
        var document = Document(await Find(scope, id, ct));
        if (document.JournalId.HasValue)
        {
            var credits = await db.JournalEntryLines.AsNoTracking().Where(l => l.JournalEntryId == document.JournalId && l.LineNo > 1)
                .OrderBy(l => l.LineNo).Select(l => l.Credit).ToArrayAsync(ct);
            if (credits.Length != document.Draft.Lines.Count) throw new InvalidOperationException("تفاصيل القيد لا تطابق سند القبض؛ راجع الربط.");
            document = document with { Draft = document.Draft with { Lines = document.Draft.Lines.Select((line, i) => line with
                { Additional = new Dictionary<string, string?>(line.Additional ?? []) { ["accountingAmount"] = credits[i].ToString(System.Globalization.CultureInfo.InvariantCulture) } }).ToList() } };
        }
        var events = await (from audit in db.AuditEvents.AsNoTracking()
            join user in db.Users on audit.ActorUserId equals user.Id into users
            from user in users.DefaultIfEmpty()
            where audit.EntityType == "ReceiptVoucher" && audit.EntityId == id && audit.CompanyId == scope.CompanyId && audit.BranchId == scope.BranchId
            orderby audit.OccurredAt
            select new ReceiptAuditItem(audit.Action, user == null ? "—" : user.DisplayName, audit.OccurredAt, audit.Reason)).ToListAsync(ct);
        var attachments = await db.Set<ReceiptAttachment>().AsNoTracking().Where(a => a.ReceiptId == id)
            .OrderBy(a => a.AddedAt).Select(a => new ReceiptAttachmentInfo(a.Id, a.FileName, a.MediaType, a.Content.Length, a.Hash, a.AddedAt)).ToListAsync(ct);
        var rows = new List<ReceiptAccountRow>();
        if (document.JournalId.HasValue)
            rows = await (from line in db.JournalEntryLines where line.JournalEntryId == document.JournalId
                join account in db.ChartOfAccounts on line.AccountId equals account.Id
                join currency in db.Currencies on line.CurrencyId equals currency.Id
                orderby line.LineNo
                select new ReceiptAccountRow(account.Code + " — " + account.NameAr, currency.Code, line.ForeignAmount, line.Debit, line.Credit, line.Description ?? "")).ToListAsync(ct);
        return await WithWorkflow(scope, document with { Audit = events, Attachments = attachments, AccountRows = rows }, ct);
    }

    public async Task<ReceiptDocument> SaveAsync(OperationContext scope, ReceiptDraft draft, CancellationToken ct = default)
    {
        await using var transaction = db.Database.IsRelational() ? await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct) : null;
        scope.EnsureComplete();
        var settings = await SettingsAsync(scope.CompanyId, ct);
        await Validate(scope, draft, settings, ct);
        var accountIds = draft.Lines.Select(l => l.AccountId).Distinct().ToArray();
        var accountNames = await db.ChartOfAccounts.Where(a => accountIds.Contains(a.Id) && a.CompanyId == scope.CompanyId)
            .ToDictionaryAsync(a => a.Id, a => a.NameAr, ct);
        // Displayed financial amounts are derived from typed amounts/the posted journal, never trusted from client metadata.
        draft = draft with { Additional = new Dictionary<string, string?>(draft.Additional)
            { ["requiresWaybill"] = settings.Types.Single(t => t.Id == draft.TypeId).RequiresWaybill ? "true" : "false" },
            Lines = draft.Lines.Select(line => line with
        {
            Additional = new Dictionary<string, string?>(line.Additional ?? [])
            {
                ["foreignAmount"] = line.Amount.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["accountingAmount"] = null, ["accountName"] = accountNames[line.AccountId]
            }
        }).ToList() };
        var json = JsonSerializer.Serialize(draft with { ExpectedVersion = null });
        var entity = await db.ReceiptVouchers.SingleOrDefaultAsync(v => v.Id == draft.Id, ct);
        if (entity != null)
        {
            RequireScope(scope, entity);
            // Identical retries are safe even when the successful response was lost.
            if (entity.DocumentJson == json) return await WithWorkflow(scope, Document(entity), ct);
            Version(entity, draft.ExpectedVersion);
            if (entity.Status != "DRAFT") throw new InvalidOperationException("السند غير قابل للتعديل بعد الاعتماد.");
        }
        else
        {
            if (draft.ExpectedVersion != null) throw new InvalidOperationException("السند المطلوب تعديله غير موجود.");
            entity = new ReceiptVoucher { Id = draft.Id, CompanyId = scope.CompanyId, BranchId = scope.BranchId,
                VoucherNo = "DRAFT-" + draft.Id.ToString("N"), Status = "DRAFT", CreatedAt = DateTimeOffset.UtcNow };
            db.ReceiptVouchers.Add(entity);
            await new GeneralLedgerPolicyService(db).BindNewAsync(scope, "RECEIPT_VOUCHER", draft.Id, ct);
        }
        entity.DocumentJson = json;
        entity.VoucherDate = DateTime.SpecifyKind(draft.Date.Date, DateTimeKind.Utc);
        entity.CurrencyId = draft.CurrencyId; entity.Amount = draft.Amount;
        entity.PaymentMethodCode = draft.Method; entity.CollectedBy = draft.CollectorId;
        entity.Notes = draft.Description; entity.ReferenceType = "RECEIPT";
        entity.PayerName = draft.Additional.GetValueOrDefault("partyName") ?? "";
        entity.UpdatedAt = DateTimeOffset.UtcNow; entity.RowVersion = Guid.NewGuid().ToByteArray();
        await Audit(scope, entity, "SaveReceipt", ct);
        await db.SaveChangesAsync(ct);
        if (transaction != null) await transaction.CommitAsync(ct);
        return await WithWorkflow(scope, Document(entity), ct);
    }

    public async Task<ReceiptDocument> ApproveAsync(OperationContext scope, Guid id, string version, CancellationToken ct = default)
    {
        await using var transaction = db.Database.IsRelational() ? await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct) : null;
        var entity = await Find(scope, id, ct);
        var workflow = await new GeneralLedgerPolicyService(db).ForDocumentAsync(scope, "RECEIPT_VOUCHER", id, ct);
        if (!workflow.Policy.RequireApproval) throw new InvalidOperationException("السياسة المحفوظة لا تتطلب اعتمادًا.");
        if (entity.Status is "APPROVED" or "POSTED") return await WithWorkflow(scope, Document(entity), ct);
        Version(entity, version);
        if (entity.Status != (workflow.Policy.RequireReview ? "REVIEWED" : "DRAFT"))
            throw new InvalidOperationException("أكمل المراجعة المطلوبة قبل الاعتماد.");
        var document = Document(entity);
        await Validate(scope, document.Draft, await SettingsAsync(scope.CompanyId, ct), ct);
        entity.Status = "APPROVED"; Stamp(entity); await Audit(scope, entity, "ApproveReceipt", ct);
        await db.SaveChangesAsync(ct); if (transaction != null) await transaction.CommitAsync(ct); return await WithWorkflow(scope, Document(entity), ct);
    }

    public async Task<ReceiptDocument> PostAsync(OperationContext scope, Guid id, string version, CancellationToken ct = default)
    {
        // Receipt, numbering and all journal lines commit together or roll back together.
        await using var transaction = db.Database.IsRelational() ? await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct) : null;
        var entity = await Find(scope, id, ct);
        if (entity.Status == "POSTED" && entity.PostingJournalId != null) return await WithWorkflow(scope, Document(entity), ct);
        Version(entity, version);
        var workflow = await new GeneralLedgerPolicyService(db).ForDocumentAsync(scope, "RECEIPT_VOUCHER", id, ct);
        if (!workflow.Policy.IsReady(entity.Status)) throw new InvalidOperationException("أكمل المراجعة والاعتماد المطلوبين في سياسة السند قبل الترحيل.");
        var draft = Document(entity).Draft;
        var postingDate = DateTime.SpecifyKind(draft.Date.Date, DateTimeKind.Utc);
        var settings = await SettingsAsync(scope.CompanyId, ct);
        await Validate(scope, draft, settings, ct);
        var company = await db.Companies.SingleAsync(c => c.Id == scope.CompanyId, ct);
        var currency = await db.Currencies.SingleAsync(c => c.Id == company.BaseCurrencyId && c.Status == "ACTIVE", ct);
        var plan = ReceiptPostingPlan.Build(draft, settings, company.BaseCurrencyId, currency.MinorUnit);
        var accounts = plan.Lines.Select(l => l.AccountId).Distinct().ToArray();
        if (await db.ChartOfAccounts.CountAsync(a => accounts.Contains(a.Id) && a.CompanyId == scope.CompanyId &&
            a.Status == "ACTIVE" && a.PostingAllowed && a.DeletedAt == null, ct) != accounts.Length)
            throw new InvalidOperationException("أحد حسابات الترحيل غير صالح أو خارج الشركة.");
        var periods = await db.FiscalPeriods.Where(p => p.CompanyId == scope.CompanyId && p.Status == "OPEN" &&
            p.StartDate <= postingDate && p.EndDate >= postingDate).ToListAsync(ct);
        if (periods.Count != 1) throw new InvalidOperationException("لا توجد فترة محاسبية مفتوحة وفريدة لتاريخ السند.");
        if (settings.NumberSequenceId == null) throw new InvalidOperationException("تسلسل ترقيم سند القبض غير مهيأ.");
        var sequence = await db.Set<NumberSequenceEntity>().SingleOrDefaultAsync(s => s.Id == settings.NumberSequenceId &&
            s.CompanyId == scope.CompanyId && (s.BranchId == null || s.BranchId == scope.BranchId) && s.Status == "ACTIVE" && s.DocumentType == "RECEIPT_VOUCHER", ct)
            ?? throw new InvalidOperationException("تسلسل الترقيم غير متاح ضمن النطاق.");
        if (sequence.NextValue < 1) throw new InvalidOperationException("قيمة تسلسل الترقيم غير صالحة.");
        string number = sequence.Prefix + sequence.NextValue.ToString("D8");
        if (number.Length > 53) throw new InvalidOperationException("بادئة الترقيم طويلة جدًا لسند القبض وقيد عكسه.");
        sequence.NextValue = checked(sequence.NextValue + 1); sequence.Version++; sequence.UpdatedAt = DateTimeOffset.UtcNow;
        var journal = new JournalEntry { Id = Guid.NewGuid(), CompanyId = scope.CompanyId, BranchId = scope.BranchId,
            DocumentNo = "RV-" + number, EntryDate = entity.VoucherDate, FiscalPeriodId = periods[0].Id,
            SourceType = "RECEIPT_VOUCHER", SourceId = id, Description = draft.Description, Status = "POSTED",
            CurrencyId = company.BaseCurrencyId, ExchangeRate = 1, TotalDebit = plan.Lines.Sum(l => l.Debit),
            TotalCredit = plan.Lines.Sum(l => l.Credit), CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray() };
        journal.Lines = plan.Lines.Select((l, i) => new JournalEntryLine { JournalEntryId = journal.Id, LineNo = i + 1,
            AccountId = l.AccountId, Debit = l.Debit, Credit = l.Credit, ForeignAmount = l.ForeignAmount,
            CurrencyId = l.CurrencyId, FinancialDimensionId = l.FinancialDimensionId, Description = draft.Description }).ToArray();
        db.JournalEntries.Add(journal);
        entity.VoucherNo = number; entity.Status = "POSTED"; entity.PostingJournalId = journal.Id;
        entity.PostingPolicyJson = plan.PolicySnapshot; Stamp(entity);
        await Audit(scope, entity, "PostReceipt", ct);
        await db.SaveChangesAsync(ct);
        if (transaction != null) await transaction.CommitAsync(ct);
        return await WithWorkflow(scope, Document(entity), ct);
    }

    public async Task<ReceiptDocument> CancelAsync(OperationContext scope, Guid id, string version, string reason, CancellationToken ct = default)
    {
        await using var transaction = db.Database.IsRelational() ? await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct) : null;
        if (string.IsNullOrWhiteSpace(reason) || reason.Length > 500) throw new InvalidOperationException("سبب الإلغاء مطلوب (حتى 500 حرف).");
        var entity = await Find(scope, id, ct);
        if (entity.Status == "CANCELLED") return Document(entity);
        Version(entity, version);
        if (entity.Status is not ("DRAFT" or "REVIEWED" or "APPROVED")) throw new InvalidOperationException("لا يمكن إلغاء سند مرحّل؛ استخدم العكس.");
        entity.Status = "CANCELLED"; entity.Notes = reason; Stamp(entity);
        await Audit(scope, entity, "CancelReceipt", ct, reason);
        await db.SaveChangesAsync(ct); if (transaction != null) await transaction.CommitAsync(ct); return Document(entity);
    }

    public async Task<ReceiptDocument> ReverseAsync(OperationContext scope, Guid id, string version, string reason, DateTime date, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(reason) || reason.Length > 500) throw new InvalidOperationException("سبب العكس مطلوب (حتى 500 حرف).");
        date = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);
        await using var transaction = db.Database.IsRelational() ? await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct) : null;
        var entity = await Find(scope, id, ct);
        if (entity.ReversalJournalId != null) return Document(entity);
        Version(entity, version);
        if (entity.Status != "POSTED" || entity.PostingJournalId == null) throw new InvalidOperationException("العكس متاح للسند المرحّل فقط.");
        var periods = await db.FiscalPeriods.Where(p => p.CompanyId == scope.CompanyId && p.Status == "OPEN" && p.StartDate <= date && p.EndDate >= date).ToListAsync(ct);
        if (periods.Count != 1) throw new InvalidOperationException("اختر تاريخ عكس ضمن فترة مفتوحة وفريدة.");
        var original = await db.JournalEntries.Include(j => j.Lines).SingleAsync(j => j.Id == entity.PostingJournalId && j.CompanyId == scope.CompanyId && j.BranchId == scope.BranchId, ct);
        var reversal = new JournalEntry { Id = Guid.NewGuid(), CompanyId = scope.CompanyId, BranchId = scope.BranchId,
            DocumentNo = "REV-" + original.DocumentNo, FiscalPeriodId = periods[0].Id, EntryDate = date, Description = reason,
            Status = "POSTED", SourceType = "RECEIPT_REVERSAL", SourceId = id, ReversalOfId = original.Id,
            TotalDebit = original.TotalCredit, TotalCredit = original.TotalDebit, CurrencyId = original.CurrencyId, ExchangeRate = original.ExchangeRate,
            CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow, RowVersion = Guid.NewGuid().ToByteArray() };
        reversal.Lines = original.Lines.Select(l => new JournalEntryLine { JournalEntryId = reversal.Id, LineNo = l.LineNo,
            AccountId = l.AccountId, FinancialDimensionId = l.FinancialDimensionId, Description = reason,
            Debit = l.Credit, Credit = l.Debit, ForeignAmount = l.ForeignAmount, CurrencyId = l.CurrencyId }).ToArray();
        db.JournalEntries.Add(reversal); entity.ReversalJournalId = reversal.Id; Stamp(entity);
        await Audit(scope, entity, "ReverseReceipt", ct, reason);
        await db.SaveChangesAsync(ct); if (transaction != null) await transaction.CommitAsync(ct);
        return Document(entity);
    }

    private async Task Validate(OperationContext scope, ReceiptDraft draft, ReceiptConfiguration settings, CancellationToken ct)
    {
        if (draft.Id == Guid.Empty || draft.Method is not ("CASH" or "CHEQUE") || draft.Amount <= 0 || draft.Amount > 999999999999999.9999m ||
            decimal.Round(draft.Amount, 4) != draft.Amount || draft.ExchangeRate <= 0 ||
            string.IsNullOrWhiteSpace(draft.Description) || draft.Description.Length > 500 ||
            draft.Lines.Count is < 1 or > 1000) throw new InvalidOperationException("بيانات سند القبض غير صالحة.");
        if (!await db.Companies.AnyAsync(c => c.Id == scope.CompanyId && c.Status == "ACTIVE", ct) ||
            !await db.Branches.AnyAsync(b => b.Id == scope.BranchId && b.CompanyId == scope.CompanyId && b.Status == "ACTIVE", ct))
            throw new InvalidOperationException("الفرع غير متاح.");
        if (!settings.CollectorIds.Contains(draft.CollectorId) || !await db.Users.AnyAsync(u => u.Id == draft.CollectorId &&
            u.Status == "ACTIVE" && u.DeletedAt == null && (!u.CompanyId.HasValue || u.CompanyId == scope.CompanyId) &&
            (!u.BranchId.HasValue || u.BranchId == scope.BranchId), ct)) throw new InvalidOperationException("المحصل غير معتمد في هذا النطاق.");
        var type = settings.Types.SingleOrDefault(t => t.Id == draft.TypeId) ?? throw new InvalidOperationException("نوع السند غير معتمد.");
        await ValidateReceiptReferences(scope, draft, settings, ct);
        var destination = settings.Destinations.SingleOrDefault(d => d.Id == draft.DestinationId);
        if (destination == null || destination.Kind != (draft.Method == "CASH" ? "CASH" : "BANK"))
            throw new InvalidOperationException("اختر وجهة قبض مطابقة للطريقة.");
        var currencies = draft.Lines.Select(l => l.CurrencyId).Append(draft.CurrencyId).Distinct().ToArray();
        if (await db.Currencies.CountAsync(c => currencies.Contains(c.Id) && c.Status == "ACTIVE", ct) != currencies.Length)
            throw new InvalidOperationException("عملة غير متاحة.");
        var accounts = draft.Lines.Select(l => l.AccountId).Distinct().ToArray();
        if (await db.ChartOfAccounts.CountAsync(a => accounts.Contains(a.Id) && a.CompanyId == scope.CompanyId &&
            a.Status == "ACTIVE" && a.PostingAllowed && a.DeletedAt == null, ct) != accounts.Length)
            throw new InvalidOperationException("حساب تفاصيل غير صالح.");
        foreach (var line in draft.Lines)
        {
            if (line.Amount <= 0 || line.Amount > 999999999999999.9999m || decimal.Round(line.Amount, 4) != line.Amount ||
                (line.CurrencyId != draft.CurrencyId && !(line.Rate > 0))) throw new InvalidOperationException("مبلغ أو سعر سطر غير صالح.");
            if (draft.Method == "CHEQUE" && (string.IsNullOrWhiteSpace(line.ChequeNumber) || line.ChequeDueDate == null))
                throw new InvalidOperationException("رقم الشيك وتاريخ استحقاقه مطلوبان.");
            if (type.RequiresWaybill && !line.WaybillId.HasValue) throw new InvalidOperationException("رقم البوليصة مطلوب لكل سطر.");
            if (line.WaybillId.HasValue && !await db.Set<WaybillEntity>().AnyAsync(w => w.Id == line.WaybillId &&
                w.CompanyId == scope.CompanyId && w.BranchId == scope.BranchId, ct))
                throw new InvalidOperationException("البوليصة غير موجودة في نطاق السند.");
        }
    }
    private async Task<ReceiptVoucher> Find(OperationContext scope, Guid id, CancellationToken ct)
    {
        scope.EnsureComplete();
        return await db.ReceiptVouchers.SingleOrDefaultAsync(v => v.Id == id && v.CompanyId == scope.CompanyId &&
            v.BranchId == scope.BranchId, ct) ?? throw new KeyNotFoundException("سند القبض غير موجود في النطاق.");
    }
    private static void RequireScope(OperationContext scope, ReceiptVoucher v)
    { if (v.CompanyId != scope.CompanyId || v.BranchId != scope.BranchId) throw new UnauthorizedAccessException(); }
    private static void Version(ReceiptVoucher v, string? version)
    { if (version != Convert.ToBase64String(v.RowVersion)) throw new DbUpdateConcurrencyException("تغير السند؛ أعد تحميله."); }
    private static void Stamp(ReceiptVoucher v) { v.UpdatedAt = DateTimeOffset.UtcNow; v.RowVersion = Guid.NewGuid().ToByteArray(); }
    private Task Audit(OperationContext scope, ReceiptVoucher v, string action, CancellationToken ct, string? reason = null) =>
        new AuditEventService(db).AppendReceiptInTransactionAsync(new(action, "SUCCESS", "ReceiptVoucher", v.Id, scope.UserId,
            scope.CompanyId, scope.BranchId, scope.CorrelationId, AfterJson: JsonSerializer.Serialize(Document(v)), Reason: reason), ct);
    private static ReceiptDocument Document(ReceiptVoucher v)
    {
        var draft = JsonSerializer.Deserialize<ReceiptDraft>(v.DocumentJson ?? "null")
            ?? throw new InvalidOperationException("هذا سند قديم لا يحتوي تفاصيل؛ يلزم استكمال ترحيل بياناته أولًا.");
        return new(draft with { ExpectedVersion = Convert.ToBase64String(v.RowVersion) }, v.VoucherNo, v.ReversalJournalId != null ? "REVERSED" : v.Status,
            Convert.ToBase64String(v.RowVersion), v.PostingJournalId, v.PostingPolicyJson);
    }
}
