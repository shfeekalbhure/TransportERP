using TransportERP.Contracts.Wave1;

namespace TransportERP.Tests;

public sealed class Wave1AccountingRuntimeContractTests
{
    [Theory]
    [InlineData("ACC-049", "ACC049", "/api/v1/accounting/reports/balance-sheet")]
    [InlineData("ACC-058", "ACC058", "/api/v1/accounting/reports/detailed-trial-balance")]
    public void Implemented_accounting_reports_keep_exact_W2_routes_and_permissions(string screenId, string permissionPrefix, string routeBase)
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
    public void Held_accounting_reports_remain_merge_blockers()
    {
        Assert.Equal(Wave1ReadinessState.Hold, Wave1ReadinessCatalog.GetRequired("ACC-074").State);
        Assert.Equal(Wave1ReadinessState.Hold, Wave1ReadinessCatalog.GetRequired("ACC-075").State);
        Assert.Equal(Wave1ReadinessState.Hold, Wave1ReadinessCatalog.GetRequired("ACC-050").State);
        Assert.True(Wave1ReadinessCatalog.HasMergeBlockers);
    }

    private static void AssertBinding(Wave1ActionBinding binding, string action, string permission, string method, string route)
    {
        Assert.Equal(action, binding.Action);
        Assert.Equal(permission, binding.Permission);
        Assert.Equal(method, binding.HttpMethod);
        Assert.Equal(route, binding.Route);
    }
}
