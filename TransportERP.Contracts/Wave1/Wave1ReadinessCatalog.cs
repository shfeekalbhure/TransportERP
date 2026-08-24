namespace TransportERP.Contracts.Wave1;

public enum Wave1ReadinessState
{
    ImplementedReviewRequired,
    Hold
}

public sealed record Wave1ReadinessEntry(string ScreenId, Wave1ReadinessState State, string Gate, string EvidenceBasis);

public static class Wave1ReadinessCatalog
{
    private static readonly IReadOnlyDictionary<string, Wave1ReadinessEntry> Entries =
        new Dictionary<string, Wave1ReadinessEntry>(StringComparer.OrdinalIgnoreCase)
        {
            ["GEN-003"] = Review("GEN-003", "OD-W1-01 promotes ISO2/ISO3/DialingCode physical persistence; dedicated country runtime and migration implement the exact Current W2 route family including print."),
            ["GEN-004"] = Review("GEN-004", "SRC-056 closes the independent Governorate specification; current geography implementation requires exact-final-SHA independent review."),
            ["GEN-005"] = Review("GEN-005", "SRC-056 closes the independent Directorate specification; current geography implementation requires exact-final-SHA independent review."),
            ["GEN-006"] = Review("GEN-006", "SRC-056 closes the independent City specification; current geography implementation requires exact-final-SHA independent review."),
            ["GEN-007"] = Review("GEN-007", "SRC-056 closes the independent Area specification; current geography implementation requires exact-final-SHA independent review."),
            ["GEN-013"] = Review("GEN-013", "OD-W1-02 closes metadata persistence and NextValue→LastNumber semantics. Runtime keeps NextValue as hidden allocation cursor, derives LastNumber without loss and preserves atomic/non-reuse rules."),
            ["GEN-014"] = Review("GEN-014", "Current W1/W2 Language contract is isolated in Wave1LanguageService with exact List/Get/Create/Update/Disable behavior."),
            ["ACC-036"] = Review("ACC-036", "OD-W1-03 closes the field-level contract with separate AccountGroup and AccountType physical entities and a Kind-discriminated exact ACC-036 API surface."),
            ["ACC-074"] = Review("ACC-074", "OD-W1-04 closes the normalized Customer/OpenItem/PaymentAllocation/source-document chain; runtime joins party and document display data rather than persisting denormalized copies."),
            ["ACC-075"] = Review("ACC-075", "OD-W1-04 closes the normalized Supplier/OpenItem/PaymentAllocation/source-document chain; runtime joins party and document display data rather than persisting denormalized copies."),
            ["ACC-049"] = Review("ACC-049", "Dedicated balance-sheet runtime and E2E evidence cover posted/reversal, branch/currency isolation, drill-down, export/print and cap 200."),
            ["ACC-050"] = Review("ACC-050", "OD-W1-05 promotes account-default + controlled movement override cash-flow classification. ReferenceType heuristics are not used by the authorized runtime; ambiguous/unmapped movements are explicit UNCLASSIFIED."),
            ["ACC-058"] = Review("ACC-058", "Dedicated detailed-trial-balance runtime and E2E evidence cover posted/reversal, branch/currency isolation, drill-down, export/print and cap 200.")
        };

    public static IReadOnlyCollection<Wave1ReadinessEntry> All => Entries.Values.ToArray();
    public static Wave1ReadinessEntry GetRequired(string screenId)
        => Entries.TryGetValue(screenId, out var value) ? value : throw new KeyNotFoundException($"No WAVE-1 readiness entry for '{screenId}'.");
    public static bool HasMergeBlockers => Entries.Values.Any(x => x.State == Wave1ReadinessState.Hold);

    private static Wave1ReadinessEntry Review(string id, string evidence)
        => new(id, Wave1ReadinessState.ImplementedReviewRequired, "EXACT_SHA_INDEPENDENT_REVIEW", evidence);
}
