using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class ApiAuthenticationAndAuditTests
{
    private const string Issuer = "TransportERP.Test.Identity";
    private const string Audience = "TransportERP.Test.Api";
    private const string SigningKey = "transport-erp-test-signing-key-2026-minimum-32";

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Sync_batch_requires_a_valid_bearer_token()
    {
        var connection = GetConnection();
        if (connection is null) return;

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/sync/operations:batch",
            new
            {
                DeviceId = "untrusted-device",
                ProtocolVersion = "P1",
                Operations = Array.Empty<object>()
            });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Sync_batch_accepts_a_valid_token_and_enforces_claim_scope()
    {
        var connection = GetConnection();
        if (connection is null) return;

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "HTTPB");
        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(
            scope.UserId, scope.CompanyId, scope.BranchId, scope.DeviceId,
            "sync.operations.execute"));

        const string payload = "{\"http\":true}";
        var response = await client.PostAsJsonAsync("/api/v1/sync/operations:batch", new
        {
            DeviceId = scope.DeviceId,
            ProtocolVersion = "P1",
            Operations = new[]
            {
                new
                {
                    OperationType = "UPDATE",
                    EntityType = "TestEntity",
                    EntityId = Guid.NewGuid().ToString(),
                    ClientOperationId = $"http-{Guid.NewGuid():N}",
                    PayloadJson = payload,
                    PayloadHash = Sha256(payload),
                    ClientOccurredAt = DateTimeOffset.UtcNow,
                    BaseVersion = 1L
                }
            }
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<SyncBatchResponse>();
        Assert.NotNull(body);
        Assert.Single(body!.Results);
        Assert.Equal("QUEUED", body.Results[0].Status);
        Assert.Equal(scope.DeviceId, await db.SyncOperations.Select(x => x.DeviceId).SingleAsync(x => x == scope.DeviceId));
    }

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Audit_read_requires_permission_and_cannot_escape_company_scope()
    {
        var connection = GetConnection();
        if (connection is null) return;

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "HTTPA");
        var other = await SeedScopeAsync(db, "HTTPZ");
        var audit = new AuditEventService(db);
        await audit.AppendAuditEventAsync(new AuditEventDraft(
            "TestAudit", "SUCCESS", "TestEntity", Guid.NewGuid(), scope.UserId,
            scope.CompanyId, scope.BranchId, Guid.NewGuid(), scope.DeviceId));
        await audit.AppendAuditEventAsync(new AuditEventDraft(
            "OtherAudit", "SUCCESS", "TestEntity", Guid.NewGuid(), other.UserId,
            other.CompanyId, other.BranchId, Guid.NewGuid(), other.DeviceId));

        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(
            scope.UserId, scope.CompanyId, scope.BranchId, scope.DeviceId,
            "audit.events.read"));

        var denied = await client.GetAsync($"/api/v1/audit/events?companyId={other.CompanyId}");
        Assert.Equal(HttpStatusCode.Forbidden, denied.StatusCode);

        var allowed = await client.GetAsync($"/api/v1/audit/events?companyId={scope.CompanyId}&take=100");
        Assert.Equal(HttpStatusCode.OK, allowed.StatusCode);
        var body = await allowed.Content.ReadFromJsonAsync<PagedAuditEventResponse>();
        Assert.NotNull(body);
        Assert.NotEmpty(body!.Items);
        Assert.All(body.Items, item => Assert.Equal(scope.CompanyId, item.CompanyId));
        Assert.Contains(body.Items, item => item.Action == "TestAudit");
    }

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Invalid_issuer_token_is_rejected_by_jwt_bearer_provider()
    {
        var connection = GetConnection();
        if (connection is null) return;

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var scope = await SeedScopeAsync(db, "HTTPX");
        using var factory = CreateFactory(connection);
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CreateToken(
            scope.UserId, scope.CompanyId, scope.BranchId, scope.DeviceId,
            "sync.operations.execute", issuer: "untrusted-issuer"));

        var response = await client.GetAsync("/api/v1/audit/events");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private static WebApplicationFactory<Program> CreateFactory(string connection)
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", connection);
            builder.UseSetting("Auth:Issuer", Issuer);
            builder.UseSetting("Auth:Audience", Audience);
            builder.UseSetting("Auth:SigningKey", SigningKey);
        });

    private static string CreateToken(Guid userId, Guid companyId, Guid branchId, string deviceId,
        string permission, string issuer = Issuer)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("company_id", companyId.ToString()),
            new Claim("branch_id", branchId.ToString()),
            new Claim("device_id", deviceId),
            new Claim("device_registered", "true"),
            new Claim("permission", permission)
        };
        var identity = new ClaimsIdentity(claims);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Issuer = issuer,
            Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)),
                SecurityAlgorithms.HmacSha256)
        };
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(descriptor));
    }

    private static string? GetConnection()
        => Environment.GetEnvironmentVariable("TRANSPORTERP_TEST_CONNSTR")
            ?? Environment.GetEnvironmentVariable("TRANSPORTERP_P1_POSTGRES_CONNECTION");

    private static TransportErpDbContext CreateDb(string connection)
        => new(new DbContextOptionsBuilder<TransportErpDbContext>().UseNpgsql(connection).Options);

    private static string Sha256(string payload)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload))).ToLowerInvariant();

        private static async Task<TestScope> SeedScopeAsync(TransportErpDbContext db, string suffix)
    {
        for (var attempt = 0; attempt < 8; attempt++)
        {
            var now = DateTimeOffset.UtcNow;
            var currency = new Currency
            {
                Id = Guid.NewGuid(), Code = Guid.NewGuid().ToString("N")[..3].ToUpperInvariant(),
                NameAr = "عملة اختبار HTTP", MinorUnit = 2, IsBase = true, CreatedAt = now, UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Code = $"H-{suffix}-{Guid.NewGuid():N}"[..18], LegalNameAr = "شركة اختبار HTTP",
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
                Id = Guid.NewGuid(), UserName = $"http-{Guid.NewGuid():N}", NormalizedUserName = $"HTTP{suffix}",
                DisplayName = "مستخدم اختبار HTTP", PasswordHash = "test-only", Status = "ACTIVE",
                CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            db.Currencies.Add(currency);
            db.Companies.Add(company);
            db.Branches.Add(branch);
            db.Users.Add(user);
            try
            {
                await db.SaveChangesAsync();
                return new TestScope(company.Id, branch.Id, user.Id, $"http-device-{suffix}-{Guid.NewGuid():N}");
            }
            catch (Exception ex) when (IsUniqueViolation(ex) && attempt < 7)
            {
                db.ChangeTracker.Clear();
                await Task.Delay(10 * (attempt + 1));
            }
        }

        throw new InvalidOperationException("Unable to seed a unique HTTP test scope after retries.");
    }

    private static bool IsUniqueViolation(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is Npgsql.PostgresException { SqlState: "23505" })
                return true;
        }

        return false;
    }

    private sealed record TestScope(Guid CompanyId, Guid BranchId, Guid UserId, string DeviceId);
}
