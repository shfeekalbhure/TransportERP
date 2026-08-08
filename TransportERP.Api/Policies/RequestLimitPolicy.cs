namespace TransportERP.Api.Policies;

/// <summary>
/// Server-side bounds for collection endpoints. Values are deliberately centralized so that
/// controllers and handlers cannot quietly choose an unbounded page or lookup response.
/// </summary>
public static class RequestLimitPolicy
{
    public const int DefaultPageSize = 100;
    public const int MaximumPageSize = 500;
    public const int MaximumLookupResults = 100;

    /// <summary>
    /// Uses the default only when a caller omitted pageSize. Explicit non-positive values are
    /// invalid; oversized values are clamped to the advertised hard maximum.
    /// </summary>
    public static int NormalizePageSize(int? requestedPageSize)
    {
        if (requestedPageSize is null)
        {
            return DefaultPageSize;
        }

        if (requestedPageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(requestedPageSize),
                requestedPageSize,
                "pageSize must be greater than zero.");
        }

        return Math.Min(requestedPageSize.Value, MaximumPageSize);
    }

    /// <summary>
    /// Applies the lookup cap before a result is serialized. The caller may use
    /// <see cref="MaximumLookupResults"/> as the response metadata limit.
    /// </summary>
    public static IReadOnlyList<T> LimitLookup<T>(IEnumerable<T> results)
    {
        ArgumentNullException.ThrowIfNull(results);

        return results.Take(MaximumLookupResults).ToArray();
    }
}
