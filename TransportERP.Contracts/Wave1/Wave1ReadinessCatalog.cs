namespace TransportERP.Contracts.Wave1;

public enum Wave1ReadinessState
{
    ImplementedReviewRequired,
    Hold
}

public sealed record Wave1ReadinessEntry(
    string ScreenId,
    Wave1ReadinessState State,
    string Gate,
    string EvidenceBasis);

public static class Wave1ReadinessCatalog
{
    private static readonly IReadOnlyDictionary<string, Wave1ReadinessEntry> Entries =
        new Dictionary<string, Wave1ReadinessEntry>(StringComparer.OrdinalIgnoreCase)
        {
            ["GEN-003"] = Hold("GEN-003", "W1_PHYSICAL_PROMOTION", "SRC-055 closes the full logical/W2/W3 specification, but ISO2/ISO3/DialingCode physical promotion is not present in the current governing W1 physical contract; the V1.3 DB reconciliation source is candidate-only."),
            ["GEN-004"] = Review("GEN-004", "EXACT_SHA_RUNTIME_REVIEW", "SRC-056 closes the independent Governorate specification; current geography implementation still requires exact-SHA runtime and independent review."),
            ["GEN-005"] = Review("GEN-005", "EXACT_SHA_RUNTIME_REVIEW", "SRC-056 closes the independent Directorate specification; current geography implementation still requires exact-SHA runtime and independent review."),
            ["GEN-006"] = Review("GEN-006", "EXACT_SHA_RUNTIME_REVIEW", "SRC-056 closes the independent City specification; current geography implementation still requires exact-SHA runtime and independent review."),
            ["GEN-007"] = Review("GEN-007", "EXACT_SHA_RUNTIME_REVIEW", "SRC-056 closes the independent Area specification; current geography implementation still requires exact-SHA runtime and independent review."),
            ["GEN-013"] = Hold("GEN-013", "W1_W3_FIELD_SEMANTICS", "Current W1 NumberSequence persists CompanyId/BranchId/FiscalYearId/DocumentType/NextValue/ResetRule/format/IsActive/Version, but does not persist Code/ArabicName/EnglishName/Notes; Scope needs explicit binding and no governing derivation permits LastNumber to be silently equated to NextValue."),
            ["GEN-014"] = Review("GEN-014", "EXACT_SHA_INDEPENDENT_REVIEW", "Current W1/W2 contract is implemented as Id/Code/CultureCode/Direction/IsActive/Version with List/Get/Create/Update/Disable only; translations and display-name fields were removed. Runtime tests cover paging cap, filter, uniqueness, concurrency, disable and audit; exact-final-SHA CI and independent review remain required."),
            ["ACC-036"] = Hold("ACC-036", "W1_W2_ENTITY_DTO_RECONCILIATION", "Current W1 physical contract defines separate AccountGroup (Code/ArabicName/EnglishName/IsActive/Version) and AccountType (Code/NormalBalance/IsActive/Version) entities. W2 exact route and DTO type names are proven, but the exact DTO field schema/entity discrimination is not proven; the branch currently merges both into one AccountClassification entity and must not be promoted by inference."),
            ["ACC-074"] = Hold("ACC-074", "OPEN_ITEM_SOURCE_RECONCILIATION", "Current W1 physical OpenItem uses PartyType plus conditional CustomerId/SupplierId, SourceDocumentType/Id, JournalLineId, OriginalAmount/AllocatedAmount, DueDate, Status and Version, with PaymentAllocation as a separate source. W2 also requires PartyCode/PartyName and source-document display fields, so authoritative joins are required. The branch currently denormalizes copied party/document fields and diverges from W1."),
            ["ACC-075"] = Hold("ACC-075", "OPEN_ITEM_SOURCE_RECONCILIATION", "Current W1 physical OpenItem uses PartyType plus conditional CustomerId/SupplierId, SourceDocumentType/Id, JournalLineId, OriginalAmount/AllocatedAmount, DueDate, Status and Version, with PaymentAllocation as a separate source. W2 also requires PartyCode/PartyName and source-document display fields, so authoritative joins are required. The branch currently denormalizes copied party/document fields and diverges from W1."),
            ["ACC-049"] = Hold("ACC-049", "ACCOUNTING_E2E_RECONCILIATION", "Balance sheet source-of-truth is posted JournalEntry/JournalLine/Account/AccountType; exact-SHA period/currency/reversal/scope and drill-down reconciliation evidence remains required."),
            ["ACC-050"] = Hold("ACC-050", "OTS_W1_005_CASH_FLOW_CLASSIFICATION", "AP-A4-002 closes only an OFFICIAL BASELINE CANDIDATE and explicitly grants no implementation/programming/final-freeze authority. Current Approved V1.25 does not promote it. The existing ReferenceType heuristic is therefore non-governing and cannot earn READY."),
            ["ACC-058"] = Hold("ACC-058", "ACCOUNTING_E2E_RECONCILIATION", "Detailed trial balance source-of-truth is posted JournalEntry/JournalLine/Account; exact-SHA period/currency/reversal/scope plus drill-down/export/print reconciliation evidence remains required.")
        };

    public static IReadOnlyCollection<Wave1ReadinessEntry> All => Entries.Values.ToArray();

    public static Wave1ReadinessEntry GetRequired(string screenId)
        => Entries.TryGetValue(screenId, out var value)
            ? value
            : throw new KeyNotFoundException($"No WAVE-1 readiness entry for '{screenId}'.");

    public static bool HasMergeBlockers => Entries.Values.Any(x => x.State == Wave1ReadinessState.Hold);

    private static Wave1ReadinessEntry Hold(string id, string gate, string evidence)
        => new(id, Wave1ReadinessState.Hold, gate, evidence);

    private static Wave1ReadinessEntry Review(string id, string gate, string evidence)
        => new(id, Wave1ReadinessState.ImplementedReviewRequired, gate, evidence);
}
