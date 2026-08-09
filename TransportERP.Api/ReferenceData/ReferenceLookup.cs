namespace TransportERP.Api.ReferenceData;

public sealed record LookupItem(string Id, string Name, string Company, string Branch);
public sealed record LookupAccessContext(string Company, string Branch, bool CanReadLookups);

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
        .Select(number => new LookupItem(
            number.ToString(),
            $"Reference {number}",
            number <= 60 ? "north" : "south",
            number <= 30 || (number > 60 && number <= 90) ? "north-1" : "north-2"))
        .ToArray();

    public IReadOnlyList<LookupItem> Search(string query, LookupAccessContext access)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(query);
        if (!access.CanReadLookups)
        {
            throw new UnauthorizedAccessException("The lookup.read permission is required.");
        }

        return Items.Where(item => item.Company == access.Company)
            .Where(item => item.Branch == access.Branch)
            .Where(item => item.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
            .Take(Policies.RequestLimitPolicy.MaximumLookupResults)
            .ToArray();
    }
}
