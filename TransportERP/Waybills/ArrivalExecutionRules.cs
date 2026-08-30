namespace TransportERP.Domain.Waybills;

public static class ArrivalExecutionStatuses
{
    public static class Receipt
    {
        public const string Draft = "DRAFT";
        public const string Finalized = "FINALIZED";
    }

    public static class Difference
    {
        public const string Unvalidated = "UNVALIDATED";
        public const string None = "NONE";
        public const string Short = "SHORT";
        public const string Damage = "DAMAGE";
        public const string ShortAndDamage = "SHORT_AND_DAMAGE";
    }

    public static class Holding
    {
        public const string Transit = "TRANSIT";
        public const string Destination = "DESTINATION";
        public const string Available = "AVAILABLE";
        public const string Reserved = "RESERVED";
        public const string Released = "RELEASED";
        public const string Exception = "EXCEPTION";
    }
}

public static class ArrivalExecutionRules
{
    private const decimal Tolerance = 0.0001m;

    public static void EnsureRecordArrival(string tripStatus, Guid destinationId, Guid locationId, bool validStop)
    {
        if (!string.Equals(tripStatus, "DEPARTED", StringComparison.Ordinal))
            throw new ArrivalExecutionRuleException("INVALID_STATE");
        if (locationId == Guid.Empty || (locationId != destinationId && !validStop))
            throw new ArrivalExecutionRuleException("LOCATION_INVALID");
    }

    public static void EnsureUnload(decimal expectedRemaining, decimal actualQuantity, decimal damageQuantity)
    {
        if (actualQuantity < 0m || damageQuantity < 0m || damageQuantity - actualQuantity > Tolerance)
            throw new ArrivalExecutionRuleException("VALIDATION_ERROR");
        if (actualQuantity - expectedRemaining > Tolerance)
            throw new ArrivalExecutionRuleException("QUANTITY_EXCEEDS_IN_TRANSIT");
    }

    public static string DifferenceType(decimal expectedQuantity, decimal actualQuantity, decimal damageQuantity, string? requested)
    {
        var shortQty = expectedQuantity - actualQuantity > Tolerance;
        var damaged = damageQuantity > Tolerance;
        var derived = shortQty && damaged ? ArrivalExecutionStatuses.Difference.ShortAndDamage
            : damaged ? ArrivalExecutionStatuses.Difference.Damage
            : shortQty ? ArrivalExecutionStatuses.Difference.Short
            : ArrivalExecutionStatuses.Difference.None;

        if (!string.IsNullOrWhiteSpace(requested) &&
            !string.Equals(requested.Trim(), derived, StringComparison.OrdinalIgnoreCase))
            throw new ArrivalExecutionRuleException("VALIDATION_ERROR");
        return derived;
    }

    public static void EnsureDifferenceEvidence(string differenceType, Guid? evidenceAttachmentId)
    {
        if (differenceType is ArrivalExecutionStatuses.Difference.Short or
            ArrivalExecutionStatuses.Difference.Damage or
            ArrivalExecutionStatuses.Difference.ShortAndDamage)
        {
            if (!evidenceAttachmentId.HasValue || evidenceAttachmentId == Guid.Empty)
                throw new ArrivalExecutionRuleException("DIFFERENCE_REQUIRES_EVIDENCE");
        }
    }

    public static void EnsureFinalize(string status, IEnumerable<(string DifferenceType, decimal Expected, decimal Actual)> lines)
    {
        if (!string.Equals(status, ArrivalExecutionStatuses.Receipt.Draft, StringComparison.Ordinal))
            throw new ArrivalExecutionRuleException("INVALID_STATE");
        var materialized = lines.ToList();
        if (materialized.Count == 0 || materialized.Any(x =>
                string.Equals(x.DifferenceType, ArrivalExecutionStatuses.Difference.Unvalidated, StringComparison.Ordinal)))
            throw new ArrivalExecutionRuleException("UNVALIDATED_LINES");
        if (materialized.Any(x => x.Actual < 0m || x.Actual - x.Expected > Tolerance))
            throw new ArrivalExecutionRuleException("UNVALIDATED_LINES");
    }

    public static void EnsureReallocate(string holdingStatus, string holdingType, decimal available, decimal requested)
    {
        if (!string.Equals(holdingStatus, ArrivalExecutionStatuses.Holding.Available, StringComparison.Ordinal) ||
            !string.Equals(holdingType, ArrivalExecutionStatuses.Holding.Transit, StringComparison.Ordinal))
            throw new ArrivalExecutionRuleException("INVALID_STATE");
        if (requested <= 0m)
            throw new ArrivalExecutionRuleException("VALIDATION_ERROR");
        if (requested - available > Tolerance)
            throw new ArrivalExecutionRuleException("QUANTITY_EXCEEDS_AVAILABLE");
    }

    public static void EnsureRouteCompatible(Guid holdingLocationId, Guid tripOriginId, IEnumerable<Guid> tripStops)
    {
        if (tripOriginId != holdingLocationId && !tripStops.Contains(holdingLocationId))
            throw new ArrivalExecutionRuleException("ROUTE_INCOMPATIBLE");
    }

    public static void EnsureTripClose(string tripStatus, decimal departedQuantity, decimal accountedQuantity, bool custodyOpen, bool exceptionBlocked)
    {
        if (!string.Equals(tripStatus, "ARRIVED", StringComparison.Ordinal))
            throw new ArrivalExecutionRuleException("INVALID_STATE");
        if (departedQuantity - accountedQuantity > Tolerance)
            throw new ArrivalExecutionRuleException("CARGO_UNACCOUNTED");
        if (custodyOpen)
            throw new ArrivalExecutionRuleException("CUSTODY_OPEN");
        if (exceptionBlocked)
            throw new ArrivalExecutionRuleException("EXCEPTION_BLOCKED");
    }
}

public sealed class ArrivalExecutionRuleException(string code) : InvalidOperationException(code)
{
    public string Code { get; } = code;
}
