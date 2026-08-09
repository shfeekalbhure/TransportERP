using TransportERP.Desktop.CoreUI.Architecture;

namespace TransportERP.Tests;

public sealed class CoreUiArchitectureTests
{
    [Fact]
    public void Every_frozen_profile_has_one_concrete_CoreUI_reference_screen()
    {
        CoreUiReferenceScreenCatalog.Validate();

        Assert.Equal(6, CoreUiReferenceScreenCatalog.All.Count);
        Assert.All(
            CoreUiReferenceScreenCatalog.All.Values,
            type => Assert.True(typeof(CoreUiReferenceScreen).IsAssignableFrom(type)));
    }

    [Fact]
    public void Missing_profile_mapping_is_rejected()
    {
        var invalid = CoreUiReferenceScreenCatalog.All
            .Where(entry => entry.Key != TransportScreenProfile.Transaction)
            .ToDictionary(entry => entry.Key, entry => entry.Value);

        Assert.Throws<InvalidOperationException>(() => CoreUiReferenceScreenCatalog.ValidateMappings(invalid));
    }

    [Fact]
    public void Duplicate_reference_screen_is_rejected()
    {
        var invalid = CoreUiReferenceScreenCatalog.All.ToDictionary(entry => entry.Key, entry => entry.Value);
        invalid[TransportScreenProfile.Settings] = typeof(Gen003CountriesReferenceScreen);

        Assert.Throws<InvalidOperationException>(() => CoreUiReferenceScreenCatalog.ValidateMappings(invalid));
    }

    [Fact]
    public void Reference_screens_cover_the_six_named_W3_pilot_screens()
    {
        var definitions = CoreUiReferenceScreenCatalog.All.Values
            .Select(type => ((CoreUiReferenceScreen)Activator.CreateInstance(type)!).Definition)
            .ToDictionary(definition => definition.Code, definition => definition.Profile);

        Assert.Equal(6, definitions.Count);
        Assert.Equal(TransportScreenProfile.MasterData, definitions["GEN-003"]);
        Assert.Equal(TransportScreenProfile.TreeMaster, definitions["ACC-035"]);
        Assert.Equal(TransportScreenProfile.ControlApproval, definitions["ACC-041"]);
        Assert.Equal(TransportScreenProfile.Transaction, definitions["ACC-042"]);
        Assert.Equal(TransportScreenProfile.ReportInquiry, definitions["ACC-046"]);
        Assert.Equal(TransportScreenProfile.Settings, definitions["GEN-015"]);
    }
}
