using TransportERP.Contracts.Wave1;

namespace TransportERP.Tests;

public sealed class Wave1AccountingRuntimeContractTests
{
    [Theory]
    [InlineData("ACC-049", "ACC049", "/api/v1/accounting/reports/balance-sheet")]
    [InlineData("ACC-050", "ACC050", "/api/v1/accounting/reports/cash-flow")]
    [InlineData("ACC-058", "ACC058", "/api/v1/accounting/reports/detailed-trial-balance")]
    [InlineData("ACC-074", "ACC074", "/api/v1/accounting/reports/customer-aging")]
    [InlineData("ACC-075", "ACC075", "/api/v1/accounting/reports/supplier-aging")]
    public void Accounting_reports_keep_exact_W2_routes_and_permissions(string screenId, string permissionPrefix, string routeBase)
    {
        var screen = Wave1ScreenCatalog.GetRequired(screenId);
        Assert.Equal(Wave1ReadinessState.ImplementedReviewRequired, Wave1ReadinessCatalog.GetRequired(screenId).State);
        Assert.Collection(screen.Actions,
            x => AssertBinding(x, "Query", $"{permissionPrefix}.View", "POST", $"{routeBase}/query"),
            x => AssertBinding(x, "DrillDown", $"{permissionPrefix}.DrillDown", "POST", $"{routeBase}/drill-down"),
            x => AssertBinding(x, "Export", $"{permissionPrefix}.Export", "POST", $"{routeBase}/export"),
            x => AssertBinding(x, "Print", $"{permissionPrefix}.Print", "POST", $"{routeBase}/print"));
    }

    [Fact]
    public void ACC036_remains_separate_entities_behind_the_exact_W2_route_family()
    {
        var screen = Wave1ScreenCatalog.GetRequired("ACC-036");
        Assert.Equal(Wave1ReadinessState.ImplementedReviewRequired, Wave1ReadinessCatalog.GetRequired("ACC-036").State);
        Assert.Equal(5, screen.Actions.Count);
        Assert.All(screen.Actions, x => Assert.StartsWith("ACC036.", x.Permission));
        Assert.All(screen.Actions, x => Assert.StartsWith("/api/v1/accounting/account-classifications", x.Route));
    }

    [Fact]
    public void Owner_decision_closure_removes_all_merge_blockers_but_not_independent_review_gate()
    {
        Assert.False(Wave1ReadinessCatalog.HasMergeBlockers);
        Assert.All(Wave1ReadinessCatalog.All, x => Assert.Equal("EXACT_SHA_INDEPENDENT_REVIEW", x.Gate));
    }

    private static void AssertBinding(Wave1ActionBinding binding, string action, string permission, string method, string route)
    {
        Assert.Equal(action, binding.Action); Assert.Equal(permission, binding.Permission); Assert.Equal(method, binding.HttpMethod); Assert.Equal(route, binding.Route);
    }
}
