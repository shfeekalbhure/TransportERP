using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class P2C01CConcurrencyPostgreSqlTests
{
    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Concurrent_release_requests_never_overrelease()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        ShippingScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "CR", 10m);

        var context = NewContext(scope);
        var gate = NewGate();
        var first = RunAtGate(gate, async () =>
        {
            await using var db = CreateP2Db(connection);
            await CreateService(db).ReleaseItemAsync(context, scope.WaybillId, scope.ItemId,
                new ReleaseItemRequest(7m, DateTimeOffset.UtcNow, $"release-a-{Guid.NewGuid():N}"));
        });
        var second = RunAtGate(gate, async () =>
        {
            await using var db = CreateP2Db(connection);
            await CreateService(db).ReleaseItemAsync(context, scope.WaybillId, scope.ItemId,
                new ReleaseItemRequest(7m, DateTimeOffset.UtcNow, $"release-b-{Guid.NewGuid():N}"));
        });

        gate.SetResult();
        var results = await Task.WhenAll(first, second);
        Assert.Equal(1, results.Count(x => x == "OK"));
        Assert.Equal(1, results.Count(x => x is "CONCURRENCY_CONFLICT" or "QUANTITY_EXCEEDS_REMAINING"));

        await using var verify = CreateP2Db(connection);
        var releases = await verify.Set<ItemReleaseEntity>().AsNoTracking()
            .Where(x => x.WaybillItemId == scope.ItemId && x.Status == "ACTIVE")
            .ToListAsync();
        Assert.Single(releases);
        Assert.Equal(7m, releases.Sum(x => x.Quantity));
        Assert.True(releases.Sum(x => x.Quantity) <= 10m);
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Concurrent_allocations_never_exceed_released_quantity()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        ShippingScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "CA", 10m);
        var context = NewContext(scope);

        await using (var db = CreateP2Db(connection))
            await CreateService(db).ReleaseItemAsync(context, scope.WaybillId, scope.ItemId,
                new ReleaseItemRequest(10m, DateTimeOffset.UtcNow, $"release-{Guid.NewGuid():N}"));

        Guid releaseId;
        await using (var db = CreateP2Db(connection))
            releaseId = await db.Set<ItemReleaseEntity>().AsNoTracking()
                .Where(x => x.WaybillItemId == scope.ItemId && x.Status == "ACTIVE")
                .Select(x => x.Id).SingleAsync();

        var trip1 = await CreateTripAsync(connection, context, scope, "A");
        var trip2 = await CreateTripAsync(connection, context, scope, "B");

        var gate = NewGate();
        var first = RunAtGate(gate, async () =>
        {
            await using var db = CreateP2Db(connection);
            await CreateService(db).AllocateAsync(context, trip1.Id,
                new AllocateItemRequest(scope.ItemId, releaseId, 7m, $"alloc-a-{Guid.NewGuid():N}"));
        });
        var second = RunAtGate(gate, async () =>
        {
            await using var db = CreateP2Db(connection);
            await CreateService(db).AllocateAsync(context, trip2.Id,
                new AllocateItemRequest(scope.ItemId, releaseId, 7m, $"alloc-b-{Guid.NewGuid():N}"));
        });

        gate.SetResult();
        var results = await Task.WhenAll(first, second);
        Assert.Equal(1, results.Count(x => x == "OK"));
        Assert.Equal(1, results.Count(x => x is "CONCURRENCY_CONFLICT" or "QUANTITY_EXCEEDS_RELEASED"));

        await using var verify = CreateP2Db(connection);
        var allocations = await verify.Set<TripAllocationEntity>().AsNoTracking()
            .Where(x => x.ReleaseId == releaseId && x.Status == "ALLOCATED")
            .ToListAsync();
        Assert.Single(allocations);
        Assert.Equal(7m, allocations.Sum(x => x.Quantity));
        Assert.True(allocations.Sum(x => x.Quantity) <= 10m);
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Concurrent_loads_never_exceed_manifest_line_quantity()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        ShippingScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "CL", 10m);
        var context = NewContext(scope);

        await using (var db = CreateP2Db(connection))
            await CreateService(db).ReleaseItemAsync(context, scope.WaybillId, scope.ItemId,
                new ReleaseItemRequest(10m, DateTimeOffset.UtcNow, $"release-{Guid.NewGuid():N}"));

        Guid releaseId;
        await using (var db = CreateP2Db(connection))
            releaseId = await db.Set<ItemReleaseEntity>().AsNoTracking()
                .Where(x => x.WaybillItemId == scope.ItemId && x.Status == "ACTIVE")
                .Select(x => x.Id).SingleAsync();

        var trip = await CreateTripAsync(connection, context, scope, "L");
        await using (var db = CreateP2Db(connection))
            await CreateService(db).AllocateAsync(context, trip.Id,
                new AllocateItemRequest(scope.ItemId, releaseId, 10m, $"alloc-{Guid.NewGuid():N}"));

        ManifestResponse manifest;
        await using (var db = CreateP2Db(connection))
            manifest = await CreateService(db).GenerateManifestAsync(context, trip.Id,
                new GenerateManifestRequest(null, $"manifest-{Guid.NewGuid():N}"));
        var lineId = Assert.Single(manifest.Lines).Id;

        var gate = NewGate();
        var first = RunAtGate(gate, async () =>
        {
            await using var db = CreateP2Db(connection);
            await CreateService(db).LoadManifestLineAsync(context, manifest.Id, lineId,
                new LoadManifestLineRequest(7m, DateTimeOffset.UtcNow, true, $"load-a-{Guid.NewGuid():N}"));
        });
        var second = RunAtGate(gate, async () =>
        {
            await using var db = CreateP2Db(connection);
            await CreateService(db).LoadManifestLineAsync(context, manifest.Id, lineId,
                new LoadManifestLineRequest(7m, DateTimeOffset.UtcNow, true, $"load-b-{Guid.NewGuid():N}"));
        });

        gate.SetResult();
        var results = await Task.WhenAll(first, second);
        Assert.Equal(1, results.Count(x => x == "OK"));
        Assert.Equal(1, results.Count(x => x is "CONCURRENCY_CONFLICT" or "QUANTITY_EXCEEDS_ALLOCATION"));

        await using var verify = CreateP2Db(connection);
        var line = await verify.Set<ManifestLineEntity>().AsNoTracking().SingleAsync(x => x.Id == lineId);
        var loaded = await verify.Set<MovementEventEntity>().AsNoTracking()
            .Where(x => x.ManifestLineId == lineId && x.EventType == "LOAD")
            .SumAsync(x => x.Quantity ?? 0m);
        Assert.Equal(7m, loaded);
        Assert.Equal(7m, line.LoadedQuantity);
        Assert.True(loaded <= line.Quantity);
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task A_trip_cannot_acquire_a_second_manifest_lifecycle()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        ShippingScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "CM", 10m);
        var context = NewContext(scope);

        await using (var db = CreateP2Db(connection))
            await CreateService(db).ReleaseItemAsync(context, scope.WaybillId, scope.ItemId,
                new ReleaseItemRequest(10m, DateTimeOffset.UtcNow, $"release-{Guid.NewGuid():N}"));

        Guid releaseId;
        await using (var db = CreateP2Db(connection))
            releaseId = await db.Set<ItemReleaseEntity>().AsNoTracking()
                .Where(x => x.WaybillItemId == scope.ItemId && x.Status == "ACTIVE")
                .Select(x => x.Id).SingleAsync();

        var trip = await CreateTripAsync(connection, context, scope, "M");
        await using (var db = CreateP2Db(connection))
            await CreateService(db).AllocateAsync(context, trip.Id,
                new AllocateItemRequest(scope.ItemId, releaseId, 5m, $"alloc-a-{Guid.NewGuid():N}"));

        await using (var db = CreateP2Db(connection))
            _ = await CreateService(db).GenerateManifestAsync(context, trip.Id,
                new GenerateManifestRequest(null, $"manifest-a-{Guid.NewGuid():N}"));

        await using (var db = CreateP2Db(connection))
            await CreateService(db).AllocateAsync(context, trip.Id,
                new AllocateItemRequest(scope.ItemId, releaseId, 5m, $"alloc-b-{Guid.NewGuid():N}"));

        await using (var db = CreateP2Db(connection))
        {
            var ex = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                CreateService(db).GenerateManifestAsync(context, trip.Id,
                    new GenerateManifestRequest(null, $"manifest-b-{Guid.NewGuid():N}")));
            Assert.Equal("DUPLICATE_OPERATION", ex.Code);
        }

        await using var verify = CreateP2Db(connection);
        Assert.Equal(1, await verify.Set<ManifestEntity>().AsNoTracking().CountAsync(x => x.TripId == trip.Id));
    }

    private static OperationContext NewContext(ShippingScope scope)
        => new(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());

    private static TaskCompletionSource NewGate()
        => new(TaskCreationOptions.RunContinuationsAsynchronously);

    private static async Task<string> RunAtGate(TaskCompletionSource gate, Func<Task> action)
    {
        await gate.Task;
        try
        {
            await action();
            return "OK";
        }
        catch (WaybillPersistenceException ex)
        {
            return ex.Code;
        }
        catch (ShippingExecutionRuleException ex)
        {
            return ex.Code;
        }
    }

    private static async Task<TripResponse> CreateTripAsync(
        string connection,
        OperationContext context,
        ShippingScope scope,
        string suffix)
    {
        await using var db = CreateP2Db(connection);
        return await CreateService(db).CreateTripAsync(context,
            new CreateTripRequest($"TR-{suffix}-{Guid.NewGuid():N}"[..24], Guid.NewGuid(), scope.UserId,
                scope.OriginId, scope.DestinationId, DateTimeOffset.UtcNow.AddHours(1), [],
                $"trip-{suffix}-{Guid.NewGuid():N}"));
    }

    private static ShippingExecutionApplicationService CreateService(TransportErpDbContext db)
        => new(new EfShippingExecutionStore(db, new EfWaybillAuditSink(db, new AuditEventService(db))));

    private static string RequireConnection()
        => Environment.GetEnvironmentVariable("TRANSPORTERP_TEST_CONNSTR")
            ?? throw new InvalidOperationException("TRANSPORTERP_TEST_CONNSTR is required for P2-C01-C concurrency gates.");

    private static async Task EnsureMigratedAsync(string connection)
    {
        await using var db = CreateP2Db(connection);
        await db.Database.MigrateAsync();
    }

    private static TransportErpDbContext CreateP2Db(string connection)
        => new(new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseNpgsql(connection, npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "transport_erp"))
            .ReplaceService<IModelCustomizer, TransportErpP2CombinedModelCustomizer>()
            .AddInterceptors(new P2FinanceAppendOnlyInterceptor(), new P2ShippingAppendOnlyInterceptor())
            .Options);

    private static async Task<ShippingScope> SeedApprovedWaybillAsync(
        TransportErpDbContext db,
        string suffix,
        decimal quantity)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = Guid.NewGuid().ToString("N")[..3].ToUpperInvariant(),
            NameAr = "عملة اختبار تزامن C", MinorUnit = 2, IsBase = true, Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"C-{suffix}-{Guid.NewGuid():N}"[..20],
            LegalNameAr = "شركة اختبار تزامن C", BaseCurrencyId = currency.Id,
            DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = "MAIN", NameAr = "الفرع الرئيسي",
            Timezone = "Asia/Aden", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = $"p2c-conc-{Guid.NewGuid():N}",
            NormalizedUserName = $"P2CC{suffix}{Guid.NewGuid():N}"[..24],
            DisplayName = "مستخدم تزامن C", PasswordHash = "test-only", SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1, Status = "ACTIVE",
            CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var origin = Guid.NewGuid();
        var destination = Guid.NewGuid();
        var waybill = new WaybillEntity
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, BranchId = branch.Id,
            DraftNo = $"D-{Guid.NewGuid():N}", WaybillNo = $"WB-C-{Guid.NewGuid():N}"[..24],
            WaybillDateTime = now, ServiceType = "STANDARD", Priority = "NORMAL",
            OriginId = origin, DestinationId = destination, CurrencyId = currency.Id, ExchangeRate = 1m,
            FreightTotal = 100m, DiscountTotal = 0m, Status = "APPROVED", FinancialStatus = "UNPAID",
            CreateClientOperationId = $"seed-create-{Guid.NewGuid():N}",
            LastClientOperationId = $"seed-approve-{Guid.NewGuid():N}",
            Version = 1, CreatedAt = now, UpdatedAt = now
        };
        var item = new WaybillItemEntity
        {
            Id = Guid.NewGuid(), WaybillId = waybill.Id, LineNo = 1,
            ItemType = "GENERAL", Contents = "اختبار تزامن C", Quantity = quantity,
            Pieces = 1, Weight = 10m, Length = 1m, Width = 1m, Height = 1m,
            RiskFlagsJson = "[]"
        };

        db.Currencies.Add(currency);
        db.Companies.Add(company);
        db.Branches.Add(branch);
        db.Users.Add(user);
        db.Set<WaybillEntity>().Add(waybill);
        db.Set<WaybillItemEntity>().Add(item);
        await db.SaveChangesAsync();
        return new ShippingScope(company.Id, branch.Id, user.Id, waybill.Id, item.Id, origin, destination);
    }

    private sealed record ShippingScope(
        Guid CompanyId,
        Guid BranchId,
        Guid UserId,
        Guid WaybillId,
        Guid ItemId,
        Guid OriginId,
        Guid DestinationId);
}
