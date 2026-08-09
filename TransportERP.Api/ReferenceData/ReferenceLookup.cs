namespace TransportERP.Api.ReferenceData;

public sealed record LookupItem(string Id, string Name, string Scope);
public sealed record LookupAccessContext(string Scope, bool CanReadLookups);

public interface IReferenceLookupProvider
{
    IReadOnlyList<LookupItem> Search(string query, LookupAccessContext access);
}

/// <summary>
/// Reference provider requires a query, enforces authorization and applies tenant scope before
/// materialisation. This intentionally has no unfiltered/full-table lookup operation.
/// </summary>
public sealed class InMemoryReferenceLookupProvider : IReferenceLookupProvider
{
    private static readonly IReadOnlyList<LookupItem> Items = Enumerable.Range(1, 120)
        .Select(number => new LookupItem(number.ToString(), $"Reference {number}", number <= 60 ? "north" : "south"))
        .ToArray();

    public IReadOnlyList<LookupItem> Search(string query, LookupAccessContext access)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);
        if (!access.CanReadLookups)
        {
            throw new UnauthorizedAccessException("The lookup.read permission is required.");
        }

        return Items.Where(item => item.Scope == access.Scope)
            .Where(item => item.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Take(Policies.RequestLimitPolicy.MaximumLookupResults)
            .ToArray();
    }
}
