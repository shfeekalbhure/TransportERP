using TransportERP.Domain.Waybills;

namespace TransportERP.Tests;

public sealed class P2C01CShippingExecutionTests
{
    [Fact]
    public void Release_rejects_quantity_over_original_remaining()
    {
        var ex = Assert.Throws<ShippingExecutionRuleException>(() =>
            ShippingExecutionRules.EnsureRelease(10m, 7m, 4m));
        Assert.Equal("QUANTITY_EXCEEDS_REMAINING", ex.Code);
    }

    [Fact]
    public void Allocation_rejects_quantity_over_released_remaining()
    {
        var ex = Assert.Throws<ShippingExecutionRuleException>(() =>
            ShippingExecutionRules.EnsureAllocation(8m, 5m, 4m));
        Assert.Equal("QUANTITY_EXCEEDS_RELEASED", ex.Code);
    }

    [Fact]
    public void Load_rejects_quantity_over_manifest_line_remaining()
    {
        var ex = Assert.Throws<ShippingExecutionRuleException>(() =>
            ShippingExecutionRules.EnsureLoad(6m, 4m, 3m));
        Assert.Equal("QUANTITY_EXCEEDS_ALLOCATION", ex.Code);
    }

    [Fact]
    public void Route_requires_origin_before_destination()
    {
        var origin = Guid.NewGuid();
        var destination = Guid.NewGuid();
        var stop = Guid.NewGuid();
        ShippingExecutionRules.EnsureRouteCompatible(origin, destination, origin, destination, [stop]);

        var ex = Assert.Throws<ShippingExecutionRuleException>(() =>
            ShippingExecutionRules.EnsureRouteCompatible(destination, origin, origin, destination, [stop]));
        Assert.Equal("ROUTE_INCOMPATIBLE", ex.Code);
    }

    [Fact]
    public void Trip_input_rejects_duplicate_stop_sequence()
    {
        var ex = Assert.Throws<ShippingExecutionRuleException>(() =>
            ShippingExecutionRules.EnsureTripInput(
                "TRIP-1", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                DateTimeOffset.UtcNow.AddHours(1),
                [(1, Guid.NewGuid()), (1, Guid.NewGuid())]));
        Assert.Equal("VALIDATION_ERROR", ex.Code);
    }

    [Fact]
    public void Manifest_finalize_requires_every_line_fully_loaded()
    {
        ShippingExecutionRules.EnsureManifestCanFinalize([(5m, 5m), (2m, 2m)]);

        var ex = Assert.Throws<ShippingExecutionRuleException>(() =>
            ShippingExecutionRules.EnsureManifestCanFinalize([(5m, 4m)]));
        Assert.Equal("MANIFEST_LINE_INVALID", ex.Code);
    }

    [Theory]
    [InlineData("[\"HAZARDOUS\"]")]
    [InlineData("[\"COLD\"]")]
    public void Resource_risk_requires_confirmation(string flags)
    {
        var ex = Assert.Throws<ShippingExecutionRuleException>(() =>
            ShippingExecutionRules.EnsureResourceConstraint(flags, false));
        Assert.Equal("RESOURCE_CONSTRAINT", ex.Code);
        ShippingExecutionRules.EnsureResourceConstraint(flags, true);
    }
}
