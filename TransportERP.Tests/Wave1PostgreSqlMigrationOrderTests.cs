using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class Wave1PostgreSqlMigrationOrderTests
{
    [Fact]
    [Trait("Category", "P2PostgreSQL")]
    public async Task Governed_wave1_upgrade_order_executes_on_real_postgresql_and_preserves_unknown_legacy_data()
    {
        var connection = Environment.GetEnvironmentVariable("TRANSPORTERP_TEST_CONNSTR");
        if (string.IsNullOrWhiteSpace(connection)) return;

        await using var main = CreateMain(connection);
        await main.Database.OpenConnectionAsync();
        await main.Database.ExecuteSqlRawAsync("DROP SCHEMA IF EXISTS transport_erp CASCADE; CREATE SCHEMA transport_erp;");
        await main.Database.CloseConnectionAsync();

        // Production dependency order: base/P2 schema -> geography -> reference cleanup -> authority promotions.
        await main.Database.MigrateAsync();

        await using (var geo = CreateGeo(connection))
            await geo.Database.MigrateAsync();

        await using (var reference = CreateReference(connection))
            await reference.Database.MigrateAsync();

        var legacyCountryId = Guid.NewGuid();
        var legacySequenceId = Guid.NewGuid();
        await using (var seed = CreateMain(connection))
        {
            await seed.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO transport_erp.countries
                    (\"Id\",\"Code\",\"ArabicName\",\"EnglishName\",\"IsActive\",\"Version\",\"NationalityName\")
                VALUES
                    ({legacyCountryId}, 'LEGACY', 'دولة تاريخية', NULL, TRUE, 1, NULL);");

            seed.Set<NumberSequenceEntity>().Add(new NumberSequenceEntity
            {
                Id = legacySequenceId,
                CompanyId = await seed.Companies.Select(x => x.Id).FirstAsync(),
                BranchId = null,
                DocumentType = "LEGACYDOC",
                Prefix = "L-",
                NextValue = 7,
                ResetPolicy = "NONE",
                Status = "ACTIVE",
                Version = 1,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });
            await seed.SaveChangesAsync();
        }

        await using (var country = CreateCountry(connection))
            await country.Database.MigrateAsync();

        await using (var numbering = CreateNumbering(connection))
            await numbering.Database.MigrateAsync();

        await using (var accounting = CreateAccounting(connection))
            await accounting.Database.MigrateAsync();

        await using var verify = CreateMain(connection);
        await verify.Database.OpenConnectionAsync();
        var conn = verify.Database.GetDbConnection();

        var country = await QuerySingleAsync(conn,
            "SELECT COALESCE(\"ISO2\",'<NULL>') || '|' || COALESCE(\"ISO3\",'<NULL>') || '|' || COALESCE(\"DialingCode\",'<NULL>') FROM transport_erp.countries WHERE \"Id\" = @id",
            legacyCountryId);
        Assert.Equal("<NULL>|<NULL>|<NULL>", country);

        var metadata = await QuerySingleAsync(conn,
            "SELECT \"Code\" || '|' || COALESCE(\"ArabicName\",'<NULL>') FROM transport_erp.number_sequence_metadata WHERE \"SequenceId\" = @id",
            legacySequenceId);
        Assert.Equal("LEGACYDOC|<NULL>", metadata);

        Assert.True(await TableExistsAsync(conn, "approval_requests"));
        Assert.True(await TableExistsAsync(conn, "approval_actions"));
        Assert.True(await TableExistsAsync(conn, "account_groups"));
        Assert.True(await TableExistsAsync(conn, "account_types"));
        Assert.True(await TableExistsAsync(conn, "open_items"));
        Assert.True(await TableExistsAsync(conn, "payment_allocations"));
        Assert.True(await TableExistsAsync(conn, "cash_flow_account_mappings"));
        Assert.True(await TableExistsAsync(conn, "cash_flow_movement_overrides"));
        Assert.False(await TableExistsAsync(conn, "account_classifications"));
        Assert.False(await TableExistsAsync(conn, "accounting_open_items"));

        Assert.True(await MigrationRecordedAsync(conn, "__EFMigrationsHistory_Wave1Geo", "20260822024000_Wave1Geography"));
        Assert.True(await MigrationRecordedAsync(conn, "__EFMigrationsHistory_Wave1Reference", "20260822172500_Wave1HeldArtifactsCleanup"));
        Assert.True(await MigrationRecordedAsync(conn, "__EFMigrationsHistory_Wave1CountryAuthority", "20260823001000_Wave1CountryPhysicalPromotion"));
        Assert.True(await MigrationRecordedAsync(conn, "__EFMigrationsHistory_Wave1NumberingAuthority", "20260823002000_Wave1NumberingMetadata"));
        Assert.True(await MigrationRecordedAsync(conn, "__EFMigrationsHistory_Wave1NumberingAuthority", "20260823002100_Wave1NumberingApprovalBinding"));
        Assert.True(await MigrationRecordedAsync(conn, "__EFMigrationsHistory_Wave1AccountingAuthority", "20260823003000_Wave1AccountingAuthority"));

        await verify.Database.CloseConnectionAsync();
    }

    private static TransportErpDbContext CreateMain(string connection)
        => new(new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseNpgsql(connection, x => x.MigrationsHistoryTable("__EFMigrationsHistory", "transport_erp"))
            .ReplaceService<IModelCustomizer, TransportErpP2CombinedModelCustomizer>()
            .Options);

    private static Wave1GeoDbContext CreateGeo(string connection)
        => new(new DbContextOptionsBuilder<Wave1GeoDbContext>()
            .UseNpgsql(connection, x => x.MigrationsHistoryTable("__EFMigrationsHistory_Wave1Geo", "transport_erp"))
            .Options);

    private static Wave1ReferenceDbContext CreateReference(string connection)
        => new(new DbContextOptionsBuilder<Wave1ReferenceDbContext>()
            .UseNpgsql(connection, x => x.MigrationsHistoryTable("__EFMigrationsHistory_Wave1Reference", "transport_erp"))
            .ReplaceService<IModelCustomizer, Wave1ReferenceRuntimeModelCustomizer>()
            .Options);

    private static Wave1CountryAuthorityDbContext CreateCountry(string connection)
        => new(new DbContextOptionsBuilder<Wave1CountryAuthorityDbContext>()
            .UseNpgsql(connection, x => x.MigrationsHistoryTable("__EFMigrationsHistory_Wave1CountryAuthority", "transport_erp"))
            .Options);

    private static Wave1NumberingAuthorityDbContext CreateNumbering(string connection)
        => new(new DbContextOptionsBuilder<Wave1NumberingAuthorityDbContext>()
            .UseNpgsql(connection, x => x.MigrationsHistoryTable("__EFMigrationsHistory_Wave1NumberingAuthority", "transport_erp"))
            .Options);

    private static Wave1AccountingAuthorityDbContext CreateAccounting(string connection)
        => new(new DbContextOptionsBuilder<Wave1AccountingAuthorityDbContext>()
            .UseNpgsql(connection, x => x.MigrationsHistoryTable("__EFMigrationsHistory_Wave1AccountingAuthority", "transport_erp"))
            .ReplaceService<IModelCustomizer, Wave1AccountingAuthorityModelCustomizer>()
            .Options);

    private static async Task<string?> QuerySingleAsync(DbConnection connection, string sql, Guid id)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = sql;
        var p = cmd.CreateParameter(); p.ParameterName = "id"; p.Value = id; cmd.Parameters.Add(p);
        return (await cmd.ExecuteScalarAsync())?.ToString();
    }

    private static async Task<bool> TableExistsAsync(DbConnection connection, string table)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT to_regclass('transport_erp.' || @table) IS NOT NULL";
        var p = cmd.CreateParameter(); p.ParameterName = "table"; p.Value = table; cmd.Parameters.Add(p);
        return (bool)(await cmd.ExecuteScalarAsync() ?? false);
    }

    private static async Task<bool> MigrationRecordedAsync(DbConnection connection, string historyTable, string migrationId)
    {
        await using var cmd = connection.CreateCommand();
        cmd.CommandText = $"SELECT EXISTS (SELECT 1 FROM transport_erp.\"{historyTable}\" WHERE \"MigrationId\" = @id)";
        var p = cmd.CreateParameter(); p.ParameterName = "id"; p.Value = migrationId; cmd.Parameters.Add(p);
        return (bool)(await cmd.ExecuteScalarAsync() ?? false);
    }
}
