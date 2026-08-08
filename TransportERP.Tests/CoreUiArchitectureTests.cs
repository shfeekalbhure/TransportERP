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
        invalid[TransportScreenProfile.Settings] = typeof(MasterDataReferenceScreen);

        Assert.Throws<InvalidOperationException>(() => CoreUiReferenceScreenCatalog.ValidateMappings(invalid));
    }
}
