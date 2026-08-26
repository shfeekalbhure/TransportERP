using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Stage4SyncConflictPermissionMigrationPostgreSqlTests
{
    private const string PreviousMigration = "20260826090000_P1Stage4SyncRetentionRedaction";
    private const string PermissionMigration = "20260826095000_P1SyncConflictResolvePermission";
    private static readonly Guid PermissionId = Guid.Parse("d1000000-0000-4000-8000-000000000004");

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Permission_migration_roundtrips_up_down_up_with_exact_catalog_shape()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(PreviousMigration);
            Assert.Equal(0, await PermissionCountAsync(db));

            await migrator.MigrateAsync(PermissionMigration);
            await AssertExactPermissionAsync(db);
            await SystemPermissionCatalog.EnsureAsync(db, allowCreate: false);

            await migrator.MigrateAsync(PreviousMigration);
            Assert.Equal(0, await PermissionCountAsync(db));

            await migrator.MigrateAsync(PermissionMigration);
            await AssertExactPermissionAsync(db);
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Down_fails_closed_while_role_or_user_reference_exists()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(PermissionMigration);
            var now = DateTimeOffset.UtcNow;
            var userName = $"conflict-permission-{Guid.NewGuid():N}";
            var user = new User
            {
                Id = Guid.NewGuid(), UserName = userName, NormalizedUserName = userName.ToUpperInvariant(),
                DisplayName = "Conflict permission test", PasswordHash = "test-only", Status = "ACTIVE",
                SecurityStamp = Guid.NewGuid().ToString("N"), AuthVersion = 1,
                CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
            };
            db.Users.Add(user);
            db.UserPermissionOverrides.Add(new UserPermissionOverride
            {
                UserId = user.Id, PermissionId = PermissionId, IsAllowed = true,
                Reason = "migration down guard", CreatedAt = now, UpdatedAt = now,
                RowVersion = RandomNumberGenerator.GetBytes(16)
            });
            await db.SaveChangesAsync();

            var failure = await Assert.ThrowsAnyAsync<Exception>(() =>
                migrator.MigrateAsync(PreviousMigration));
            Assert.Contains("down blocked: permission references exist",
                failure.GetBaseException().Message, StringComparison.Ordinal);
            await AssertExactPermissionAsync(db);

            db.UserPermissionOverrides.Remove(await db.UserPermissionOverrides.SingleAsync(x =>
                x.UserId == user.Id && x.PermissionId == PermissionId));
            await db.SaveChangesAsync();
            await migrator.MigrateAsync(PreviousMigration);
            Assert.Equal(0, await PermissionCountAsync(db));
        });
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Up_preflight_rejects_code_or_id_collision_without_replacing_existing_data()
    {
        await WithFreshDatabaseAsync(async connection =>
        {
            await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
            var migrator = db.GetService<IMigrator>();
            await migrator.MigrateAsync(PreviousMigration);
            var collisionId = Guid.NewGuid();
            await db.Database.ExecuteSqlInterpolatedAsync($$"""
                INSERT INTO transport_erp.permissions
                  ("Id","Code","NameAr","Resource","Action","ScopeType","IsSystem","Status",
                   "CreatedAt","UpdatedAt","RowVersion","DeletedAt")
                VALUES ({{collisionId}},'sync.conflicts.resolve','drift','wrong','wrong','BRANCH',false,
                        'ACTIVE',clock_timestamp(),clock_timestamp(),decode(md5(random()::text),'hex'),NULL)
                """);

            var failure = await Assert.ThrowsAnyAsync<Exception>(() => migrator.MigrateAsync(PermissionMigration));
            Assert.Contains("permission identity already exists",
                failure.GetBaseException().Message, StringComparison.Ordinal);
            Assert.Equal(collisionId, await db.Permissions.IgnoreQueryFilters()
                .Where(x => x.Code == SyncConflictPermissionCodes.Resolve).Select(x => x.Id).SingleAsync());
            Assert.False(await db.Permissions.IgnoreQueryFilters().AnyAsync(x => x.Id == PermissionId));
        });
    }

    private static Task<int> PermissionCountAsync(TransportErpDbContext db) =>
        db.Permissions.IgnoreQueryFilters().CountAsync(x =>
            x.Id == PermissionId || x.Code == SyncConflictPermissionCodes.Resolve);

    private static async Task AssertExactPermissionAsync(TransportErpDbContext db)
    {
        var permission = await db.Permissions.IgnoreQueryFilters().SingleAsync(x => x.Id == PermissionId);
        Assert.Equal(SyncConflictPermissionCodes.Resolve, permission.Code);
        Assert.Equal("حل تعارضات المزامنة", permission.NameAr);
        Assert.Equal("sync.conflicts", permission.Resource);
        Assert.Equal("resolve", permission.Action);
        Assert.Equal("BRANCH", permission.ScopeType);
        Assert.True(permission.IsSystem);
        Assert.Equal("ACTIVE", permission.Status);
        Assert.Null(permission.DeletedAt);
    }

    private static async Task WithFreshDatabaseAsync(Func<string, Task> test)
    {
        var baseConnection = PostgreSqlTestEnvironment.RequireConnection();
        var database = $"transporterp_conflict_permission_{Guid.NewGuid():N}";
        var adminBuilder = new NpgsqlConnectionStringBuilder(baseConnection)
        {
            Database = "postgres", Pooling = false
        };
        await using (var admin = new NpgsqlConnection(adminBuilder.ConnectionString))
        {
            await admin.OpenAsync();
            await using var create = new NpgsqlCommand($"CREATE DATABASE \"{database}\"", admin);
            await create.ExecuteNonQueryAsync();
        }
        var testBuilder = new NpgsqlConnectionStringBuilder(baseConnection)
        {
            Database = database, Pooling = false
        };
        try
        {
            await test(testBuilder.ConnectionString);
        }
        finally
        {
            NpgsqlConnection.ClearAllPools();
            await using var admin = new NpgsqlConnection(adminBuilder.ConnectionString);
            await admin.OpenAsync();
            await using var drop = new NpgsqlCommand(
                $"DROP DATABASE IF EXISTS \"{database}\" WITH (FORCE)", admin);
            await drop.ExecuteNonQueryAsync();
        }
    }
}
