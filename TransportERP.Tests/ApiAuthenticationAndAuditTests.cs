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
using TransportERP.Contracts.Identity;
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
        var connection = PostgreSqlTestEnvironment.RequireConnection();

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
    public async Task Sync_batch_fails_closed_until_a_device_trust_provider_is_installed()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

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

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Empty(await db.SyncOperations.Where(x => x.DeviceId == scope.DeviceId).ToListAsync());
    }

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Audit_read_requires_permission_and_cannot_escape_company_scope()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

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
        var connection = PostgreSqlTestEnvironment.RequireConnection();

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

    [Fact]
    [Trait("Category", "HTTP")]
    public async Task Login_rate_limit_is_generic_and_supplies_retry_after_without_partition_data()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        using var factory = CreateFactory(connection, loginRateLimit: 1);
        using var client = factory.CreateClient();

        var first = await client.PostAsJsonAsync("/api/v1/auth/sessions",
            new CreateIdentitySessionRequest("missing-a", "wrong-password", null, null, "device-a"));
        Assert.Equal(HttpStatusCode.Unauthorized, first.StatusCode);

        var second = await client.PostAsJsonAsync("/api/v1/auth/sessions",
            new CreateIdentitySessionRequest("missing-b", "wrong-password", null, null, "device-b"));
        Assert.Equal((HttpStatusCode)429, second.StatusCode);
        Assert.True(second.Headers.TryGetValues("Retry-After", out var values));
        Assert.True(int.TryParse(values.Single(), out var seconds) && seconds > 0);
        var body = await second.Content.ReadAsStringAsync();
        Assert.Contains("RATE_LIMITED", body, StringComparison.Ordinal);
        Assert.DoesNotContain("missing", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("device", body, StringComparison.OrdinalIgnoreCase);
    }

    private static WebApplicationFactory<Program> CreateFactory(string connection, int? loginRateLimit = null)
        => new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:TransportErp", connection);
            builder.UseSetting("Auth:Issuer", Issuer);
            builder.UseSetting("Auth:Audience", Audience);
            builder.UseSetting("Auth:SigningKey", SigningKey);
            builder.UseSetting("Auth:SigningKeyId", "test-current");
            if (loginRateLimit.HasValue)
                builder.UseSetting("Auth:LoginRateLimitPermitCount", loginRateLimit.Value.ToString());
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
            new Claim("sid", userId.ToString()),
            new Claim("security_stamp", userId.ToString("N")),
            new Claim("auth_version", "1")
        };
        var identity = new ClaimsIdentity(claims);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Issuer = issuer,
            Audience = Audience,
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)) { KeyId = "test-current" },
                SecurityAlgorithms.HmacSha256)
        };
        return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityTokenHandler().CreateToken(descriptor));
    }

    private static TransportErpDbContext CreateDb(string connection)
        => PostgreSqlTestEnvironment.CreateDbContext(connection);

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
            var deviceId = $"http-device-{suffix}-{Guid.NewGuid():N}";
            var user = new User
            {
                Id = Guid.NewGuid(), UserName = $"http-{Guid.NewGuid():N}", NormalizedUserName = $"HTTP{suffix}",
                DisplayName = "مستخدم اختبار HTTP", PasswordHash = "test-only", SecurityStamp = Guid.Empty.ToString("N"), AuthVersion = 1, Status = "ACTIVE",
                CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            db.Currencies.Add(currency);
            db.Companies.Add(company);
            db.Branches.Add(branch);
            db.Users.Add(user);
            user.SecurityStamp = user.Id.ToString("N");
            db.AuthSessions.Add(new AuthSession
            {
                Id = user.Id, UserId = user.Id, CompanyId = company.Id, BranchId = branch.Id, DeviceId = deviceId,
                Mode = "LOCAL", SecurityStampAtIssue = user.SecurityStamp, AuthVersionAtIssue = user.AuthVersion,
                RefreshTokenHash = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(user.Id.ToByteArray())).ToLowerInvariant(),
                RefreshTokenFamilyId = Guid.NewGuid(), IssuedAt = now, AccessTokenExpiresAt = now.AddMinutes(10),
                RefreshTokenExpiresAt = now.AddDays(1), CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
            });
            var permissionCodes = new[] { "sync.operations.execute", "audit.events.read" };
            var role = new Role { Id = Guid.NewGuid(), Code = $"HTTP-{suffix}-{Guid.NewGuid():N}", NameAr = "دور اختبار",
                CompanyId = company.Id, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray() };
            db.Roles.Add(role);
            db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = role.Id, CompanyId = company.Id, BranchId = branch.Id,
                CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray() });
            foreach (var code in permissionCodes)
            {
                var permissionEntity = await db.Permissions.SingleOrDefaultAsync(x => x.Code == code) ?? new Permission
                {
                    Id = Guid.NewGuid(), Code = code, NameAr = code, Resource = code.Split('.')[0], Action = code.Split('.')[^1],
                    ScopeType = "BRANCH", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
                };
                if (db.Entry(permissionEntity).State == EntityState.Detached) db.Permissions.Add(permissionEntity);
                db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = permissionEntity.Id,
                    ScopeType = "BRANCH", CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
                    RowVersion = Guid.NewGuid().ToByteArray() });
            }
            try
            {
                await db.SaveChangesAsync();
                return new TestScope(company.Id, branch.Id, user.Id, deviceId);
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
