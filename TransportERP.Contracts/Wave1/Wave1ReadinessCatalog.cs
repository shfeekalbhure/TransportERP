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
            ["GEN-013"] = Hold("GEN-013", "W1_W3_FIELD_SEMANTICS", "Owner/business numbering semantics are partially resolved, but the governing reconciliation explicitly leaves legacy NextValue to separate LastNumber supersession/migration plus physical scope/FK/approval/concurrency evidence open. No silent persistence promotion is permitted."),
            ["GEN-014"] = Review("GEN-014", "EXACT_SHA_INDEPENDENT_REVIEW", "Current W1/W2 contract is implemented as Id/Code/CultureCode/Direction/IsActive/Version with List/Get/Create/Update/Disable only; translations and display-name fields were removed. Runtime tests cover paging cap, filter, uniqueness, concurrency, disable and audit; exact-final-SHA CI and independent review remain required."),
            ["ACC-036"] = Hold("ACC-036", "W1_W2_ENTITY_DTO_RECONCILIATION", "Current W1 physical contract defines separate AccountGroup and AccountType entities. Exact field-level DTO schema/entity discrimination and a separately authorized physical implementation contract remain NOT PROVEN; candidate/derived field design cannot authorize runtime."),
            ["ACC-074"] = Hold("ACC-074", "OPEN_ITEM_SOURCE_RECONCILIATION", "Current W1 physical OpenItem/PaymentAllocation source chain and the authoritative Customer/source-document joins required by W2 remain NOT PROVEN. Candidate read-model mappings do not authorize denormalized runtime persistence."),
            ["ACC-075"] = Hold("ACC-075", "OPEN_ITEM_SOURCE_RECONCILIATION", "Current W1 physical OpenItem/PaymentAllocation source chain and the authoritative Supplier/source-document joins required by W2 remain NOT PROVEN. Candidate read-model mappings do not authorize denormalized runtime persistence."),
            ["ACC-049"] = Review("ACC-049", "EXACT_SHA_INDEPENDENT_REVIEW", "Dedicated balance-sheet runtime uses posted JournalEntry/JournalLine/Account data only; exact tests cover original-plus-reversal accounting semantics, branch/currency isolation, drill-down, export/print payloads and PageSize cap 200. Exact-final-SHA CI and independent review remain required."),
            ["ACC-050"] = Hold("ACC-050", "OTS_W1_005_CASH_FLOW_CLASSIFICATION", "AP-A4-002 closes only an OFFICIAL BASELINE CANDIDATE and explicitly grants no implementation/programming/final-freeze authority. The existing ReferenceType heuristic is non-governing and cannot earn READY."),
            ["ACC-058"] = Review("ACC-058", "EXACT_SHA_INDEPENDENT_REVIEW", "Dedicated detailed-trial-balance runtime uses posted JournalEntry/JournalLine/Account data only; exact tests cover original-plus-reversal accounting semantics, branch/currency isolation, drill-down, export/print payloads and PageSize cap 200. Exact-final-SHA CI and independent review remain required.")
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
