using TransportERP.Contracts.Wave1;

namespace TransportERP.Tests;

public sealed class Wave1ScreenCatalogTests
{
    [Fact]
    public void Catalog_contains_exactly_ten_unique_wave1_screens()
    {
        Assert.Equal(10, Wave1ScreenCatalog.All.Count);
        Assert.Equal(10, Wave1ScreenCatalog.All.Select(x => x.ScreenId).Distinct(StringComparer.OrdinalIgnoreCase).Count());
    }

    [Theory]
    [InlineData("SET-001", "MasterData", "Standard")]
    [InlineData("SET-002", "MasterData", "Standard")]
    [InlineData("SET-011", "Settings", "NumberingControlled")]
    [InlineData("SET-013", "MasterData", "Standard")]
    [InlineData("FIN-003", "MasterData", "Standard")]
    [InlineData("FIN-028", "ReportInquiry", "Aging")]
    [InlineData("FIN-029", "ReportInquiry", "Aging")]
    [InlineData("FIN-042", "ReportInquiry", "Report")]
    [InlineData("FIN-043", "ReportInquiry", "Report")]
    [InlineData("FIN-055", "ReportInquiry", "Report")]
    public void Profile_and_variant_are_governing(string screenId, string profile, string variant)
    {
        var screen = Wave1ScreenCatalog.GetRequired(screenId);
        Assert.Equal(profile, screen.Profile);
        Assert.Equal(variant, screen.Variant);
        Assert.True(screen.IsRtl);
    }

    [Fact]
    public void Toolbar_contract_never_exposes_close_action()
    {
        Assert.DoesNotContain(
            Wave1ScreenCatalog.All.SelectMany(x => x.Actions),
            x => string.Equals(x.Action, "Close", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Numbering_uses_governing_lifecycle_not_generic_crud()
    {
        var actions = Wave1ScreenCatalog.GetRequired("SET-011").Actions.Select(x => x.Action).ToHashSet(StringComparer.OrdinalIgnoreCase);
        Assert.Contains("Reserve", actions);
        Assert.Contains("Commit", actions);
        Assert.Contains("Cancel", actions);
        Assert.Contains("Override", actions);
        Assert.DoesNotContain("New", actions);
        Assert.DoesNotContain("Save", actions);
        Assert.DoesNotContain("Activate/Disable", actions);
    }

    [Theory]
    [InlineData("FIN-028")]
    [InlineData("FIN-029")]
    [InlineData("FIN-042")]
    [InlineData("FIN-043")]
    [InlineData("FIN-055")]
    public void Report_screens_are_read_only_inquiry_surfaces(string screenId)
    {
        var actions = Wave1ScreenCatalog.GetRequired(screenId).Actions.Select(x => x.Action).ToHashSet(StringComparer.OrdinalIgnoreCase);
        Assert.Equal(5, actions.Count);
        Assert.Contains("ApplyFilters", actions);
        Assert.Contains("Refresh", actions);
        Assert.Contains("DrillDown", actions);
        Assert.Contains("Export", actions);
        Assert.Contains("Print", actions);
        Assert.DoesNotContain("New", actions);
        Assert.DoesNotContain("Edit", actions);
    }

    [Fact]
    public void All_non_cross_cutting_bindings_use_api_v1_routes_and_permissions()
    {
        foreach (var binding in Wave1ScreenCatalog.All.SelectMany(x => x.Actions).Where(x => !x.CrossCutting))
        {
            Assert.StartsWith("/api/v1/", binding.Route, StringComparison.Ordinal);
            Assert.False(string.IsNullOrWhiteSpace(binding.Permission));
            Assert.Contains(binding.HttpMethod, new[] { "GET", "POST", "PUT", "DELETE" });
        }
    }

    [Fact]
    public void Unknown_screen_is_rejected()
        => Assert.Throws<KeyNotFoundException>(() => Wave1ScreenCatalog.GetRequired("UNKNOWN"));
}
