using TransportERP.Contracts.Wave1;

namespace TransportERP.Tests;

public sealed class Wave1ScreenCatalogTests
{
    [Fact]
    public void Catalog_contains_exactly_thirteen_unique_current_approved_wave1_screens()
    {
        Assert.Equal(13, Wave1ScreenCatalog.All.Count);
        Assert.Equal(13, Wave1ScreenCatalog.All.Select(x => x.ScreenId).Distinct(StringComparer.OrdinalIgnoreCase).Count());
        Assert.Equal(
            Wave1ScreenCatalog.All.Select(x => x.ScreenId).OrderBy(x => x, StringComparer.Ordinal).ToArray(),
            Wave1VisualCatalog.All.Select(x => x.ScreenId).OrderBy(x => x, StringComparer.Ordinal).ToArray());
    }

    [Theory]
    [InlineData("GEN-003", "MasterData", "Standard")]
    [InlineData("GEN-004", "MasterData", "Standard")]
    [InlineData("GEN-005", "MasterData", "Standard")]
    [InlineData("GEN-006", "MasterData", "Standard")]
    [InlineData("GEN-007", "MasterData", "Standard")]
    [InlineData("GEN-013", "Settings", "NumberingControlled")]
    [InlineData("GEN-014", "MasterData", "Standard")]
    [InlineData("ACC-036", "MasterData", "Standard")]
    [InlineData("ACC-074", "ReportInquiry", "Aging")]
    [InlineData("ACC-075", "ReportInquiry", "Aging")]
    [InlineData("ACC-049", "ReportInquiry", "Report")]
    [InlineData("ACC-050", "ReportInquiry", "Report")]
    [InlineData("ACC-058", "ReportInquiry", "Report")]
    public void Profile_and_variant_are_governing(string screenId, string profile, string variant)
    {
        var screen = Wave1ScreenCatalog.GetRequired(screenId);
        Assert.Equal(profile, screen.Profile);
        Assert.Equal(variant, screen.Variant);
        Assert.True(screen.IsRtl);
    }

    [Theory]
    [InlineData("ACC-074", "ACC074")]
    [InlineData("ACC-075", "ACC075")]
    [InlineData("ACC-049", "ACC049")]
    [InlineData("ACC-050", "ACC050")]
    [InlineData("ACC-058", "ACC058")]
    public void Report_permissions_match_exact_current_w2_binding(string screenId, string permissionPrefix)
    {
        var screen = Wave1ScreenCatalog.GetRequired(screenId);
        Assert.Equal(4, screen.Actions.Count);
        Assert.All(screen.Actions, x => Assert.StartsWith(permissionPrefix + ".", x.Permission, StringComparison.Ordinal));
        Assert.Contains(screen.Actions, x => x.Action == "Query" && x.Permission == permissionPrefix + ".View");
        Assert.Contains(screen.Actions, x => x.Action == "DrillDown" && x.Permission == permissionPrefix + ".DrillDown");
        Assert.Contains(screen.Actions, x => x.Action == "Export" && x.Permission == permissionPrefix + ".Export");
        Assert.Contains(screen.Actions, x => x.Action == "Print" && x.Permission == permissionPrefix + ".Print");
    }

    [Theory]
    [InlineData("GEN-003", 6)]
    [InlineData("GEN-004", 5)]
    [InlineData("GEN-005", 5)]
    [InlineData("GEN-006", 5)]
    [InlineData("GEN-007", 5)]
    [InlineData("GEN-014", 5)]
    [InlineData("ACC-036", 5)]
    public void Master_screen_action_count_matches_exact_current_w2(string screenId, int expected)
        => Assert.Equal(expected, Wave1ScreenCatalog.GetRequired(screenId).Actions.Count);

    [Fact]
    public void Every_screen_has_one_binding_per_http_method_and_route()
    {
        foreach (var screen in Wave1ScreenCatalog.All)
        {
            var keys = screen.Actions
                .Select(x => $"{x.HttpMethod.ToUpperInvariant()} {x.Route}")
                .ToArray();
            Assert.Equal(keys.Length, keys.Distinct(StringComparer.OrdinalIgnoreCase).Count());
        }
    }

    [Fact]
    public void Catalog_does_not_expose_ui_aliases_as_w2_actions()
    {
        var aliases = new[] { "New", "Save", "Search", "Refresh", "ApplyFilters", "Activate/Disable" };
        foreach (var action in Wave1ScreenCatalog.All.SelectMany(x => x.Actions))
            Assert.DoesNotContain(action.Action, aliases, StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void Legacy_catalog_targets_are_mapping_only_and_never_authoritative_screens()
    {
        var authoritative = Wave1ScreenCatalog.All.Select(x => x.ScreenId).ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (var alias in Wave1ScreenCatalog.LegacyCatalogMappings.Keys)
            Assert.DoesNotContain(alias, authoritative);

        Assert.Equal(new[] { "GEN-003", "GEN-004" }, Wave1ScreenCatalog.ResolveLegacyCatalogTarget("SET-001"));
        Assert.Equal(new[] { "GEN-005", "GEN-006", "GEN-007" }, Wave1ScreenCatalog.ResolveLegacyCatalogTarget("SET-002"));
        Assert.Equal(new[] { "GEN-013" }, Wave1ScreenCatalog.ResolveLegacyCatalogTarget("SET-011"));
        Assert.Equal(new[] { "ACC-058" }, Wave1ScreenCatalog.ResolveLegacyCatalogTarget("FIN-055"));
    }

    [Fact]
    public void Geography_current_identities_do_not_share_cross_screen_permissions()
    {
        Assert.All(Wave1ScreenCatalog.GetRequired("GEN-003").Actions, x => Assert.StartsWith("GEN003.", x.Permission));
        Assert.All(Wave1ScreenCatalog.GetRequired("GEN-004").Actions, x => Assert.StartsWith("GEN004.", x.Permission));
        Assert.All(Wave1ScreenCatalog.GetRequired("GEN-005").Actions, x => Assert.StartsWith("GEN005.", x.Permission));
        Assert.All(Wave1ScreenCatalog.GetRequired("GEN-006").Actions, x => Assert.StartsWith("GEN006.", x.Permission));
        Assert.All(Wave1ScreenCatalog.GetRequired("GEN-007").Actions, x => Assert.StartsWith("GEN007.", x.Permission));
    }

    [Fact]
    public void Numbering_uses_governing_lifecycle_not_generic_crud()
    {
        var actions = Wave1ScreenCatalog.GetRequired("GEN-013").Actions.Select(x => x.Action).ToHashSet(StringComparer.OrdinalIgnoreCase);
        Assert.Contains("Reserve", actions);
        Assert.Contains("Commit", actions);
        Assert.Contains("Cancel", actions);
        Assert.Contains("Override", actions);
        Assert.DoesNotContain("Create", actions);
        Assert.DoesNotContain("Disable", actions);
    }

    [Theory]
    [InlineData("ACC-074")]
    [InlineData("ACC-075")]
    [InlineData("ACC-049")]
    [InlineData("ACC-050")]
    [InlineData("ACC-058")]
    public void Report_screens_are_read_only_inquiry_surfaces(string screenId)
    {
        var actions = Wave1ScreenCatalog.GetRequired(screenId).Actions.Select(x => x.Action).ToHashSet(StringComparer.OrdinalIgnoreCase);
        Assert.Equal(4, actions.Count);
        Assert.Contains("Query", actions);
        Assert.Contains("DrillDown", actions);
        Assert.Contains("Export", actions);
        Assert.Contains("Print", actions);
        Assert.DoesNotContain("Create", actions);
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
    public void Unknown_or_legacy_screen_is_rejected_by_authoritative_lookup()
    {
        Assert.Throws<KeyNotFoundException>(() => Wave1ScreenCatalog.GetRequired("UNKNOWN"));
        Assert.Throws<KeyNotFoundException>(() => Wave1ScreenCatalog.GetRequired("SET-001"));
        Assert.Throws<KeyNotFoundException>(() => Wave1VisualCatalog.GetRequired("FIN-055"));
    }
}
