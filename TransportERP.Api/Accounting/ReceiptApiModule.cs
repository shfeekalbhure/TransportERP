using System.Security.Claims;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Accounting;
using TransportERP.Contracts.Core;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Accounting;

public static partial class ReceiptApiModule
{
    public sealed record Transition(string Version, string? Reason = null, DateTime? Date = null);
    public static void MapReceiptWorkspace(this WebApplication app)
    {
        MapLedgerSettings(app);
        var group = app.MapGroup("/api/v1/receipts").RequireAuthorization("Authenticated");
        group.MapGet("/bootstrap", (HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, "ACC043.View", async scope =>
            {
                var actions = new List<string>();
                foreach (var code in new[] { "ACC043.View", "ACC043.Create", "ACC043.Edit", "ACC043.Post", "ACC043.Cancel", "ACC043.Reverse", "accounting.receipts.review", "accounting.receipts.approve", "accounting.receipts.configure" })
                    if (await Allowed(db, scope, code, ct)) actions.Add(code);
                return new ReceiptBootstrap(await new ReceiptWorkspaceService(db).SettingsAsync(scope.CompanyId, ct),
                    await db.ChartOfAccounts.Where(a => a.CompanyId == scope.CompanyId && a.Status == "ACTIVE" && a.PostingAllowed && a.DeletedAt == null)
                        .Select(a => new ReceiptChoice(a.Id, a.Code + " — " + a.NameAr)).ToListAsync(ct),
                    await db.Currencies.Where(c => c.Status == "ACTIVE").Select(c => new ReceiptChoice(c.Id, c.Code + " — " + c.NameAr)).ToListAsync(ct),
                    await db.Users.Where(u => u.Status == "ACTIVE" && u.DeletedAt == null && (!u.CompanyId.HasValue || u.CompanyId == scope.CompanyId) &&
                        (!u.BranchId.HasValue || u.BranchId == scope.BranchId)).Select(u => new ReceiptChoice(u.Id, u.UserName + " — " + u.DisplayName)).ToListAsync(ct),
                    await db.Set<WaybillEntity>().Where(w => w.CompanyId == scope.CompanyId && w.BranchId == scope.BranchId)
                        .Select(w => new ReceiptChoice(w.Id, w.WaybillNo ?? w.DraftNo)).ToListAsync(ct),
                    await db.Set<NumberSequenceEntity>().Where(s => s.CompanyId == scope.CompanyId && (s.BranchId == null || s.BranchId == scope.BranchId) &&
                        s.Status == "ACTIVE" && s.DocumentType == "RECEIPT_VOUCHER").Select(s => new ReceiptChoice(s.Id, s.Prefix ?? s.Id.ToString())).ToListAsync(ct),
                    actions, await db.Branches.Where(b => b.Id == scope.BranchId && b.CompanyId == scope.CompanyId).Select(b => b.NameAr).SingleAsync(ct),
                    (await db.CompanySettings.AsNoTracking().SingleOrDefaultAsync(s => s.CompanyId == scope.CompanyId &&
                        s.Key == ReceiptWorkspaceService.SettingsKey && s.Status == "ACTIVE", ct)) is { } setting ? Convert.ToBase64String(setting.RowVersion) : null,
                    await db.FinancialDimensions.Where(d => d.CompanyId == scope.CompanyId && d.Status == "ACTIVE")
                        .Select(d => new ReceiptDimension(d.Id, d.DimensionCode, d.ValueCode + " — " + d.ValueNameAr, d.ValidFrom, d.ValidTo)).ToListAsync(ct),
                    await ReceiptDocuments(db, scope, ct),
                    await db.Companies.Where(c => c.Id == scope.CompanyId).Select(c => (Guid?)c.BaseCurrencyId).SingleAsync(ct),
                    await new GeneralLedgerPolicyService(db).CurrentAsync(scope, ct));
            }, ct));
        group.MapPut("/configuration", (ReceiptConfigurationUpdate update, HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, "accounting.receipts.configure", async scope =>
            {
                var c = update.Configuration;
                if (c.PostingDimension is not (null or "costCenter" or "project" or "activity"))
                    throw new InvalidOperationException("يمكن اختيار بُعد واحد فقط للترحيل.");
                string? postingSource = c.PostingDimension switch { "costCenter" => c.CostCenterDimensionCode, "project" => c.ProjectDimensionCode, "activity" => c.ActivityDimensionCode, _ => null };
                if (c.PostingDimension != null && string.IsNullOrWhiteSpace(postingSource))
                    throw new InvalidOperationException("حدد مصدر البعد المختار للترحيل.");
                if (c.ChequeTreatment is not (null or "DIRECT_BANK" or "CHEQUES_RECEIVABLE") || c.Rounding is not (null or "TO_EVEN" or "AWAY_FROM_ZERO") ||
                    c.Destinations.Any(d => d.Id == Guid.Empty || d.Kind is not ("CASH" or "BANK") || string.IsNullOrWhiteSpace(d.Label)) ||
                    c.Types.Any(t => t.Id == Guid.Empty || string.IsNullOrWhiteSpace(t.Label)) ||
                    c.Destinations.Select(d => d.Id).Distinct().Count() != c.Destinations.Count || c.Types.Select(t => t.Id).Distinct().Count() != c.Types.Count)
                    throw new InvalidOperationException("راجع حقول إعدادات القبض.");
                var accounts = c.Destinations.Select(d => d.AccountId).Where(id => id != Guid.Empty).Concat(c.ChequesReceivableAccountId.HasValue ? [c.ChequesReceivableAccountId.Value] : Array.Empty<Guid>()).Distinct().ToArray();
                if (await db.ChartOfAccounts.CountAsync(a => accounts.Contains(a.Id) && a.CompanyId == scope.CompanyId && a.Status == "ACTIVE" && a.PostingAllowed && a.DeletedAt == null, ct) != accounts.Length)
                    throw new InvalidOperationException("الحسابات المختارة غير صالحة.");
                var users = c.CollectorIds.Concat(c.SalespersonIds).Distinct().ToArray();
                if (await db.Users.CountAsync(u => users.Contains(u.Id) && u.Status == "ACTIVE" && u.DeletedAt == null && (!u.CompanyId.HasValue || u.CompanyId == scope.CompanyId), ct) != users.Length)
                    throw new InvalidOperationException("أحد المستخدمين غير متاح للشركة.");
                if (c.NumberSequenceId.HasValue && !await db.Set<NumberSequenceEntity>().AnyAsync(s => s.Id == c.NumberSequenceId && s.CompanyId == scope.CompanyId &&
                    s.DocumentType == "RECEIPT_VOUCHER" && s.Status == "ACTIVE", ct)) throw new InvalidOperationException("تسلسل القبض غير صالح.");
                if (c.DefaultCurrencyId.HasValue && !await db.Currencies.AnyAsync(x => x.Id == c.DefaultCurrencyId && x.Status == "ACTIVE", ct))
                    throw new InvalidOperationException("العملة الافتراضية غير متاحة.");
                foreach (string? code in new[] { c.CostCenterDimensionCode, c.ProjectDimensionCode, c.ActivityDimensionCode })
                    if (code != null && !await db.FinancialDimensions.AnyAsync(d => d.CompanyId == scope.CompanyId && d.DimensionCode == code && d.Status == "ACTIVE", ct))
                        throw new InvalidOperationException("مصدر البعد المالي غير متاح للشركة.");
                if (c.DefaultCostCenterId.HasValue && !await db.FinancialDimensions.AnyAsync(d => d.CompanyId == scope.CompanyId &&
                    d.Id == c.DefaultCostCenterId && d.DimensionCode == c.CostCenterDimensionCode && d.Status == "ACTIVE", ct))
                    throw new InvalidOperationException("مركز التكلفة الافتراضي غير متاح ضمن المصدر المختار.");
                var setting = await db.CompanySettings.SingleOrDefaultAsync(s => s.CompanyId == scope.CompanyId && s.Key == ReceiptWorkspaceService.SettingsKey && s.Status == "ACTIVE", ct);
                if (setting == null)
                {
                    if (update.ExpectedVersion != null) throw new DbUpdateConcurrencyException();
                    setting = new() { Id = Guid.NewGuid(), CompanyId = scope.CompanyId, Key = ReceiptWorkspaceService.SettingsKey, ValueType = "JSON", CreatedAt = DateTimeOffset.UtcNow };
                    db.CompanySettings.Add(setting);
                }
                else if (Convert.ToBase64String(setting.RowVersion) != update.ExpectedVersion) throw new DbUpdateConcurrencyException();
                setting.ValueJson = JsonSerializer.Serialize(c); setting.Version++; setting.RowVersion = Guid.NewGuid().ToByteArray(); setting.UpdatedAt = DateTimeOffset.UtcNow;
                await db.SaveChangesAsync(ct);
                return c;
            }, ct));
        group.MapGet("/", (HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, "ACC043.View", async scope => await db.ReceiptVouchers.AsNoTracking()
                .Where(v => v.CompanyId == scope.CompanyId && v.BranchId == scope.BranchId && v.DocumentJson != null)
                .OrderByDescending(v => v.CreatedAt).Take(200)
                .Select(v => new ReceiptListItem(v.Id, v.VoucherNo, v.VoucherDate, v.Status, v.Amount)).ToListAsync(ct), ct));
        group.MapGet("/{id:guid}", (Guid id, HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, "ACC043.View", async scope => await new ReceiptWorkspaceService(db).GetAsync(scope, id, ct), ct));
        group.MapPost("/{id:guid}/attachments", (Guid id, ReceiptAttachmentUpload upload, HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, "ACC043.Edit", scope => new ReceiptWorkspaceService(db).AddAttachmentAsync(scope, id, upload, ct), ct));
        group.MapGet("/{id:guid}/attachments/{attachmentId:guid}", (Guid id, Guid attachmentId, HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, "ACC043.View", scope => new ReceiptWorkspaceService(db).DownloadAttachmentAsync(scope, id, attachmentId, ct), ct));
        group.MapPut("/", (ReceiptDraft draft, HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, draft.ExpectedVersion == null ? "ACC043.Create" : "ACC043.Edit",
                async scope => await CompleteReceipt(db, scope, await new ReceiptWorkspaceService(db).SaveAsync(scope, draft, ct), ct), ct));
        group.MapPost("/{id:guid}/review", (Guid id, Transition request, HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, "accounting.receipts.review", async scope => await CompleteReceipt(db, scope, await new ReceiptWorkspaceService(db).ReviewAsync(scope, id, request.Version, ct), ct), ct));
        group.MapPost("/{id:guid}/approve", (Guid id, Transition request, HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, "accounting.receipts.approve", async scope => await CompleteReceipt(db, scope, await new ReceiptWorkspaceService(db).ApproveAsync(scope, id, request.Version, ct), ct), ct));
        group.MapPost("/{id:guid}/post", (Guid id, Transition request, HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, "ACC043.Post", async scope => await new ReceiptWorkspaceService(db).PostAsync(scope, id, request.Version, ct), ct));
        group.MapPost("/{id:guid}/cancel", (Guid id, Transition request, HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, "ACC043.Cancel", async scope => await new ReceiptWorkspaceService(db).CancelAsync(scope, id, request.Version, request.Reason ?? "", ct), ct));
        group.MapPost("/{id:guid}/reverse", (Guid id, Transition request, HttpContext http, TransportErpDbContext db, CancellationToken ct) =>
            Execute(http, db, "ACC043.Reverse", async scope => await new ReceiptWorkspaceService(db).ReverseAsync(scope, id, request.Version, request.Reason ?? "",
                request.Date ?? throw new InvalidOperationException("تاريخ العكس مطلوب."), ct), ct));
    }

    private static async Task<List<ReceiptLinkedDocument>> ReceiptDocuments(TransportErpDbContext db, OperationContext scope, CancellationToken ct)
    {
        var result = await db.Set<WaybillEntity>().Where(w => w.CompanyId == scope.CompanyId && w.BranchId == scope.BranchId)
            .Select(w => new ReceiptLinkedDocument("WAYBILL", w.Id, "بوليصة — " + (w.WaybillNo ?? w.DraftNo))).ToListAsync(ct);
        result.AddRange(await db.ReceiptVouchers.Where(v => v.CompanyId == scope.CompanyId && v.BranchId == scope.BranchId)
            .Select(v => new ReceiptLinkedDocument("RECEIPT", v.Id, "سند قبض — " + v.VoucherNo)).ToListAsync(ct));
        result.AddRange(await db.JournalEntries.Where(v => v.CompanyId == scope.CompanyId && v.BranchId == scope.BranchId)
            .Select(v => new ReceiptLinkedDocument("JOURNAL", v.Id, "قيد — " + v.DocumentNo)).ToListAsync(ct));
        return result;
    }

    private static async Task<IResult> Execute<T>(HttpContext http, TransportErpDbContext db, string permission,
        Func<OperationContext, Task<T>> action, CancellationToken ct)
    {
        if (!Guid.TryParse(http.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? http.User.FindFirstValue("sub"), out var user) ||
            !Guid.TryParse(http.User.FindFirstValue("company_id"), out var company) ||
            !Guid.TryParse(http.User.FindFirstValue("branch_id"), out var branch)) return Results.Unauthorized();
        var scope = new OperationContext(user, company, branch, Guid.NewGuid());
        bool permitted = permission == GeneralLedgerPolicyService.ConfigurePermission
            ? await Allowed(db, scope, permission, ct, companyWide: true)
            : await Allowed(db, scope, permission, ct);
        if (!permitted && permission == "accounting.general-ledger.view")
            permitted = await Allowed(db, scope, GeneralLedgerPolicyService.ConfigurePermission, ct, companyWide: true);
        if (!permitted) return Results.StatusCode(403);
        try
        {
            var result = await action(scope);
            if (result is ReceiptDocument receipt) return Results.Ok((await new ReceiptWorkspaceService(db).GetAsync(scope, receipt.Draft.Id, ct)) with { WorkflowMessage = receipt.WorkflowMessage });
            return Results.Ok(result);
        }
        catch (KeyNotFoundException) { return Results.NotFound(); }
        catch (UnauthorizedAccessException) { return Results.StatusCode(403); }
        catch (DbUpdateConcurrencyException) { return Results.Conflict(new { message = "تغير السند؛ أعد تحميله قبل المتابعة." }); }
        catch (DbUpdateException) { return Results.Conflict(new { message = "تعذر حفظ العملية بسبب تعارض. أعد تحميل السند قبل إعادة الإرسال." }); }
        catch (InvalidOperationException ex) { return Results.BadRequest(new { message = ex.Message }); }
        catch (OverflowException) { return Results.BadRequest(new { message = "المبالغ تتجاوز الدقة المسموحة." }); }
    }

    // Re-read current scoped permissions rather than trusting a stale token's permission claims.
    internal static async Task<bool> Allowed(TransportErpDbContext db, OperationContext scope, string code, CancellationToken ct, bool companyWide = false)
    {
        var user = await db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == scope.UserId && u.Status == "ACTIVE" && u.DeletedAt == null, ct);
        if (user == null || (user.CompanyId.HasValue && user.CompanyId != scope.CompanyId) ||
            (user.BranchId.HasValue && user.BranchId != scope.BranchId) || (companyWide && user.BranchId.HasValue)) return false;
        var overrides = await (from o in db.UserPermissionOverrides join p in db.Permissions on o.PermissionId equals p.Id
            where o.UserId == scope.UserId && p.Code == code && p.Status == "ACTIVE" && p.ScopeType != "PLATFORM" &&
                (!o.CompanyId.HasValue || o.CompanyId == scope.CompanyId) && (!o.BranchId.HasValue || o.BranchId == scope.BranchId)
            where !companyWide || o.BranchId == null || !o.IsAllowed
            select o.IsAllowed).ToListAsync(ct);
        if (overrides.Contains(false)) return false;
        var roleAllowed = await (from ur in db.UserRoles join r in db.Roles on ur.RoleId equals r.Id
            join rp in db.RolePermissions on r.Id equals rp.RoleId join p in db.Permissions on rp.PermissionId equals p.Id
            where ur.UserId == scope.UserId && r.Status == "ACTIVE" && p.Status == "ACTIVE" && p.Code == code &&
                (ur.CompanyId ?? r.CompanyId ?? user.CompanyId) == scope.CompanyId &&
                (!r.CompanyId.HasValue || r.CompanyId == scope.CompanyId) && (!ur.CompanyId.HasValue || ur.CompanyId == scope.CompanyId) &&
                (!ur.BranchId.HasValue || ur.BranchId == scope.BranchId) && (!rp.CompanyId.HasValue || rp.CompanyId == scope.CompanyId) &&
                (!rp.BranchId.HasValue || rp.BranchId == scope.BranchId) && rp.ScopeType != "PLATFORM" && p.ScopeType != "PLATFORM"
                && (!companyWide || (ur.BranchId == null && rp.BranchId == null))
            select p.Id).AnyAsync(ct);
        return roleAllowed || overrides.Contains(true);
    }
}
