using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class P2C01CShippingPostgreSqlIntegrationTests
{
    private const string Issuer = "TransportERP.P2C.Test.Identity";
    private const string Audience = "TransportERP.P2C.Test.Api";
    private const string SigningKey = "transport-erp-p2c-test-signing-key-2026-minimum-32";

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Release_allocate_manifest_load_handover_depart_round_trip_is_consistent_and_audited()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        ShippingScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "C1", quantity: 10m);

        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());

        ItemQuantityStateResponse releaseState;
        await using (var db = CreateP2Db(connection))
        {
            releaseState = await CreateService(db).ReleaseItemAsync(context, scope.WaybillId, scope.ItemId,
                new ReleaseItemRequest(10m, DateTimeOffset.UtcNow, $"release-{Guid.NewGuid():N}"));
        }
        Assert.Equal(0m, releaseState.RemainingToRelease);

        Guid releaseId;
        await using (var db = CreateP2Db(connection))
            releaseId = await db.Set<ItemReleaseEntity>().AsNoTracking().Where(x => x.WaybillItemId == scope.ItemId)
                .Select(x => x.Id).SingleAsync();

        TripResponse trip;
        await using (var db = CreateP2Db(connection))
        {
            trip = await CreateService(db).CreateTripAsync(context,
                new CreateTripRequest($"TR-{Guid.NewGuid():N}"[..20], Guid.NewGuid(), scope.UserId,
                    scope.OriginId, scope.DestinationId, DateTimeOffset.UtcNow.AddHours(1), [],
                    $"trip-{Guid.NewGuid():N}"));
        }
        Assert.Equal("DRAFT", trip.Status);

        AllocationResponse allocation;
        await using (var db = CreateP2Db(connection))
        {
            allocation = await CreateService(db).AllocateAsync(context, trip.Id,
                new AllocateItemRequest(scope.ItemId, releaseId, 10m, $"allocate-{Guid.NewGuid():N}"));
        }
        Assert.Equal("ALLOCATED", allocation.Status);

        ManifestResponse manifest;
        await using (var db = CreateP2Db(connection))
        {
            manifest = await CreateService(db).GenerateManifestAsync(context, trip.Id,
                new GenerateManifestRequest(null, $"manifest-{Guid.NewGuid():N}"));
        }
        Assert.Single(manifest.Lines);
        Assert.Equal(10m, manifest.Lines[0].Quantity);

        await using (var db = CreateP2Db(connection))
        {
            var line = await CreateService(db).LoadManifestLineAsync(context, manifest.Id, manifest.Lines[0].Id,
                new LoadManifestLineRequest(10m, DateTimeOffset.UtcNow, true, $"load-{Guid.NewGuid():N}"));
            Assert.Equal("LOADED", line.LoadStatus);
            Assert.Equal(10m, line.LoadedQuantity);
        }

        await using (var db = CreateP2Db(connection))
        {
            var finalized = await CreateService(db).FinalizeManifestAsync(context, manifest.Id,
                new FinalizeManifestRequest(1, $"finalize-{Guid.NewGuid():N}"));
            Assert.Equal("FINALIZED", finalized.Status);
            manifest = finalized;
        }

        await using (var db = CreateP2Db(connection))
        {
            var accepted = await CreateService(db).HandoverManifestAsync(context, manifest.Id,
                new HandoverManifestRequest(scope.UserId, DateTimeOffset.UtcNow, manifest.Version,
                    $"handover-{Guid.NewGuid():N}"));
            Assert.Equal("ACCEPTED", accepted.Status);
        }

        await using (var db = CreateP2Db(connection))
        {
            var currentTrip = await db.Set<TripEntity>().AsNoTracking().SingleAsync(x => x.Id == trip.Id);
            var departed = await CreateService(db).StartTripAsync(context, trip.Id,
                new StartTripRequest(DateTimeOffset.UtcNow, currentTrip.Version, $"start-{Guid.NewGuid():N}"));
            Assert.Equal("DEPARTED", departed.Status);
        }

        await using var verify = CreateP2Db(connection);
        Assert.Equal(1, await verify.Set<MovementEventEntity>().CountAsync(x => x.ManifestId == manifest.Id && x.EventType == "LOAD"));
        Assert.Equal(1, await verify.Set<MovementEventEntity>().CountAsync(x => x.ManifestId == manifest.Id && x.EventType == "DEPART"));
        Assert.True(await verify.AuditEvents.AsNoTracking().AnyAsync(x => x.Action == "WaybillItemRelease"));
        Assert.True(await verify.AuditEvents.AsNoTracking().AnyAsync(x => x.Action == "WaybillItemAllocate"));
        Assert.True(await verify.AuditEvents.AsNoTracking().AnyAsync(x => x.Action == "ManifestGenerate"));
        Assert.True(await verify.AuditEvents.AsNoTracking().AnyAsync(x => x.Action == "ManifestLineLoad"));
        Assert.True(await verify.AuditEvents.AsNoTracking().AnyAsync(x => x.Action == "ManifestFinalize"));
        Assert.True(await verify.AuditEvents.AsNoTracking().AnyAsync(x => x.Action == "ManifestHandover"));
        Assert.True(await verify.AuditEvents.AsNoTracking().AnyAsync(x => x.Action == "TripStart"));
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Quantity_ledgers_are_idempotent_append_only_and_block_unallocate_after_load()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        ShippingScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "C2", quantity: 5m);
        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        var releaseOp = $"release-{Guid.NewGuid():N}";

        await using (var db = CreateP2Db(connection))
        {
            var service = CreateService(db);
            var first = await service.ReleaseItemAsync(context, scope.WaybillId, scope.ItemId,
                new ReleaseItemRequest(5m, DateTimeOffset.UtcNow, releaseOp));
            var replay = await service.ReleaseItemAsync(context, scope.WaybillId, scope.ItemId,
                new ReleaseItemRequest(5m, DateTimeOffset.UtcNow, releaseOp));
            Assert.Equal(first.ReleasedNet, replay.ReleasedNet);
        }

        Guid releaseId;
        await using (var db = CreateP2Db(connection))
            releaseId = await db.Set<ItemReleaseEntity>().AsNoTracking().Where(x => x.WaybillItemId == scope.ItemId).Select(x => x.Id).SingleAsync();

        TripResponse trip;
        await using (var db = CreateP2Db(connection))
            trip = await CreateService(db).CreateTripAsync(context,
                new CreateTripRequest($"TR-{Guid.NewGuid():N}"[..20], Guid.NewGuid(), scope.UserId,
                    scope.OriginId, scope.DestinationId, DateTimeOffset.UtcNow.AddHours(1), [], $"trip-{Guid.NewGuid():N}"));

        AllocationResponse allocation;
        await using (var db = CreateP2Db(connection))
            allocation = await CreateService(db).AllocateAsync(context, trip.Id,
                new AllocateItemRequest(scope.ItemId, releaseId, 5m, $"alloc-{Guid.NewGuid():N}"));

        ManifestResponse manifest;
        await using (var db = CreateP2Db(connection))
            manifest = await CreateService(db).GenerateManifestAsync(context, trip.Id,
                new GenerateManifestRequest(null, $"manifest-{Guid.NewGuid():N}"));
        await using (var db = CreateP2Db(connection))
            _ = await CreateService(db).LoadManifestLineAsync(context, manifest.Id, manifest.Lines[0].Id,
                new LoadManifestLineRequest(1m, DateTimeOffset.UtcNow, true, $"load-{Guid.NewGuid():N}"));

        await using (var db = CreateP2Db(connection))
        {
            var ex = await Assert.ThrowsAsync<WaybillPersistenceException>(() =>
                CreateService(db).UnallocateAsync(context, allocation.Id,
                    new UnallocateRequest("بعد التحميل", $"unalloc-{Guid.NewGuid():N}")));
            Assert.Equal("ALREADY_LOADED", ex.Code);
        }

        await using (var db = CreateP2Db(connection))
        {
            var release = await db.Set<ItemReleaseEntity>().SingleAsync(x => x.Id == releaseId);
            release.Quantity = 4m;
            await Assert.ThrowsAsync<InvalidOperationException>(() => db.SaveChangesAsync());
        }

        await using (var db = CreateP2Db(connection))
        {
            var movement = await db.Set<MovementEventEntity>().FirstAsync(x => x.ManifestId == manifest.Id && x.EventType == "LOAD");
            db.Remove(movement);
            await Assert.ThrowsAsync<InvalidOperationException>(() => db.SaveChangesAsync());
        }
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Shipping_API_enforces_permission_and_branch_scope()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        ShippingScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedApprovedWaybillAsync(seedDb, "CHTTP", quantity: 3m);

        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        var body = new ReleaseItemRequest(1m, DateTimeOffset.UtcNow, $"http-release-{Guid.NewGuid():N}");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            CreateToken(scope.UserId, scope.CompanyId, scope.BranchId, "waybill.view"));
        var denied = await client.PostAsJsonAsync($"/api/v1/waybills/{scope.WaybillId}/items/{scope.ItemId}/releases", body);
        Assert.Equal(HttpStatusCode.Forbidden, denied.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            CreateToken(scope.UserId, scope.CompanyId, scope.BranchId, ShippingExecutionPermissionCodes.Release));
        var allowed = await client.PostAsJsonAsync($"/api/v1/waybills/{scope.WaybillId}/items/{scope.ItemId}/releases", body);
        Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            CreateToken(scope.UserId, scope.CompanyId, Guid.NewGuid(), ShippingExecutionPermissionCodes.Release));
        var scoped = await client.PostAsJsonAsync($"/api/v1/waybills/{scope.WaybillId}/items/{scope.ItemId}/releases",
            body with { ClientOperationId = $"wrong-branch-{Guid.NewGuid():N}" });
        Assert.Equal(HttpStatusCode.NotFound, scoped.StatusCode);
    }

    private static ShippingExecutionApplicationService CreateService(TransportErpDbContext db)
        => new(new EfShippingExecutionStore(db, new EfWaybillAuditSink(db, new AuditEventService(db))));

    private static string RequireConnection()
        => Environment.GetEnvironmentVariable("TRANSPORTERP_TEST_CONNSTR")
            ?? throw new InvalidOperationException("TRANSPORTERP_TEST_CONNSTR is required for P2-C01-C PostgreSQL gates.");

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
            builder.UseSetting("Auth:SigningKeyId", "test-current");
            builder.ConfigureServices(services =>
            {
                Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.RemoveAll<TransportERP.Api.Security.ICurrentSecurityContext>(services);
                services.AddSingleton<TransportERP.Api.Security.ICurrentSecurityContext, ClaimTestSecurityContext>();
            });
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
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)) { KeyId = "test-current" }, SecurityAlgorithms.HmacSha256)
        };
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(descriptor));
    }

    private static async Task<ShippingScope> SeedApprovedWaybillAsync(TransportErpDbContext db, string suffix, decimal quantity)
    {
        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = await PostgreSqlTestCurrencyCodeAllocator.NextAsync(db), NameAr = "عملة اختبار C",
            MinorUnit = 2, IsBase = true, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"C-{suffix}-{Guid.NewGuid():N}"[..20], LegalNameAr = "شركة اختبار C",
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
            Id = Guid.NewGuid(), UserName = $"p2c-{Guid.NewGuid():N}", NormalizedUserName = $"P2C{suffix}{Guid.NewGuid():N}"[..24],
            DisplayName = "مستخدم اختبار C", PasswordHash = "test-only", SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1, Status = "ACTIVE",
            CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var origin = Guid.NewGuid();
        var destination = Guid.NewGuid();
        var waybill = new WaybillEntity
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, BranchId = branch.Id, DraftNo = $"D-{Guid.NewGuid():N}",
            WaybillNo = $"WB-C-{Guid.NewGuid():N}"[..24], WaybillDateTime = now, ServiceType = "STANDARD", Priority = "NORMAL",
            OriginId = origin, DestinationId = destination, CurrencyId = currency.Id, ExchangeRate = 1m,
            FreightTotal = 100m, DiscountTotal = 0m, Status = "APPROVED", FinancialStatus = "UNPAID",
            CreateClientOperationId = $"seed-create-{Guid.NewGuid():N}", LastClientOperationId = $"seed-approve-{Guid.NewGuid():N}",
            Version = 1, CreatedAt = now, UpdatedAt = now
        };
        var item = new WaybillItemEntity
        {
            Id = Guid.NewGuid(), WaybillId = waybill.Id, LineNo = 1, ItemType = "GENERAL", Contents = "اختبار C",
            Quantity = quantity, Pieces = 1, Weight = 10m, Length = 1m, Width = 1m, Height = 1m,
            RiskFlagsJson = "[]"
        };

        db.Currencies.Add(currency); db.Companies.Add(company); db.Branches.Add(branch); db.Users.Add(user);
        db.Set<WaybillEntity>().Add(waybill); db.Set<WaybillItemEntity>().Add(item);
        await db.SaveChangesAsync();
        return new ShippingScope(company.Id, branch.Id, user.Id, currency.Id, waybill.Id, item.Id, origin, destination);
    }

    private sealed record ShippingScope(
        Guid CompanyId, Guid BranchId, Guid UserId, Guid CurrencyId,
        Guid WaybillId, Guid ItemId, Guid OriginId, Guid DestinationId);
}
