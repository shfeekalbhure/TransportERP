using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Accounting;
using TransportERP.Contracts.Core;

namespace TransportERP.Infrastructure.Persistence;

public sealed class AccountingWorkflowSnapshot
{
    public Guid DocumentId { get; set; }
    public string DocumentType { get; set; } = "";
    public Guid CompanyId { get; set; }
    public Guid BranchId { get; set; }
    public string PolicyJson { get; set; } = "";
    public DateTimeOffset CreatedAt { get; set; }
}

// Company-wide policy. No implicit branch override or migration of existing documents.
public sealed class GeneralLedgerPolicyService(TransportErpDbContext db)
{
    public const string SettingsKey = "accounting.general-ledger.workflow";
    public const string ConfigurePermission = "accounting.general-ledger.configure";
    public static LedgerWorkflowSnapshot Legacy => new(LedgerWorkflowPolicy.Legacy, null, true);
    private async Task ScopeAsync(OperationContext scope, CancellationToken ct)
    {
        scope.EnsureComplete();
        if (!await db.Companies.AnyAsync(c => c.Id == scope.CompanyId && c.Status == "ACTIVE", ct) ||
            !await db.Branches.AnyAsync(b => b.Id == scope.BranchId && b.CompanyId == scope.CompanyId && b.Status == "ACTIVE", ct))
            throw new UnauthorizedAccessException("نطاق الشركة والفرع غير صالح.");
    }
    public async Task<LedgerWorkflowSnapshot> CurrentAsync(OperationContext scope, CancellationToken ct = default)
    {
        await ScopeAsync(scope, ct);
        var setting = await db.CompanySettings.AsNoTracking().SingleOrDefaultAsync(s => s.CompanyId == scope.CompanyId && s.Key == SettingsKey && s.Status == "ACTIVE", ct);
        return setting == null ? Legacy : new(ReadPolicy(setting.ValueJson), Convert.ToBase64String(setting.RowVersion));
    }
    public async Task<LedgerSettingsDocument> GetSettingsAsync(OperationContext scope, bool canConfigure, CancellationToken ct = default)
    {
        var current = await CurrentAsync(scope, ct);
        var audit = await (from a in db.AuditEvents.AsNoTracking()
            join u in db.Users on a.ActorUserId equals u.Id into users from u in users.DefaultIfEmpty()
            where a.CompanyId == scope.CompanyId && a.EntityType == "GeneralLedgerSettings"
            orderby a.OccurredAt descending
            select new ReceiptAuditItem(a.Action, u == null ? "—" : u.DisplayName, a.OccurredAt, a.Reason)).Take(50).ToListAsync(ct);
        return new(current.IsLegacy ? null : current.Policy, current.Version,
            await db.Companies.Where(c => c.Id == scope.CompanyId).Select(c => c.LegalNameAr).SingleAsync(ct),
            await db.Branches.Where(b => b.Id == scope.BranchId && b.CompanyId == scope.CompanyId).Select(b => b.NameAr).SingleAsync(ct),
            canConfigure, audit, ["RECEIPT_VOUCHER"]);
    }
    public async Task SaveSettingsAsync(OperationContext scope, LedgerSettingsUpdate update, CancellationToken ct = default)
    {
        await ScopeAsync(scope, ct); Validate(update.Policy);
        await using var transaction = db.Database.IsRelational() ? await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct) : null;
        var setting = await db.CompanySettings.SingleOrDefaultAsync(s => s.CompanyId == scope.CompanyId && s.Key == SettingsKey && s.Status == "ACTIVE", ct);
        string? before = setting?.ValueJson;
        if (setting == null)
        {
            if (update.ExpectedVersion != null) throw new DbUpdateConcurrencyException();
            setting = new() { Id = Guid.NewGuid(), CompanyId = scope.CompanyId, Key = SettingsKey, ValueType = "JSON", CreatedAt = DateTimeOffset.UtcNow };
            db.CompanySettings.Add(setting);
        }
        else if (Convert.ToBase64String(setting.RowVersion) != update.ExpectedVersion) throw new DbUpdateConcurrencyException();
        setting.ValueJson = JsonSerializer.Serialize(update.Policy); setting.Version++;
        setting.RowVersion = Guid.NewGuid().ToByteArray(); setting.UpdatedAt = DateTimeOffset.UtcNow;
        await new AuditEventService(db).AppendAccountingInTransactionAsync(new("ConfigureGeneralLedger", "SUCCESS", "GeneralLedgerSettings", setting.Id,
            scope.UserId, scope.CompanyId, scope.BranchId, scope.CorrelationId, BeforeJson: before, AfterJson: setting.ValueJson,
            Reason: "سياسة الشركة للمستندات الجديدة؛ لا تغير المستندات القائمة."), ct);
        await db.SaveChangesAsync(ct); if (transaction != null) await transaction.CommitAsync(ct);
    }
    public async Task BindNewAsync(OperationContext scope, string documentType, Guid documentId, CancellationToken ct = default)
    {
        var snapshot = await CurrentAsync(scope, ct);
        db.Add(new AccountingWorkflowSnapshot { DocumentId = documentId, DocumentType = documentType, CompanyId = scope.CompanyId,
            BranchId = scope.BranchId, PolicyJson = JsonSerializer.Serialize(snapshot), CreatedAt = DateTimeOffset.UtcNow });
    }
    public async Task<LedgerWorkflowSnapshot> ForDocumentAsync(OperationContext scope, string type, Guid id, CancellationToken ct = default)
    {
        var saved = db.Set<AccountingWorkflowSnapshot>().Local.FirstOrDefault(s => s.DocumentId == id && s.DocumentType == type)
            ?? await db.Set<AccountingWorkflowSnapshot>().AsNoTracking().SingleOrDefaultAsync(s => s.DocumentId == id && s.DocumentType == type, ct);
        if (saved == null) return Legacy;
        if (saved.CompanyId != scope.CompanyId || saved.BranchId != scope.BranchId) throw new UnauthorizedAccessException();
        var snapshot = JsonSerializer.Deserialize<LedgerWorkflowSnapshot>(saved.PolicyJson) ?? throw new InvalidOperationException("نسخة السياسة غير صالحة.");
        Validate(snapshot.Policy); return snapshot;
    }
    private static LedgerWorkflowPolicy ReadPolicy(string json)
    { var p = JsonSerializer.Deserialize<LedgerWorkflowPolicy>(json) ?? throw new InvalidOperationException("سياسة الأستاذ العام غير صالحة."); Validate(p); return p; }
    private static void Validate(LedgerWorkflowPolicy policy)
    { if (policy.PostingMode is not ("MANUAL" or "AUTOMATIC")) throw new InvalidOperationException("اختر ترحيلًا يدويًا أو تلقائيًا."); }
}
