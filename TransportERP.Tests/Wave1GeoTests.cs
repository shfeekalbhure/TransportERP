using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Geo;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Wave1GeoTests
{
    [Fact]
    public async Task Geography_crud_concurrency_disable_and_audit_are_enforced()
    {
        await using var db = CreateDb();
        var service = new Wave1GeoService(db);
        var context = Context();

        var country = Assert.IsType<CountryDto>(await service.CreateAsync(
            Wave1GeoResource.Countries,
            new CreateCountryRequest("ye", "اليمن", "Yemen", "يمني"),
            context));
        Assert.Equal("YE", country.Code);
        Assert.Equal(1, country.Version);

        var governorate = Assert.IsType<GovernorateDto>(await service.CreateAsync(
            Wave1GeoResource.Governorates,
            new CreateGovernorateRequest(country.Id, "ADN", "عدن", "Aden"),
            context));
        Assert.Equal(country.Id, governorate.CountryId);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(
            Wave1GeoResource.Governorates,
            new CreateGovernorateRequest(country.Id, "adn", "عدن مكرر", null),
            context));

        var updated = Assert.IsType<GovernorateDto>(await service.UpdateAsync(
            Wave1GeoResource.Governorates,
            governorate.Id,
            new UpdateGovernorateRequest(country.Id, "ADN", "محافظة عدن", "Aden", governorate.Version),
            context));
        Assert.Equal(2, updated.Version);
        Assert.Equal("محافظة عدن", updated.ArabicName);

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => service.UpdateAsync(
            Wave1GeoResource.Governorates,
            governorate.Id,
            new UpdateGovernorateRequest(country.Id, "ADN", "قديم", null, governorate.Version),
            context));

        var disabled = Assert.IsType<GovernorateDto>(await service.DisableAsync(
            Wave1GeoResource.Governorates,
            governorate.Id,
            new DisableRequest(updated.Version, "إيقاف للاختبار"),
            context));
        Assert.False(disabled.IsActive);
        Assert.Equal(3, disabled.Version);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(
            Wave1GeoResource.Directorates,
            new CreateDirectorateRequest(governorate.Id, "SHK", "الشيخ عثمان", null),
            context));

        var audit = await db.AuditEvents
            .OrderBy(x => x.OccurredAt)
            .ThenBy(x => x.Id)
            .ToListAsync();
        Assert.Equal(4, audit.Count);
        Assert.Equal("Geo.Create", audit[0].Action);
        Assert.Equal("Geo.Create", audit[1].Action);
        Assert.Equal("Geo.Update", audit[2].Action);
        Assert.Equal("Geo.Disable", audit[3].Action);
        Assert.Null(audit[0].PreviousHash);
        Assert.Equal(audit[0].Hash, audit[1].PreviousHash);
        Assert.Equal(audit[1].Hash, audit[2].PreviousHash);
        Assert.Equal(audit[2].Hash, audit[3].PreviousHash);
        Assert.All(audit, x => Assert.Equal(AuditEventService.ComputeHash(x), x.Hash));
        Assert.Equal("إيقاف للاختبار", audit[3].Reason);
    }

    [Fact]
    public async Task Geography_list_filters_by_parent_status_and_search()
    {
        await using var db = CreateDb();
        var service = new Wave1GeoService(db);
        var context = Context();
        var yemen = Assert.IsType<CountryDto>(await service.CreateAsync(
            Wave1GeoResource.Countries,
            new CreateCountryRequest("YE", "اليمن", "Yemen", null), context));
        var saudi = Assert.IsType<CountryDto>(await service.CreateAsync(
            Wave1GeoResource.Countries,
            new CreateCountryRequest("SA", "السعودية", "Saudi Arabia", null), context));
        await service.CreateAsync(Wave1GeoResource.Governorates,
            new CreateGovernorateRequest(yemen.Id, "ADN", "عدن", "Aden"), context);
        await service.CreateAsync(Wave1GeoResource.Governorates,
            new CreateGovernorateRequest(saudi.Id, "RYD", "الرياض", "Riyadh"), context);

        var result = await service.ListAsync(
            Wave1GeoResource.Governorates,
            new PagedQueryRequest(Page: 1, PageSize: 20, SearchText: "عدن", ParentId: yemen.Id, IsActive: true));

        var row = Assert.Single(result.Items);
        Assert.Equal("ADN", row.Code);
        Assert.Equal(1, result.TotalCount);
    }

    private static Wave1GeoDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<Wave1GeoDbContext>()
            .UseInMemoryDatabase($"wave1-geo-{Guid.NewGuid():N}")
            .Options;
        return new Wave1GeoDbContext(options);
    }

    private static Wave1GeoOperationContext Context()
        => new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "device-test", "127.0.0.1");
}
