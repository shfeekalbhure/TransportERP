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
            ["GEN-003"] = Hold("GEN-003", "W1_PHYSICAL_PROMOTION", "SRC-055 closes full specification design, but ISO2/ISO3/DialingCode physical promotion remains outside the current W1 physical contract."),
            ["GEN-004"] = Review("GEN-004", "EXACT_SHA_RUNTIME_REVIEW", "SRC-056 closes the independent Governorate specification; existing geography implementation still requires exact-SHA runtime and independent review."),
            ["GEN-005"] = Review("GEN-005", "EXACT_SHA_RUNTIME_REVIEW", "SRC-056 closes the independent Directorate specification; existing geography implementation still requires exact-SHA runtime and independent review."),
            ["GEN-006"] = Review("GEN-006", "EXACT_SHA_RUNTIME_REVIEW", "SRC-056 closes the independent City specification; existing geography implementation still requires exact-SHA runtime and independent review."),
            ["GEN-007"] = Review("GEN-007", "EXACT_SHA_RUNTIME_REVIEW", "SRC-056 closes the independent Area specification; existing geography implementation still requires exact-SHA runtime and independent review."),
            ["GEN-013"] = Hold("GEN-013", "W1_W3_FIELD_SEMANTICS", "Current W1 does not persist Code/ArabicName/EnglishName/Notes and no governing derivation allows LastNumber to be equated to NextValue."),
            ["GEN-014"] = Hold("GEN-014", "CONTRACT_CODE_PARITY", "SRC-057 requires Code/CultureCode/Direction; current implementation must be reconciled to that exact contract before READY."),
            ["ACC-036"] = Hold("ACC-036", "W1_PHYSICAL_FIELD_MAPPING", "Field-level W1 physical mappings remain unresolved for current account-classification fields; no inferred schema is allowed."),
            ["ACC-074"] = Hold("ACC-074", "OPEN_ITEM_SOURCE_RECONCILIATION", "Customer aging requires authoritative Customer/OpenItem/PaymentAllocation source and exact-SHA reconciliation evidence."),
            ["ACC-075"] = Hold("ACC-075", "OPEN_ITEM_SOURCE_RECONCILIATION", "Supplier aging requires authoritative Supplier/OpenItem/PaymentAllocation source and exact-SHA reconciliation evidence."),
            ["ACC-049"] = Hold("ACC-049", "ACCOUNTING_E2E_RECONCILIATION", "Balance sheet requires posted-journal source-of-truth and end-to-end accounting reconciliation evidence on the exact SHA."),
            ["ACC-050"] = Hold("ACC-050", "CASH_FLOW_SOURCE_RECONCILIATION", "Cash flow requires an approved source/classification contract and posted accounting reconciliation; heuristic classification cannot earn READY."),
            ["ACC-058"] = Hold("ACC-058", "ACCOUNTING_E2E_RECONCILIATION", "Detailed trial balance requires posted-journal source-of-truth and end-to-end accounting reconciliation evidence on the exact SHA.")
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
