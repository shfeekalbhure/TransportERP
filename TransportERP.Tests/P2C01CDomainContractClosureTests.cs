using TransportERP.Domain.Waybills;

namespace TransportERP.Tests;

public sealed class P2C01CDomainContractClosureTests
{
    [Fact]
    public void Finalize_rejects_active_allocation_missing_from_manifest()
    {
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();
        var item = Guid.NewGuid();

        var ex = Assert.Throws<ShippingExecutionRuleException>(() =>
            ShippingExecutionRules.EnsureManifestAllocationCoverage(
                [(first, item, 4m)],
                [(first, item, 4m), (second, item, 6m)]));

        Assert.Equal("MANIFEST_LINE_INVALID", ex.Code);
    }

    [Fact]
    public void Finalize_rejects_manifest_line_without_active_allocation()
    {
        var allocation = Guid.NewGuid();
        var item = Guid.NewGuid();

        var ex = Assert.Throws<ShippingExecutionRuleException>(() =>
            ShippingExecutionRules.EnsureManifestAllocationCoverage(
                [(allocation, item, 5m)],
                []));

        Assert.Equal("MANIFEST_LINE_INVALID", ex.Code);
    }

    [Fact]
    public void Finalize_accepts_exact_active_allocation_coverage()
    {
        var first = Guid.NewGuid();
        var second = Guid.NewGuid();
        var firstItem = Guid.NewGuid();
        var secondItem = Guid.NewGuid();

        ShippingExecutionRules.EnsureManifestAllocationCoverage(
            [(first, firstItem, 4m), (second, secondItem, 6m)],
            [(second, secondItem, 6m), (first, firstItem, 4m)]);
    }

    [Fact]
    public void Explicit_waybill_volume_is_authoritative_over_dimension_fallback()
    {
        var first = ShippingExecutionRules.AllocatePhysicalMeasures(
            10m, 4m, 100m, 2m, 3m, 4m, lineVolume: 30m);
        var second = ShippingExecutionRules.AllocatePhysicalMeasures(
            10m, 6m, 100m, 2m, 3m, 4m, lineVolume: 30m);

        Assert.Equal(40m, first.AllocatedWeight);
        Assert.Equal(12m, first.AllocatedVolume);
        Assert.Equal(60m, second.AllocatedWeight);
        Assert.Equal(18m, second.AllocatedVolume);
        Assert.Equal(30m, first.AllocatedVolume + second.AllocatedVolume);
    }
}
