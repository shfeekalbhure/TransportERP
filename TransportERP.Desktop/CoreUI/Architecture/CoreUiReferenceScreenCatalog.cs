namespace TransportERP.Desktop.CoreUI.Architecture;

/// <summary>
/// Machine-verifiable profile-to-screen traceability. Build-time tests call this catalog,
/// so a missing, duplicate or non-CoreUI reference is a test failure rather than a narrative gap.
/// </summary>
public static class CoreUiReferenceScreenCatalog
{
    private static readonly IReadOnlyDictionary<TransportScreenProfile, Type> ReferenceTypes =
        new Dictionary<TransportScreenProfile, Type>
        {
            [TransportScreenProfile.MasterData] = typeof(Gen003CountriesReferenceScreen),
            [TransportScreenProfile.TreeMaster] = typeof(Acc035ChartOfAccountsReferenceScreen),
            [TransportScreenProfile.Transaction] = typeof(Acc042JournalEntryReferenceScreen),
            [TransportScreenProfile.ControlApproval] = typeof(Acc041AccountingPeriodsReferenceScreen),
            [TransportScreenProfile.ReportInquiry] = typeof(Acc046TrialBalanceReferenceScreen),
            [TransportScreenProfile.Settings] = typeof(Gen015OperationalSettingsReferenceScreen)
        };

    public static IReadOnlyDictionary<TransportScreenProfile, Type> All => ReferenceTypes;

    public static void Validate()
    {
        ValidateMappings(ReferenceTypes);
    }

    public static void ValidateMappings(IReadOnlyDictionary<TransportScreenProfile, Type> mappings)
    {
        var expectedProfiles = Enum.GetValues<TransportScreenProfile>();

        if (mappings.Count != expectedProfiles.Length ||
            expectedProfiles.Any(profile => !mappings.ContainsKey(profile)))
        {
            throw new InvalidOperationException("Each frozen TransportERP screen profile must have exactly one CoreUI reference screen.");
        }

        if (mappings.Values.Distinct().Count() != expectedProfiles.Length ||
            mappings.Values.Any(type => !typeof(CoreUiReferenceScreen).IsAssignableFrom(type) || type.IsAbstract))
        {
            throw new InvalidOperationException("Every profile reference must be a distinct concrete CoreUiReferenceScreen.");
        }
    }
}
