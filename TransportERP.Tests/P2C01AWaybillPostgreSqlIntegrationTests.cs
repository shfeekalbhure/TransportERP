using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Geo;
using TransportERP.Contracts.Numbering;
using TransportERP.Contracts.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class P2C01AWaybillPostgreSqlIntegrationTests
{
    private const string Issuer = "TransportERP.P2.Test.Identity";
    private const string Audience = "TransportERP.P2.Test.Api";
    private const string SigningKey = "transport-erp-p2-test-signing-key-2026-minimum-32";

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Migration_and_waybill_approval_round_trip_are_atomic_and_audited()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        TestScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedScopeAsync(seedDb, "P2A1", withSequence: true);

        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        WaybillResponse draft;
        await using (var db = CreateP2Db(connection))
            draft = await CreateService(db).CreateDraftAsync(context, new CreateWaybillDraftRequest(
                scope.BranchId, DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), scope.CurrencyId,
                1m, "STANDARD", "NORMAL", $"create-{Guid.NewGuid():N}"));

        var address = new GeoAddressSnapshot(null, null, null, null, "عنوان اختبار PostgreSQL");
        WaybillResponse updated;
        await using (var db = CreateP2Db(connection))
            updated = await CreateService(db).UpdateDraftAsync(context, draft.Id, new UpdateWaybillDraftRequest(
                draft.Version, draft.WaybillDateTime, draft.OriginId, draft.DestinationId, draft.CurrencyId,
                1m, 250m, 25m, "STANDARD", "NORMAL",
                [
                    new WaybillPartyInput("SENDER", null, "مرسل اختبار", "777100001", null, null, address),
                    new WaybillPartyInput("RECEIVER", null, "مستلم اختبار", "777100002", null, null, address)
                ],
                [new WaybillItemInput(null, 1, "GENERAL", "طرود اختبار", 2m, 2, 12m, null, null, null, 500m, null, [], null)],
                $"update-{Guid.NewGuid():N}"));

        WaybillResponse submitted;
        await using (var db = CreateP2Db(connection))
            submitted = await CreateService(db).SubmitAsync(context, draft.Id,
                new SubmitWaybillRequest(updated.Version, $"submit-{Guid.NewGuid():N}"));

        var approveKey = $"approve-{Guid.NewGuid():N}";
        WaybillResponse approved;
        await using (var db = CreateP2Db(connection))
            approved = await CreateService(db).ApproveAsync(context, draft.Id,
                new ApproveWaybillRequest(submitted.Version, scope.SequenceId!.Value, approveKey));

        await using var verifyDb = CreateP2Db(connection);
        var persisted = await verifyDb.Set<WaybillEntity>().AsNoTracking().SingleAsync(x => x.Id == draft.Id);
        var reservation = await verifyDb.Set<NumberReservationEntity>().AsNoTracking()
            .SingleAsync(x => x.WaybillId == draft.Id);
        var audit = await verifyDb.AuditEvents.AsNoTracking()
            .SingleAsync(x => x.EntityType == "Waybill" && x.EntityId == draft.Id && x.Action == "WaybillApprove");

        Assert.Equal("APPROVED", approved.Status);
        Assert.NotNull(approved.WaybillNo);
        Assert.Equal(approved.WaybillNo, persisted.WaybillNo);
        Assert.Equal("APPROVED", persisted.Status);
        Assert.Equal("COMMITTED", reservation.State);
        Assert.Equal(approved.WaybillNo, reservation.RenderedNumber);
        Assert.Equal(context.CorrelationId, audit.CorrelationId);
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Idempotent_create_and_number_reservation_never_duplicate_under_retry()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        TestScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedScopeAsync(seedDb, "P2A2", withSequence: true);

        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        var createKey = $"create-{Guid.NewGuid():N}";
        var request = new CreateWaybillDraftRequest(
            scope.BranchId, DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), scope.CurrencyId,
            1m, "STANDARD", "NORMAL", createKey);

        WaybillResponse first;
        await using (var db = CreateP2Db(connection))
            first = await CreateService(db).CreateDraftAsync(context, request);
        WaybillResponse replay;
        await using (var db = CreateP2Db(connection))
            replay = await CreateService(db).CreateDraftAsync(context, request);

        Assert.Equal(first.Id, replay.Id);

        var numberKey = $"number-{Guid.NewGuid():N}";
        NumberReservationDto r1;
        await using (var db = CreateP2Db(connection))
            r1 = await new EfNumberReservationService(db).ReserveAsync(context,
                new NumberReservationRequest(scope.SequenceId!.Value, numberKey, "TEST"));
        NumberReservationDto r2;
        await using (var db = CreateP2Db(connection))
            r2 = await new EfNumberReservationService(db).ReserveAsync(context,
                new NumberReservationRequest(scope.SequenceId!.Value, numberKey, "TEST_RETRY"));

        await using var verifyDb = CreateP2Db(connection);
        Assert.Equal(1, await verifyDb.Set<WaybillEntity>().CountAsync(x =>
            x.CompanyId == scope.CompanyId && x.BranchId == scope.BranchId && x.CreateClientOperationId == createKey));
        Assert.Equal(1, await verifyDb.AuditEvents.CountAsync(x =>
            x.EntityId == first.Id && x.Action == "WaybillDraftCreate"));
        Assert.Equal(r1.Id, r2.Id);
        Assert.Equal(r1.RenderedNumber, r2.RenderedNumber);
        Assert.Equal(1, await verifyDb.Set<NumberReservationEntity>().CountAsync(x =>
            x.CompanyId == scope.CompanyId && x.IdempotencyKey == numberKey));
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Concurrent_create_with_same_operation_returns_one_waybill_and_one_audit()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        TestScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedScopeAsync(seedDb, "P2A2RACE", withSequence: false);

        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        var createKey = $"create-race-{Guid.NewGuid():N}";
        var request = new CreateWaybillDraftRequest(
            scope.BranchId, DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), scope.CurrencyId,
            1m, "STANDARD", "NORMAL", createKey);
        var start = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        async Task<WaybillResponse> CreateAsync()
        {
            await using var db = CreateP2Db(connection);
            await start.Task;
            return await CreateService(db).CreateDraftAsync(context, request);
        }

        var first = CreateAsync();
        var second = CreateAsync();
        start.SetResult();
        var results = await Task.WhenAll(first, second);

        Assert.Equal(results[0].Id, results[1].Id);
        await using var verifyDb = CreateP2Db(connection);
        Assert.Equal(1, await verifyDb.Set<WaybillEntity>().CountAsync(x =>
            x.CompanyId == scope.CompanyId && x.BranchId == scope.BranchId &&
            x.CreateClientOperationId == createKey));
        Assert.Equal(1, await verifyDb.AuditEvents.CountAsync(x =>
            x.EntityType == "Waybill" && x.EntityId == results[0].Id && x.Action == "WaybillDraftCreate"));
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Concurrent_party_create_with_same_operation_returns_one_party_and_one_audit()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        TestScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedScopeAsync(seedDb, "P2PARTYRACE", withSequence: false);

        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        var operationId = $"party-race-{Guid.NewGuid():N}";
        var request = new OperationalPartyCreateRequest(
            "طرف اختبار متزامن", "777100003", null, null,
            new GeoAddressSnapshot(null, null, null, null, "عنوان الطرف المتزامن"), operationId);
        var start = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        async Task<OperationalPartyResponse> CreateAsync()
        {
            await using var db = CreateP2Db(connection);
            await start.Task;
            return await CreateService(db).CreatePartyAsync(context, request);
        }

        var first = CreateAsync();
        var second = CreateAsync();
        start.SetResult();
        var results = await Task.WhenAll(first, second);

        Assert.Equal(results[0].Id, results[1].Id);
        await using var verifyDb = CreateP2Db(connection);
        Assert.Equal(1, await verifyDb.Set<OperationalPartyEntity>().CountAsync(x =>
            x.CompanyId == scope.CompanyId && x.ClientOperationId == operationId));
        Assert.Equal(1, await verifyDb.AuditEvents.CountAsync(x =>
            x.EntityType == "OperationalParty" && x.EntityId == results[0].Id && x.Action == "OperationalPartyCreate"));
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Waybill_create_rolls_back_when_audit_insert_fails()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        TestScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedScopeAsync(seedDb, "P2AUDFAIL", withSequence: false);
        var context = new OperationContext(scope.UserId, scope.CompanyId, scope.BranchId, Guid.NewGuid());
        var createKey = $"audit-fail-{Guid.NewGuid():N}";
        var suffix = Guid.NewGuid().ToString("N");
        var function = $"fail_waybill_audit_{suffix}";
        var trigger = $"trg_fail_waybill_audit_{suffix}";
        await using var admin = CreateP2Db(connection);
        await admin.Database.ExecuteSqlRawAsync($$"""
            CREATE FUNCTION transport_erp.{{function}}() RETURNS trigger LANGUAGE plpgsql AS $body$
            BEGIN
              IF NEW."Action" = 'WaybillDraftCreate' THEN RAISE EXCEPTION 'forced waybill audit failure'; END IF;
              RETURN NEW;
            END $body$;
            CREATE TRIGGER {{trigger}} BEFORE INSERT ON transport_erp.audit_events
              FOR EACH ROW EXECUTE FUNCTION transport_erp.{{function}}();
            """);
        try
        {
            await using var db = CreateP2Db(connection);
            await Assert.ThrowsAnyAsync<Exception>(() => CreateService(db).CreateDraftAsync(context,
                new CreateWaybillDraftRequest(scope.BranchId, DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(),
                    scope.CurrencyId, 1m, "STANDARD", "NORMAL", createKey)));
        }
        finally
        {
            await admin.Database.ExecuteSqlRawAsync($$"""
                DROP TRIGGER IF EXISTS {{trigger}} ON transport_erp.audit_events;
                DROP FUNCTION IF EXISTS transport_erp.{{function}}();
                """);
        }

        await using var verify = CreateP2Db(connection);
        Assert.False(await verify.Set<WaybillEntity>().AnyAsync(x => x.CreateClientOperationId == createKey));
        Assert.False(await verify.AuditEvents.AnyAsync(x => x.Action == "WaybillDraftCreate" && x.CorrelationId == context.CorrelationId));
    }

    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Waybill_create_API_enforces_permission_and_branch_scope()
    {
        var connection = RequireConnection();
        await EnsureMigratedAsync(connection);
        TestScope scope;
        await using (var seedDb = CreateP2Db(connection))
            scope = await SeedScopeAsync(seedDb, "P2HTTP", withSequence: false);

        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            CreateToken(scope.UserId, scope.CompanyId, scope.BranchId, "audit.events.read"));
        var denied = await client.PostAsJsonAsync("/api/v1/waybills/drafts", NewCreateRequest(scope));
        Assert.Equal(HttpStatusCode.Forbidden, denied.StatusCode);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            CreateToken(scope.UserId, scope.CompanyId, scope.BranchId, WaybillPermissionCodes.Create));
        var allowed = await client.PostAsJsonAsync("/api/v1/waybills/drafts", NewCreateRequest(scope));
        Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);
        var created = await allowed.Content.ReadFromJsonAsync<WaybillResponse>();
        Assert.NotNull(created);
        Assert.Equal(scope.CompanyId, created!.CompanyId);
        Assert.Equal(scope.BranchId, created.BranchId);
        Assert.Null(created.WaybillNo);

        var wrongBranch = NewCreateRequest(scope) with
        {
            BranchId = Guid.NewGuid(),
            ClientOperationId = $"http-wrong-{Guid.NewGuid():N}"
        };
        var scoped = await client.PostAsJsonAsync("/api/v1/waybills/drafts", wrongBranch);
        Assert.Equal(HttpStatusCode.Forbidden, scoped.StatusCode);
    }

    private static string RequireConnection()
        => Environment.GetEnvironmentVariable("TRANSPORTERP_TEST_CONNSTR")
            ?? throw new InvalidOperationException("TRANSPORTERP_TEST_CONNSTR is required for P2 PostgreSQL gates.");

    private static async Task EnsureMigratedAsync(string connection)
    {
        await using var db = CreateP2Db(connection);
        await db.Database.MigrateAsync();
    }

    private static CreateWaybillDraftRequest NewCreateRequest(TestScope scope)
        => new(scope.BranchId, DateTimeOffset.UtcNow, Guid.NewGuid(), Guid.NewGuid(), scope.CurrencyId,
            1m, "STANDARD", "NORMAL", $"http-create-{Guid.NewGuid():N}");

    private static WaybillApplicationService CreateService(TransportErpDbContext db)
        => new(
            new ConcurrencySafeWaybillRepository(db),
            new EfOperationalPartyRepository(db),
            new EfNumberReservationService(db),
            new EfWaybillUnitOfWork(db),
            new EfWaybillAuditSink(db, new AuditEventService(db)));

    private static TransportErpDbContext CreateP2Db(string connection)
        => PostgreSqlTestEnvironment.CreateDbContext(connection);

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
            Subject = new ClaimsIdentity(claims),
            Issuer = Issuer,
            Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)) { KeyId = "test-current" }, SecurityAlgorithms.HmacSha256)
        };
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(descriptor));
    }

    private static async Task<TestScope> SeedScopeAsync(TransportErpDbContext db, string suffix, bool withSequence)
    {
        for (var attempt = 0; attempt < 8; attempt++)
        {
            var now = DateTimeOffset.UtcNow;
            var currency = new Currency
            {
                Id = Guid.NewGuid(), Code = Guid.NewGuid().ToString("N")[..3].ToUpperInvariant(),
                NameAr = "عملة اختبار P2", MinorUnit = 2, IsBase = true,
                CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
            };
            var company = new Company
            {
                Id = Guid.NewGuid(), Code = $"P2-{suffix}-{Guid.NewGuid():N}"[..20], LegalNameAr = "شركة اختبار P2",
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
                Id = Guid.NewGuid(), UserName = $"p2-{Guid.NewGuid():N}", NormalizedUserName = $"P2{suffix}{Guid.NewGuid():N}"[..24],
                DisplayName = "مستخدم اختبار P2", PasswordHash = "test-only", SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1, Status = "ACTIVE",
                CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            NumberSequenceEntity? sequence = null;
            if (withSequence)
            {
                sequence = new NumberSequenceEntity
                {
                    Id = Guid.NewGuid(), CompanyId = company.Id, BranchId = branch.Id, DocumentType = "WAYBILL",
                    Prefix = $"WB-{suffix}-", NextValue = 1, ResetPolicy = "NONE", Status = "ACTIVE",
                    CreatedAt = now, UpdatedAt = now, Version = 1
                };
            }

            db.Currencies.Add(currency);
            db.Companies.Add(company);
            db.Branches.Add(branch);
            db.Users.Add(user);
            if (sequence is not null) db.Set<NumberSequenceEntity>().Add(sequence);
            try
            {
                await db.SaveChangesAsync();
                return new TestScope(company.Id, branch.Id, user.Id, currency.Id, sequence?.Id);
            }
            catch (Exception ex) when (IsUniqueViolation(ex) && attempt < 7)
            {
                db.ChangeTracker.Clear();
                await Task.Delay(10 * (attempt + 1));
            }
        }

        throw new InvalidOperationException("Unable to seed a unique P2 test scope after retries.");
    }

    private static bool IsUniqueViolation(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
            if (current is PostgresException { SqlState: "23505" }) return true;
        return false;
    }

    private sealed record TestScope(Guid CompanyId, Guid BranchId, Guid UserId, Guid CurrencyId, Guid? SequenceId);
}
