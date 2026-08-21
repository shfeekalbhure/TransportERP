using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class P2C01CPhysicalMeasurePostgreSqlTests
{
    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Split_item_across_two_trips_preserves_total_weight_and_volume()
    {
        var connection = RequireConnection();
        await using (var migrate = CreateDb(connection))
            await migrate.Database.MigrateAsync();

        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = Guid.NewGuid().ToString("N")[..3].ToUpperInvariant(), NameAr = "عملة قياسات C",
            MinorUnit = 2, IsBase = true, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"CM-{Guid.NewGuid():N}"[..20], LegalNameAr = "شركة قياسات C",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "MAIN", NameAr = "الرئيسي",
            Timezone = "Asia/Aden", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = $"measure-{Guid.NewGuid():N}", NormalizedUserName = $"MEASURE{Guid.NewGuid():N}"[..24],
            DisplayName = "مستخدم قياسات", PasswordHash = "test-only", Status = "ACTIVE",
            CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var origin = Guid.NewGuid();
        var destination = Guid.NewGuid();
        var waybill = new WaybillEntity
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, BranchId = branch.Id, DraftNo = $"D-{Guid.NewGuid():N}",
            WaybillNo = $"WB-M-{Guid.NewGuid():N}"[..24], WaybillDateTime = now, ServiceType = "STANDARD", Priority = "NORMAL",
            OriginId = origin, DestinationId = destination, CurrencyId = currency.Id, ExchangeRate = 1m,
            FreightTotal = 100m, DiscountTotal = 0m, Status = "APPROVED", FinancialStatus = "UNPAID",
            CreateClientOperationId = $"seed-{Guid.NewGuid():N}", LastClientOperationId = $"approve-{Guid.NewGuid():N}",
            Version = 1, CreatedAt = now, UpdatedAt = now
        };
        var item = new WaybillItemEntity
        {
            Id = Guid.NewGuid(), WaybillId = waybill.Id, LineNo = 1, ItemType = "GENERAL", Contents = "Split measures",
            Quantity = 10m, Pieces = 10, Weight = 100m, Length = 2m, Width = 3m, Height = 4m,
            RiskFlagsJson = "[]"
        };

        await using (var seed = CreateDb(connection))
        {
            seed.Currencies.Add(currency); seed.Companies.Add(company); seed.Branches.Add(branch); seed.Users.Add(user);
            seed.Set<WaybillEntity>().Add(waybill); seed.Set<WaybillItemEntity>().Add(item);
            await seed.SaveChangesAsync();
        }

        var context = new OperationContext(user.Id, company.Id, branch.Id, Guid.NewGuid());
        await using (var db = CreateDb(connection))
            _ = await Service(db).ReleaseItemAsync(context, waybill.Id, item.Id,
                new ReleaseItemRequest(10m, now, $"release-{Guid.NewGuid():N}"));

        Guid releaseId;
        await using (var db = CreateDb(connection))
            releaseId = await db.Set<ItemReleaseEntity>().Where(x => x.WaybillItemId == item.Id).Select(x => x.Id).SingleAsync();

        var first = await CreateTripAllocationManifest(connection, context, item.Id, releaseId, origin, destination, 4m);
        var second = await CreateTripAllocationManifest(connection, context, item.Id, releaseId, origin, destination, 6m);

        Assert.Equal(40m, first.Lines.Single().Weight);
        Assert.Equal(9.6m, first.Lines.Single().Volume);
        Assert.Equal(60m, second.Lines.Single().Weight);
        Assert.Equal(14.4m, second.Lines.Single().Volume);
        Assert.Equal(100m, first.Lines.Single().Weight + second.Lines.Single().Weight);
        Assert.Equal(24m, first.Lines.Single().Volume + second.Lines.Single().Volume);
    }

    private static async Task<ManifestResponse> CreateTripAllocationManifest(
        string connection, OperationContext context, Guid itemId, Guid releaseId,
        Guid origin, Guid destination, decimal quantity)
    {
        TripResponse trip;
        await using (var db = CreateDb(connection))
            trip = await Service(db).CreateTripAsync(context,
                new CreateTripRequest($"TR-M-{Guid.NewGuid():N}"[..24], Guid.NewGuid(), context.UserId,
                    origin, destination, DateTimeOffset.UtcNow.AddHours(1), [], $"trip-{Guid.NewGuid():N}"));

        await using (var db = CreateDb(connection))
            _ = await Service(db).AllocateAsync(context, trip.Id,
                new AllocateItemRequest(itemId, releaseId, quantity, $"alloc-{Guid.NewGuid():N}"));

        await using (var db = CreateDb(connection))
            return await Service(db).GenerateManifestAsync(context, trip.Id,
                new GenerateManifestRequest(null, $"manifest-{Guid.NewGuid():N}"));
    }

    private static ShippingExecutionApplicationService Service(TransportErpDbContext db)
        => new(new EfShippingExecutionStore(db, new EfWaybillAuditSink(db, new AuditEventService(db))));

    private static TransportErpDbContext CreateDb(string connection)
        => new(new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseNpgsql(connection, npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "transport_erp"))
            .ReplaceService<IModelCustomizer, TransportErpP2CombinedModelCustomizer>()
            .AddInterceptors(new P2FinanceAppendOnlyInterceptor(), new P2ShippingAppendOnlyInterceptor())
            .Options);

    private static string RequireConnection()
        => Environment.GetEnvironmentVariable("TRANSPORTERP_TEST_CONNSTR")
            ?? throw new InvalidOperationException("TRANSPORTERP_TEST_CONNSTR is required for P2-C01-C PostgreSQL gates.");
}
