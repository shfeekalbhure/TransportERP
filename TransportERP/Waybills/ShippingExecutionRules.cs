namespace TransportERP.Domain.Waybills;

public static class ShippingExecutionStatuses
{
    public static class Trip
    {
        public const string Draft = "DRAFT";
        public const string Ready = "READY";
        public const string Departed = "DEPARTED";
        public const string Arrived = "ARRIVED";
        public const string Closed = "CLOSED";
        public const string Cancelled = "CANCELLED";
    }

    public static class Manifest
    {
        public const string Draft = "DRAFT";
        public const string Finalized = "FINALIZED";
        public const string HandedOver = "HANDED_OVER";
        public const string Accepted = "ACCEPTED";
        public const string Closed = "CLOSED";
    }

    public static class Allocation
    {
        public const string Allocated = "ALLOCATED";
        public const string Reversed = "REVERSED";
    }

    public static class Release
    {
        public const string Active = "ACTIVE";
        public const string Reversed = "REVERSED";
    }

    public static class Load
    {
        public const string Planned = "PLANNED";
        public const string Partial = "PARTIAL";
        public const string Loaded = "LOADED";
    }
}

public static class ShippingExecutionRules
{
    private const decimal Tolerance = 0.0001m;

    public static void EnsureRelease(decimal originalQuantity, decimal releasedNet, decimal requested)
    {
        EnsurePositive(originalQuantity, "ITEM_QUANTITY_INVALID");
        EnsurePositive(requested, "QUANTITY_INVALID");
        if (releasedNet < -Tolerance || releasedNet - originalQuantity > Tolerance)
            throw new ShippingExecutionRuleException("BALANCE_INCONSISTENT");
        if (releasedNet + requested - originalQuantity > Tolerance)
            throw new ShippingExecutionRuleException("QUANTITY_EXCEEDS_REMAINING");
    }

    public static void EnsureAllocation(decimal releaseQuantity, decimal allocatedNet, decimal requested)
    {
        EnsurePositive(releaseQuantity, "RELEASE_QUANTITY_INVALID");
        EnsurePositive(requested, "QUANTITY_INVALID");
        if (allocatedNet < -Tolerance || allocatedNet - releaseQuantity > Tolerance)
            throw new ShippingExecutionRuleException("BALANCE_INCONSISTENT");
        if (allocatedNet + requested - releaseQuantity > Tolerance)
            throw new ShippingExecutionRuleException("QUANTITY_EXCEEDS_RELEASED");
    }

    public static void EnsureLoad(decimal plannedQuantity, decimal loadedNet, decimal requested)
    {
        EnsurePositive(plannedQuantity, "MANIFEST_LINE_INVALID");
        EnsurePositive(requested, "QUANTITY_INVALID");
        if (loadedNet < -Tolerance || loadedNet - plannedQuantity > Tolerance)
            throw new ShippingExecutionRuleException("BALANCE_INCONSISTENT");
        if (loadedNet + requested - plannedQuantity > Tolerance)
            throw new ShippingExecutionRuleException("QUANTITY_EXCEEDS_ALLOCATION");
    }

    /// <summary>
    /// W1 WaybillItem physical measures are line-level snapshots. When one line is split across trips,
    /// each allocation receives the same quantity ratio of the line's total weight and dimensional volume.
    /// Values are rounded to the persistence precision used by ManifestLine (4 decimal places).
    /// </summary>
    public static (decimal AllocatedWeight, decimal AllocatedVolume) AllocatePhysicalMeasures(
        decimal itemQuantity,
        decimal allocatedQuantity,
        decimal? lineWeight,
        decimal? length,
        decimal? width,
        decimal? height)
    {
        EnsurePositive(itemQuantity, "ITEM_QUANTITY_INVALID");
        EnsurePositive(allocatedQuantity, "QUANTITY_INVALID");
        if (allocatedQuantity - itemQuantity > Tolerance)
            throw new ShippingExecutionRuleException("QUANTITY_EXCEEDS_ITEM");

        var weight = lineWeight ?? 0m;
        var l = length ?? 0m;
        var w = width ?? 0m;
        var h = height ?? 0m;
        if (weight < 0m || l < 0m || w < 0m || h < 0m)
            throw new ShippingExecutionRuleException("PHYSICAL_MEASURE_INVALID");

        var ratio = allocatedQuantity / itemQuantity;
        var totalVolume = l == 0m || w == 0m || h == 0m ? 0m : l * w * h;
        return (
            decimal.Round(weight * ratio, 4, MidpointRounding.AwayFromZero),
            decimal.Round(totalVolume * ratio, 4, MidpointRounding.AwayFromZero));
    }

    public static void EnsureRouteCompatible(
        Guid waybillOriginId,
        Guid waybillDestinationId,
        Guid tripOriginId,
        Guid tripDestinationId,
        IReadOnlyList<Guid> orderedStops)
    {
        if (waybillOriginId == Guid.Empty || waybillDestinationId == Guid.Empty ||
            tripOriginId == Guid.Empty || tripDestinationId == Guid.Empty)
            throw new ShippingExecutionRuleException("ROUTE_INCOMPATIBLE");

        var route = new List<Guid>(orderedStops.Count + 2) { tripOriginId };
        route.AddRange(orderedStops);
        route.Add(tripDestinationId);

        var from = route.IndexOf(waybillOriginId);
        var to = route.LastIndexOf(waybillDestinationId);
        if (from < 0 || to < 0 || from >= to)
            throw new ShippingExecutionRuleException("ROUTE_INCOMPATIBLE");
    }

    public static void EnsureTripInput(
        string tripNo,
        Guid vehicleId,
        Guid driverId,
        Guid originId,
        Guid destinationId,
        DateTimeOffset plannedDepartAt,
        IReadOnlyList<(int StopNo, Guid LocationId)> stops)
    {
        if (string.IsNullOrWhiteSpace(tripNo) || tripNo.Trim().Length > 80)
            throw new ShippingExecutionRuleException("VALIDATION_ERROR");
        if (vehicleId == Guid.Empty || driverId == Guid.Empty || originId == Guid.Empty || destinationId == Guid.Empty)
            throw new ShippingExecutionRuleException("VALIDATION_ERROR");
        if (originId == destinationId || plannedDepartAt == default)
            throw new ShippingExecutionRuleException("VALIDATION_ERROR");
        if (stops.Any(x => x.StopNo < 1 || x.LocationId == Guid.Empty) ||
            stops.Select(x => x.StopNo).Distinct().Count() != stops.Count)
            throw new ShippingExecutionRuleException("VALIDATION_ERROR");
    }

    public static void EnsureManifestCanFinalize(IReadOnlyList<(decimal Planned, decimal Loaded)> lines)
    {
        if (lines.Count == 0)
            throw new ShippingExecutionRuleException("MANIFEST_LINE_INVALID");
        foreach (var line in lines)
        {
            EnsurePositive(line.Planned, "MANIFEST_LINE_INVALID");
            if (line.Loaded < -Tolerance || Math.Abs(line.Loaded - line.Planned) > Tolerance)
                throw new ShippingExecutionRuleException("MANIFEST_LINE_INVALID");
        }
    }

    public static void EnsureResourceConstraint(string? riskFlagsJson, bool confirmed)
    {
        if (confirmed || string.IsNullOrWhiteSpace(riskFlagsJson)) return;
        if (riskFlagsJson.Contains("HAZARDOUS", StringComparison.OrdinalIgnoreCase) ||
            riskFlagsJson.Contains("COLD", StringComparison.OrdinalIgnoreCase))
            throw new ShippingExecutionRuleException("RESOURCE_CONSTRAINT");
    }

    private static void EnsurePositive(decimal value, string code)
    {
        if (value <= 0m)
            throw new ShippingExecutionRuleException(code);
    }
}

public sealed class ShippingExecutionRuleException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}
