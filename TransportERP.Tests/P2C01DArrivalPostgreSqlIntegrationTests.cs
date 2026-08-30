using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class P2C01DArrivalPostgreSqlIntegrationTests
{
    private const string Issuer = "TransportERP.P2D.PgSql.Test.Identity";
    private const string Audience = "TransportERP.P2D.PgSql.Test.Api";
    private const string SigningKey = "transport-erp-p2d-pgsql-test-signing-key-2026-minimum-32";

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Record_arrival_creates_receipt_and_arrive_movement_events()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        var scope = await SeedDepartedTripAsync(connection, "D1", quantity: 10m);
        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());

        ArrivalReceiptResponse receipt;
        await using (var db = CreateP2Db(connection))
        {
            receipt = await CreateService(db).RecordArrivalAsync(context, scope.TripId,
                new RecordArrivalRequest(scope.ManifestId, scope.DestinationId, DateTimeOffset.UtcNow, $"arrival-{Guid.NewGuid():N}"));
        }

        Assert.Equal("DRAFT", receipt.Status);
        Assert.Single(receipt.Lines);

        await using var verify = CreateP2Db(connection);
        Assert.Equal(1, await verify.Set<ArrivalReceiptEntity>().CountAsync(x => x.TripId == scope.TripId));
        Assert.Equal(1, await verify.Set<ArrivalReceiptLineEntity>().CountAsync(x => x.ArrivalReceiptId == receipt.Id));
        Assert.Equal(1, await verify.Set<MovementEventEntity>().CountAsync(x =>
            x.TripId == scope.TripId && x.ManifestId == scope.ManifestId && x.EventType == "ARRIVE"));
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Record_unload_updates_receipt_line_quantities()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        var scope = await SeedDepartedTripAsync(connection, "D2", quantity: 10m);
        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());

        ArrivalReceiptResponse receipt;
        await using (var db = CreateP2Db(connection))
        {
            receipt = await CreateService(db).RecordArrivalAsync(context, scope.TripId,
                new RecordArrivalRequest(scope.ManifestId, scope.DestinationId, DateTimeOffset.UtcNow, $"arrival-{Guid.NewGuid():N}"));
        }

        await using (var db = CreateP2Db(connection))
        {
            var line = receipt.Lines[0];
            var evidenceId = Guid.NewGuid();
            _ = await CreateService(db).RecordUnloadAsync(context, receipt.Id,
                new RecordUnloadRequest(
                    [new ArrivalUnloadLineInput(line.ManifestLineId, 8m, 1m, "SHORT_AND_DAMAGE", evidenceId, "notes")],
                    DateTimeOffset.UtcNow,
                    $"unload-{Guid.NewGuid():N}"));
        }

        await using var verify = CreateP2Db(connection);
        var updatedLine = await verify.Set<ArrivalReceiptLineEntity>().SingleAsync(x => x.ArrivalReceiptId == receipt.Id);
        Assert.Equal(8m, updatedLine.ActualQty);
        Assert.Equal(1m, updatedLine.DamageQty);
        Assert.Equal("SHORT_AND_DAMAGE", updatedLine.DifferenceType);
        Assert.Equal(1, await verify.Set<MovementEventEntity>().CountAsync(x =>
            x.TripId == scope.TripId && x.EventType == "UNLOAD"));
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Reallocate_transit_creates_warehouse_holding()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        var scope = await SeedDepartedTripAsync(connection, "D3", quantity: 10m);
        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        var transitLocationId = Guid.NewGuid();

        await using (var seed = CreateP2Db(connection))
        {
            seed.Set<TripStopEntity>().Add(new TripStopEntity
            {
                Id = Guid.NewGuid(),
                TripId = scope.TripId,
                StopNo = 1,
                LocationId = transitLocationId,
                Status = "PLANNED"
            });
            await seed.SaveChangesAsync();
        }

        ArrivalReceiptResponse receipt;
        await using (var db = CreateP2Db(connection))
        {
            receipt = await CreateService(db).RecordArrivalAsync(context, scope.TripId,
                new RecordArrivalRequest(scope.ManifestId, transitLocationId, DateTimeOffset.UtcNow, $"arrival-{Guid.NewGuid():N}"));
        }

        await using (var db = CreateP2Db(connection))
        {
            var line = receipt.Lines[0];
            _ = await CreateService(db).RecordUnloadAsync(context, receipt.Id,
                new RecordUnloadRequest(
                    [new ArrivalUnloadLineInput(line.ManifestLineId, 10m, 0m, "NONE", null, null)],
                    DateTimeOffset.UtcNow,
                    $"unload-{Guid.NewGuid():N}"));
        }

        var nextTrip = await CreateNextTripAsync(connection, context);
        await using (var db = CreateP2Db(connection))
        {
            var holding = await db.Set<WarehouseHoldingEntity>().AsNoTracking().SingleAsync(x => x.WaybillItemId == scope.ItemId);
            Assert.Equal("TRANSIT", holding.HoldingType);
            _ = await CreateService(db).ReallocateTransitAsync(context, holding.Id,
                new ReallocateTransitRequest(nextTrip.Id, 10m, $"reallocate-{Guid.NewGuid():N}"));
        }

        await using var verify = CreateP2Db(connection);
        Assert.Equal(1, await verify.Set<MovementEventEntity>().CountAsync(x =>
            x.TripId == scope.TripId && x.EventType == "REALLOCATE"));
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Finalize_arrival_transitions_receipt_to_finalized()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        var scope = await SeedDepartedTripAsync(connection, "D4", quantity: 10m);
        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());

        ArrivalReceiptResponse receipt;
        await using (var db = CreateP2Db(connection))
        {
            receipt = await CreateService(db).RecordArrivalAsync(context, scope.TripId,
                new RecordArrivalRequest(scope.ManifestId, scope.DestinationId, DateTimeOffset.UtcNow, $"arrival-{Guid.NewGuid():N}"));
        }

        await using (var db = CreateP2Db(connection))
        {
            var line = receipt.Lines[0];
            receipt = await CreateService(db).RecordUnloadAsync(context, receipt.Id,
                new RecordUnloadRequest(
                    [new ArrivalUnloadLineInput(line.ManifestLineId, 10m, 0m, "NONE", null, null)],
                    DateTimeOffset.UtcNow,
                    $"unload-{Guid.NewGuid():N}"));
        }

        ArrivalReceiptResponse finalized;
        await using (var db = CreateP2Db(connection))
        {
            finalized = await CreateService(db).FinalizeArrivalAsync(context, receipt.Id,
                new FinalizeArrivalRequest(receipt.Version, $"finalize-{Guid.NewGuid():N}"));
        }

        Assert.Equal("FINALIZED", finalized.Status);
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task CloseTrip_blocks_when_blocking_exception_is_open()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        var scope = await SeedDepartedTripAsync(connection, "D5", quantity: 10m);
        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());

        await using (var db = CreateP2Db(connection))
        {
            var receipt = await CreateService(db).RecordArrivalAsync(context, scope.TripId,
                new RecordArrivalRequest(scope.ManifestId, scope.DestinationId, DateTimeOffset.UtcNow, $"arrival-{Guid.NewGuid():N}"));
            var line = receipt.Lines[0];
            _ = await CreateService(db).RecordUnloadAsync(context, receipt.Id,
                new RecordUnloadRequest(
                    [new ArrivalUnloadLineInput(line.ManifestLineId, 10m, 0m, "NONE", null, null)],
                    DateTimeOffset.UtcNow,
                    $"unload-{Guid.NewGuid():N}"));
        }

        await using (var db = CreateP2Db(connection))
        {
            db.Set<ShipmentExceptionEntity>().Add(new ShipmentExceptionEntity
            {
                Id = Guid.NewGuid(),
                CompanyId = scope.CompanyId,
                BranchId = scope.BranchId,
                TripId = scope.TripId,
                WaybillId = scope.WaybillId,
                ExceptionType = "DAMAGED_CARGO",
                Severity = "BLOCKING",
                Status = "OPEN",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });
            await db.SaveChangesAsync();
        }

        await using (var db = CreateP2Db(connection))
        {
            var currentTrip = await db.Set<TripEntity>().AsNoTracking().SingleAsync(x => x.Id == scope.TripId);
            var ex = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                CreateService(db).CloseTripAsync(context, scope.TripId,
                    new CloseTripRequest(DateTimeOffset.UtcNow, currentTrip.Version, $"close-{Guid.NewGuid():N}")));
            Assert.Equal("EXCEPTION_BLOCKED", ex.Code);
        }
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Record_arrival_is_idempotent_under_retry()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        var scope = await SeedDepartedTripAsync(connection, "D6", quantity: 10m);
        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        var opId = $"arrival-{Guid.NewGuid():N}";
        var receivedAt = DateTimeOffset.UtcNow;

        ArrivalReceiptResponse first;
        await using (var db = CreateP2Db(connection))
        {
            first = await CreateService(db).RecordArrivalAsync(context, scope.TripId,
                new RecordArrivalRequest(scope.ManifestId, scope.DestinationId, receivedAt, opId));
        }

        ArrivalReceiptResponse replay;
        await using (var db = CreateP2Db(connection))
        {
            replay = await CreateService(db).RecordArrivalAsync(context, scope.TripId,
                new RecordArrivalRequest(scope.ManifestId, scope.DestinationId, receivedAt, opId));
        }

        Assert.Equal(first.Id, replay.Id);

        await using var verify = CreateP2Db(connection);
        Assert.Equal(1, await verify.Set<ArrivalReceiptEntity>().CountAsync(x => x.TripId == scope.TripId));
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Arrival_API_enforces_permission_and_branch_scope()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        var scope = await SeedDepartedTripAsync(connection, "DHTTP", quantity: 5m);

        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        var body = new RecordArrivalRequest(scope.ManifestId, scope.DestinationId, DateTimeOffset.UtcNow, $"http-arrival-{Guid.NewGuid():N}");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            CreateToken(scope.UserId, scope.CompanyId, scope.BranchId, "waybill.view"));
        var denied = await client.PostAsJsonAsync($"/api/v1/trips/{scope.TripId}/arrivals", body);
        Assert.Equal(HttpStatusCode.Forbidden, denied.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            CreateToken(scope.UserId, scope.CompanyId, scope.BranchId, ArrivalExecutionPermissionCodes.RecordArrival));
        var allowed = await client.PostAsJsonAsync($"/api/v1/trips/{scope.TripId}/arrivals", body);
        Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            CreateToken(scope.UserId, scope.CompanyId, Guid.NewGuid(), ArrivalExecutionPermissionCodes.RecordArrival));
        var scoped = await client.PostAsJsonAsync($"/api/v1/trips/{scope.TripId}/arrivals",
            body with { ClientOperationId = $"wrong-branch-{Guid.NewGuid():N}" });
        Assert.Equal(HttpStatusCode.Forbidden, scoped.StatusCode);
    }

    private static ArrivalExecutionApplicationService CreateService(TransportErpDbContext db)
        => new(new EfArrivalExecutionStore(db, new EfWaybillAuditSink(db, new AuditEventService(db))));

    private static ShippingExecutionApplicationService CreateShippingService(TransportErpDbContext db)
        => new(new EfShippingExecutionStore(db, new EfWaybillAuditSink(db, new AuditEventService(db))));

    private static string RequireConnection()
        => Environment.GetEnvironmentVariable("TRANSPORTERP_TEST_CONNSTR")
            ?? throw new InvalidOperationException("TRANSPORTERP_TEST_CONNSTR is required for P2-C01-D PostgreSQL gates.");

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

    private static WebApplicationFactory<Program> CreateFactory(string connection)
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", connection);
            builder.UseSetting("Auth:Issuer", Issuer);
            builder.UseSetting("Auth:Audience", Audience);
            builder.UseSetting("Auth:SigningKey", SigningKey);
        });

    private static string CreateToken(Guid userId, Guid companyId, Guid branchId, string permission)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("company_id", companyId.ToString()),
            new Claim("branch_id", branchId.ToString()),
            new Claim("permission", permission)
        };
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), Issuer = Issuer, Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)), SecurityAlgorithms.HmacSha256)
        };
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(descriptor));
    }

    private static async Task<ShippingScope> SeedDepartedTripAsync(string connection, string suffix, decimal quantity)
    {
        await using var seedDb = CreateP2Db(connection);
        var scope = await SeedApprovedWaybillAsync(seedDb, suffix, quantity);
        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());

        var shipping = CreateShippingService(seedDb);
        var release = await shipping.ReleaseItemAsync(context, scope.WaybillId, scope.ItemId,
            new ReleaseItemRequest(quantity, DateTimeOffset.UtcNow, $"release-{suffix}-{Guid.NewGuid():N}"));

        Guid releaseId;
        await using (var db = CreateP2Db(connection))
            releaseId = await db.Set<ItemReleaseEntity>().AsNoTracking().Where(x => x.WaybillItemId == scope.ItemId)
                .Select(x => x.Id).SingleAsync();

        var trip = await shipping.CreateTripAsync(context,
            new CreateTripRequest($"TR-{suffix}-{Guid.NewGuid():N}"[..20], Guid.NewGuid(), scope.UserId,
                scope.OriginId, scope.DestinationId, DateTimeOffset.UtcNow.AddHours(1), [],
                $"trip-{suffix}-{Guid.NewGuid():N}"));

        var allocation = await shipping.AllocateAsync(context, trip.Id,
            new AllocateItemRequest(scope.ItemId, releaseId, quantity, $"alloc-{suffix}-{Guid.NewGuid():N}"));

        var manifest = await shipping.GenerateManifestAsync(context, trip.Id,
            new GenerateManifestRequest(null, $"manifest-{suffix}-{Guid.NewGuid():N}"));

        _ = await shipping.LoadManifestLineAsync(context, manifest.Id, manifest.Lines[0].Id,
            new LoadManifestLineRequest(quantity, DateTimeOffset.UtcNow, true, $"load-{suffix}-{Guid.NewGuid():N}"));

        var finalized = await shipping.FinalizeManifestAsync(context, manifest.Id,
            new FinalizeManifestRequest(manifest.Version, $"finalize-{suffix}-{Guid.NewGuid():N}"));

        var accepted = await shipping.HandoverManifestAsync(context, manifest.Id,
            new HandoverManifestRequest(scope.UserId, DateTimeOffset.UtcNow, finalized.Version,
                $"handover-{suffix}-{Guid.NewGuid():N}"));

        var currentTrip = await seedDb.Set<TripEntity>().AsNoTracking().SingleAsync(x => x.Id == trip.Id);
        var departed = await shipping.StartTripAsync(context, trip.Id,
            new StartTripRequest(DateTimeOffset.UtcNow, currentTrip.Version, $"start-{suffix}-{Guid.NewGuid():N}"));

        return scope with { TripId = trip.Id, ManifestId = accepted.Id, ManifestLineId = manifest.Lines[0].Id };
    }

    private static async Task<TripResponse> CreateNextTripAsync(string connection, OperationContext context)
    {
        await using var db = CreateP2Db(connection);
        var origin = Guid.NewGuid();
        var destination = Guid.NewGuid();
        return await CreateShippingService(db).CreateTripAsync(context,
            new CreateTripRequest($"TR-NEXT-{Guid.NewGuid():N}"[..20], Guid.NewGuid(), context.UserId,
                origin, destination, DateTimeOffset.UtcNow.AddHours(1), [],
                $"next-trip-{Guid.NewGuid():N}"));
    }

    private static async Task<ShippingScope> SeedApprovedWaybillAsync(TransportErpDbContext db, string suffix, decimal quantity)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = Guid.NewGuid().ToString("N")[..3].ToUpperInvariant(), NameAr = "عملة اختبار D",
            MinorUnit = 2, IsBase = true, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"D-{suffix}-{Guid.NewGuid():N}"[..20], LegalNameAr = "شركة اختبار D",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
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
            Id = Guid.NewGuid(), UserName = $"p2d-{Guid.NewGuid():N}", NormalizedUserName = $"P2D{suffix}{Guid.NewGuid():N}"[..24],
            DisplayName = "مستخدم اختبار D", PasswordHash = "test-only", Status = "ACTIVE",
            CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var origin = Guid.NewGuid();
        var destination = Guid.NewGuid();
        var waybill = new WaybillEntity
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, BranchId = branch.Id, DraftNo = $"D-{Guid.NewGuid():N}",
            WaybillNo = $"WB-D-{Guid.NewGuid():N}"[..24], WaybillDateTime = now, ServiceType = "STANDARD", Priority = "NORMAL",
            OriginId = origin, DestinationId = destination, CurrencyId = currency.Id, ExchangeRate = 1m,
            FreightTotal = 100m, DiscountTotal = 0m, Status = "APPROVED", FinancialStatus = "UNPAID",
            CreateClientOperationId = $"seed-create-{Guid.NewGuid():N}", LastClientOperationId = $"seed-approve-{Guid.NewGuid():N}",
            Version = 1, CreatedAt = now, UpdatedAt = now
        };
        var item = new WaybillItemEntity
        {
            Id = Guid.NewGuid(), WaybillId = waybill.Id, LineNo = 1, ItemType = "GENERAL", Contents = "اختبار D",
            Quantity = quantity, Pieces = 1, Weight = 10m, Length = 1m, Width = 1m, Height = 1m,
            RiskFlagsJson = "[]"
        };

        db.Currencies.Add(currency); db.Companies.Add(company); db.Branches.Add(branch); db.Users.Add(user);
        db.Set<WaybillEntity>().Add(waybill); db.Set<WaybillItemEntity>().Add(item);
        await db.SaveChangesAsync();
        return new ShippingScope(company.Id, branch.Id, user.Id, currency.Id, waybill.Id, item.Id, origin, destination, Guid.Empty, Guid.Empty, Guid.Empty);
    }

    private sealed record ShippingScope(
        Guid CompanyId, Guid BranchId, Guid UserId, Guid CurrencyId,
        Guid WaybillId, Guid ItemId, Guid OriginId, Guid DestinationId,
        Guid TripId, Guid ManifestId, Guid ManifestLineId);
}

