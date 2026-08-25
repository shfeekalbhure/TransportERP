using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using TransportERP.Api.Identity;
using TransportERP.Api.Security;
using TransportERP.Contracts.Identity;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class BootstrapAdminPostgreSqlTests
{
    private const string Password = "Bootstrap-test-password-2026!";

    [Fact]
    public void Bootstrap_password_is_rejected_from_general_configuration_provider()
    {
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["BootstrapAdmin:AdminPassword"] = Password
        }).Build();
        var ex = Assert.Throws<InvalidOperationException>(() => BootstrapAdminOptions.FromConfiguration(configuration));
        Assert.Equal("BOOTSTRAP_PASSWORD_CONFIGURATION_PROVIDER_FORBIDDEN", ex.Message);
        Assert.DoesNotContain(Password, ex.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public void Bootstrap_configuration_requires_only_explicit_bootstrap_values_and_env_secret_not_jwt()
    {
        var previous = Environment.GetEnvironmentVariable("TRANSPORTERP_BOOTSTRAP_ADMIN_PASSWORD");
        try
        {
            Environment.SetEnvironmentVariable("TRANSPORTERP_BOOTSTRAP_ADMIN_PASSWORD", Password);
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["BootstrapAdmin:CurrencyCode"] = "YER", ["BootstrapAdmin:CurrencyNameAr"] = "ريال يمني",
                ["BootstrapAdmin:CurrencyMinorUnit"] = "2", ["BootstrapAdmin:CompanyCode"] = "BOOT",
                ["BootstrapAdmin:CompanyNameAr"] = "شركة التهيئة", ["BootstrapAdmin:DefaultCalendarId"] = Guid.NewGuid().ToString(),
                ["BootstrapAdmin:BranchCode"] = "MAIN", ["BootstrapAdmin:BranchNameAr"] = "الفرع الرئيسي",
                ["BootstrapAdmin:BranchTimezone"] = "Asia/Riyadh", ["BootstrapAdmin:AdminUserName"] = "admin",
                ["BootstrapAdmin:AdminDisplayName"] = "مدير النظام"
            }).Build();
            var parsed = BootstrapAdminOptions.FromConfiguration(configuration);
            Assert.Equal("admin", parsed.AdminUserName);
            Assert.Equal(Password, parsed.AdminPassword);
            Assert.Null(configuration["Auth:SigningKey"]);
        }
        finally { Environment.SetEnvironmentVariable("TRANSPORTERP_BOOTSTRAP_ADMIN_PASSWORD", previous); }
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Fresh_bootstrap_is_one_time_hashed_audited_and_can_login_with_catalog_permissions()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            var options = OptionsFor("OK");
            await using (var db = CreateDb(connection))
            {
                await db.Database.MigrateAsync();
                await CreateBootstrapService(db).ExecuteAsync(options);
            }

            await using var verify = CreateDb(connection);
            var user = await verify.Users.SingleAsync();
            Assert.NotEqual(Password, user.PasswordHash);
            Assert.DoesNotContain(Password, user.PasswordHash, StringComparison.Ordinal);
            var marker = await verify.GlobalSettings.SingleAsync(x => x.Key == BootstrapAdminService.MarkerKey);
            Assert.DoesNotContain(Password, marker.ValueJson, StringComparison.Ordinal);
            var bootstrapAudit = await verify.AuditEvents.SingleAsync(x => x.Action == "BootstrapAdminCreated");
            Assert.DoesNotContain(Password,
                $"{bootstrapAudit.BeforeJson}|{bootstrapAudit.AfterJson}|{bootstrapAudit.Reason}", StringComparison.Ordinal);
            Assert.True(await new EffectivePermissionResolver(verify).HasPermissionAsync(
                user.Id, user.CompanyId!.Value, user.BranchId, "waybill.create"));

            var hasher = new PasswordHasher<User>();
            var sessions = new IdentitySessionService(verify, hasher, new IdentityPasswordSentinel(hasher),
                new TenantScopeResolver(verify, new EffectivePermissionResolver(verify)), new AuditEventService(verify),
                Options.Create(SecurityOptions()));
            var login = await sessions.CreateAsync(new CreateIdentitySessionRequest(
                options.AdminUserName, Password, user.CompanyId, user.BranchId, "bootstrap-login-device"),
                Guid.NewGuid(), "127.0.0.1", default);
            Assert.Equal(user.Id, login.UserId);

            await using var secondDb = CreateDb(connection);
            var second = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                CreateBootstrapService(secondDb).ExecuteAsync(options));
            Assert.Equal("BOOTSTRAP_ALREADY_COMPLETED", second.Message);
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Bootstrap_rolls_back_everything_owned_by_command_when_audit_fails()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = CreateDb(connection);
            await db.Database.MigrateAsync();
            await db.Database.ExecuteSqlRawAsync("DELETE FROM transport_erp.permissions");
            await db.Database.ExecuteSqlRawAsync("""
                CREATE FUNCTION transport_erp.fail_bootstrap_audit() RETURNS trigger LANGUAGE plpgsql AS $body$
                BEGIN
                  IF NEW."Action"='BootstrapAdminCreated' THEN RAISE EXCEPTION 'forced bootstrap audit failure'; END IF;
                  RETURN NEW;
                END $body$;
                CREATE TRIGGER trg_fail_bootstrap_audit BEFORE INSERT ON transport_erp.audit_events
                  FOR EACH ROW EXECUTE FUNCTION transport_erp.fail_bootstrap_audit();
                """);

            await Assert.ThrowsAnyAsync<Exception>(() => CreateBootstrapService(db).ExecuteAsync(OptionsFor("RB")));
            db.ChangeTracker.Clear();
            Assert.False(await db.Users.IgnoreQueryFilters().AnyAsync());
            Assert.False(await db.Currencies.AnyAsync());
            Assert.False(await db.Companies.AnyAsync());
            Assert.False(await db.Branches.AnyAsync());
            Assert.False(await db.Roles.IgnoreQueryFilters().AnyAsync());
            Assert.False(await db.Permissions.IgnoreQueryFilters().AnyAsync());
            Assert.False(await db.UserRoles.AnyAsync());
            Assert.False(await db.RolePermissions.AnyAsync());
            Assert.False(await db.GlobalSettings.AnyAsync(x => x.Key == BootstrapAdminService.MarkerKey));
            Assert.False(await db.AuditEvents.AnyAsync(x => x.Action == "BootstrapAdminCreated"));
            Assert.False(await db.AuditStreamHeads.AnyAsync());
        });
    }

    [Theory]
    [InlineData(false, "BOOTSTRAP_REFERENCE_DRIFT:ADMIN_ROLE_MISSING_GRANT")]
    [InlineData(true, "BOOTSTRAP_REFERENCE_DRIFT:ADMIN_ROLE_EXTRA_GRANT")]
    [Trait("Category", "PostgreSQL")]
    public async Task Existing_admin_role_must_have_exact_catalog_grant_set(bool addExtraGrant, string expectedPrefix)
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = CreateDb(connection);
            await db.Database.MigrateAsync();
            var options = OptionsFor(addExtraGrant ? "EXTRA" : "MISSING");
            var now = DateTimeOffset.UtcNow;
            var currency = Entity(new Currency { Code = options.CurrencyCode, NameAr = options.CurrencyNameAr,
                MinorUnit = options.CurrencyMinorUnit, IsBase = true, Status = "ACTIVE" }, now);
            var company = Entity(new Company { Code = options.CompanyCode, LegalNameAr = options.CompanyNameAr,
                BaseCurrencyId = currency.Id, DefaultCalendarId = options.DefaultCalendarId, Status = "ACTIVE" }, now);
            var branch = Entity(new Branch { CompanyId = company.Id, Code = options.BranchCode,
                NameAr = options.BranchNameAr, BranchType = "MAIN", Timezone = options.BranchTimezone, Status = "ACTIVE" }, now);
            var role = Entity(new Role { Code = "SYSTEM_ADMIN", NameAr = "مدير النظام",
                Description = "دور الإدارة الأولي للنظام", IsSystem = true, CompanyId = company.Id, Status = "ACTIVE" }, now);
            db.AddRange(currency, company, branch, role);
            if (addExtraGrant)
            {
                var extra = Entity(new Permission { Code = $"test.extra.{Guid.NewGuid():N}", NameAr = "إضافية",
                    Resource = "test", Action = "extra", ScopeType = "BRANCH", IsSystem = false, Status = "ACTIVE" }, now);
                db.Permissions.Add(extra);
                db.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = extra.Id,
                    ScopeType = "BRANCH", CompanyId = company.Id, BranchId = branch.Id,
                    CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray() });
            }
            await db.SaveChangesAsync();

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => CreateBootstrapService(db).ExecuteAsync(options));
            Assert.StartsWith(expectedPrefix, ex.Message, StringComparison.Ordinal);
            db.ChangeTracker.Clear();
            Assert.False(await db.Users.IgnoreQueryFilters().AnyAsync());
            Assert.False(await db.GlobalSettings.AnyAsync(x => x.Key == BootstrapAdminService.MarkerKey));
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Concurrent_bootstrap_has_one_winner_and_deterministic_already_completed_loser()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using (var migrate = CreateDb(connection)) await migrate.Database.MigrateAsync();
            var options = OptionsFor("RACE");
            var start = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            async Task<string> RunAsync()
            {
                await using var db = CreateDb(connection);
                await start.Task;
                try { await CreateBootstrapService(db).ExecuteAsync(options); return "SUCCESS"; }
                catch (InvalidOperationException ex) { return ex.Message; }
            }
            var first = RunAsync(); var second = RunAsync(); start.SetResult();
            var results = await Task.WhenAll(first, second);
            Assert.Single(results.Where(x => x == "SUCCESS"));
            Assert.Single(results.Where(x => x == "BOOTSTRAP_ALREADY_COMPLETED"));
            await using var verify = CreateDb(connection);
            Assert.Equal(1, await verify.Users.IgnoreQueryFilters().CountAsync());
            Assert.Equal(1, await verify.AuditEvents.CountAsync(x => x.Action == "BootstrapAdminCreated"));
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Bootstrap_rejects_permission_catalog_scope_drift_before_provisioning()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = CreateDb(connection);
            await db.Database.MigrateAsync();
            var permission = await db.Permissions.SingleAsync(x => x.Code == "waybill.create");
            permission.ScopeType = "PLATFORM";
            await db.SaveChangesAsync();
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                CreateBootstrapService(db).ExecuteAsync(OptionsFor("DRIFT")));
            Assert.Equal("PERMISSION_CATALOG_DRIFT:waybill.create", ex.Message);
            db.ChangeTracker.Clear();
            Assert.False(await db.Users.IgnoreQueryFilters().AnyAsync());
            Assert.False(await db.GlobalSettings.AnyAsync(x => x.Key == BootstrapAdminService.MarkerKey));
        });
    }

    private static BootstrapAdminService CreateBootstrapService(TransportErpDbContext db)
        => new(db, new PasswordHasher<User>(), new AuditEventService(db));

    private static BootstrapAdminOptions OptionsFor(string suffix) => new(
        "YER", "ريال يمني", 2, $"BOOT-{suffix}", "شركة التهيئة", Guid.NewGuid(),
        "MAIN", "الفرع الرئيسي", "Asia/Riyadh", $"admin-{suffix.ToLowerInvariant()}", "مدير النظام", Password);

    private static TransportSecurityOptions SecurityOptions() => new()
    {
        Mode = TransportAuthMode.LocalSessions, Issuer = "bootstrap-test", Audience = "bootstrap-test",
        SigningKeyId = "bootstrap-current", SigningKey = "bootstrap-test-signing-key-minimum-32-characters"
    };

    private static TransportErpDbContext CreateDb(string connection)
        => PostgreSqlTestEnvironment.CreateDbContext(connection);

    private static T Entity<T>(T entity, DateTimeOffset now) where T : P1Entity
    {
        entity.Id = Guid.NewGuid(); entity.CreatedAt = now; entity.UpdatedAt = now;
        entity.RowVersion = Guid.NewGuid().ToByteArray(); return entity;
    }

    private static async Task WithFreshDatabaseAsync(Func<string, Task> test)
    {
        var baseConnection = PostgreSqlTestEnvironment.RequireConnection();
        var database = $"transporterp_bootstrap_{Guid.NewGuid():N}";
        var adminBuilder = new NpgsqlConnectionStringBuilder(baseConnection) { Database = "postgres", Pooling = false };
        await using (var admin = new NpgsqlConnection(adminBuilder.ConnectionString))
        {
            await admin.OpenAsync();
            await using var create = new NpgsqlCommand($"CREATE DATABASE \"{database}\"", admin);
            await create.ExecuteNonQueryAsync();
        }
        var testBuilder = new NpgsqlConnectionStringBuilder(baseConnection) { Database = database, Pooling = false };
        try { await test(testBuilder.ConnectionString); }
        finally
        {
            NpgsqlConnection.ClearAllPools();
            await using var admin = new NpgsqlConnection(adminBuilder.ConnectionString);
            await admin.OpenAsync();
            await using var drop = new NpgsqlCommand($"DROP DATABASE IF EXISTS \"{database}\" WITH (FORCE)", admin);
            await drop.ExecuteNonQueryAsync();
        }
    }
}
