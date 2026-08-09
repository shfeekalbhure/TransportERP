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
    public void Frozen_reference_screens_have_real_screen_definitions()
    {
        Assert.Equal(6, ScreenDefinitions.All.Count);
        Assert.Equal(6, ScreenDefinitions.All.Select(definition => definition.Code).Distinct().Count());
        Assert.All(ScreenDefinitions.All, definition =>
        {
            Assert.NotEmpty(definition.Fields);
            Assert.False(string.IsNullOrWhiteSpace(definition.Variant));
        });
        Assert.Equal("ScopedSettings", ScreenDefinitions.Gen015.Variant);
        Assert.DoesNotContain("CurrencyCode", ScreenDefinitions.Gen003.Fields);
        Assert.True(ScreenDefinitions.Acc042.IsReadOnly);
        Assert.True(ScreenDefinitions.Acc046.IsReadOnly);
    }

    [Fact]
    public void Every_runtime_reference_exposes_its_screen_definition()
    {
        foreach (var type in CoreUiReferenceScreenCatalog.All.Values)
        {
            var screen = (CoreUiReferenceScreen)Activator.CreateInstance(type)!;
            Assert.Equal(screen.Profile, screen.Definition.Profile);
            Assert.NotEmpty(screen.Definition.Code);
            Assert.Equal(AutoScaleMode.Dpi, screen.AutoScaleMode);
        }
    }
}
