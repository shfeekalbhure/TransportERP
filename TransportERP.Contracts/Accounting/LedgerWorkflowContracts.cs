namespace TransportERP.Contracts.Accounting;

public sealed record LedgerWorkflowPolicy(bool RequireReview, bool RequireApproval, string PostingMode)
{
    // Absent in older settings: unknown, not an implicit enabled/disabled value.
    public bool? RequireJournalDescription { get; init; }
    public static LedgerWorkflowPolicy Legacy => new(false, true, "MANUAL");
    public string ReadyState => RequireApproval ? "APPROVED" : RequireReview ? "REVIEWED" : "DRAFT";
    public bool IsReady(string state) => state == ReadyState;
    public string Description => "حفظ مسودة" + (RequireReview ? " ← مراجعة المحاسب" : "") +
        (RequireApproval ? " ← اعتماد المسؤول" : "") +
        (PostingMode == "AUTOMATIC" ? " ← ترحيل تلقائي عند اكتمال المراحل وبصلاحية الترحيل" : " ← ترحيل يدوي بصلاحية الترحيل");
}
public sealed record LedgerWorkflowSnapshot(LedgerWorkflowPolicy Policy, string? Version, bool IsLegacy = false);
public sealed record LedgerSettingsUpdate(LedgerWorkflowPolicy Policy, string? ExpectedVersion);
public sealed record LedgerSettingsDocument(LedgerWorkflowPolicy? Policy, string? Version, string CompanyName,
    string BranchName, bool CanConfigure, List<ReceiptAuditItem> Audit, List<string> SupportedDocumentTypes);
