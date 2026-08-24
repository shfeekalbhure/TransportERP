using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Wave1RemainingImplementationTests
{
    [Fact]
    public async Task Language_master_matches_current_GEN014_contract_and_is_concurrency_safe_and_audited()
    {
        await using var db = CreateReferenceDb();
        var service = new Wave1LanguageService(db);
        var ctx = Context();

        var language = await service.CreateLanguageAsync(ctx, new CreateLanguageRequest("ar", "ar-YE", "rtl"));
        Assert.Equal("ar", language.Code);
        Assert.Equal("ar-YE", language.CultureCode);
        Assert.Equal("RTL", language.Direction);
        Assert.Equal("Active", language.Status);
        Assert.Equal(1, language.Version);

        var byId = await service.GetLanguageAsync(language.Id);
        Assert.NotNull(byId);
        Assert.Equal(language, byId);

        var list = await service.ListLanguagesAsync(
            new LanguageQueryRequest(SearchText: "ar", Direction: "RTL", Page: 1, PageSize: 500));
        Assert.Single(list.Items);
        Assert.Equal(200, list.PageSize);

        await Assert.ThrowsAsync<Wave1ReferenceRuleException>(() =>
            service.CreateLanguageAsync(ctx, new CreateLanguageRequest("ar2", "ar-YE", "RTL")));
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
            service.UpdateLanguageAsync(ctx, language.Id, new UpdateLanguageRequest("ar", "ar-SA", "RTL", 99)));

        var updated = await service.UpdateLanguageAsync(
            ctx, language.Id, new UpdateLanguageRequest("ar", "ar-SA", "RTL", language.Version));
        Assert.NotNull(updated);
        Assert.Equal(2, updated!.Version);
        Assert.Equal("ar-SA", updated.CultureCode);

        var disabled = await service.DisableLanguageAsync(
            ctx, language.Id, new DisableRequest(updated.Version, "retired locale"));
        Assert.NotNull(disabled);
        Assert.Equal("Stopped", disabled!.Status);
        Assert.Equal(3, disabled.Version);

        Assert.Equal(3, await db.AuditEvents.CountAsync());
        Assert.All(await db.AuditEvents.OrderBy(x => x.OccurredAt).ToListAsync(),
            x => Assert.False(string.IsNullOrWhiteSpace(x.Hash)));
    }

    private static OperationContext Context()
        => new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

    private static Wave1ReferenceDbContext CreateReferenceDb()
    {
        var options = new DbContextOptionsBuilder<Wave1ReferenceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .ReplaceService<Microsoft.EntityFrameworkCore.Infrastructure.IModelCustomizer, Wave1ReferenceRuntimeModelCustomizer>()
            .Options;
        return new Wave1ReferenceDbContext(options);
    }
}
