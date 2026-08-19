using Microsoft.EntityFrameworkCore;
using Npgsql;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class AuditEventPersistenceTests
{
    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Audit_event_append_only_hash_chain_and_company_filter_work()
    {
        var connection = GetConnection();
        if (connection is null) return;

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var (companyA, branchA) = await SeedScopeAsync(db, "A");
        var (companyB, branchB) = await SeedScopeAsync(db, "B");
        var service = new AuditEventService(db);

        await service.AppendAuditEventAsync(new AuditEventDraft(
            "TEST_A", "SUCCESS", "TEST_ENTITY", CompanyId: companyA, BranchId: branchA));
        await service.AppendAuditEventAsync(new AuditEventDraft(
            "TEST_B", "SUCCESS", "TEST_ENTITY", CompanyId: companyB, BranchId: branchB));

        var chain = await service.VerifyHashChainAsync();
        Assert.True(chain.IsValid, chain.FailureReason);

        var companyEvents = await service.GetAuditEventsAsync(new AuditEventQuery(
            CompanyId: companyA, Action: "TEST_A", Take: 10));
        Assert.Single(companyEvents);
        Assert.Equal(companyA, companyEvents[0].CompanyId);
        Assert.DoesNotContain(companyEvents, x => x.CompanyId == companyB);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task PostgreSQL_trigger_blocks_update_and_delete_of_audit_event()
    {
        var connection = GetConnection();
        if (connection is null) return;

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();
        var audit = new AuditEvent
        {
            Id = Guid.NewGuid(),
            OccurredAt = DateTimeOffset.UtcNow,
            Action = "TRIGGER_TEST",
            Outcome = "SUCCESS",
            EntityType = "TEST_ENTITY",
            CorrelationId = Guid.NewGuid(),
            Hash = "trigger-test-hash"
        };
        db.AuditEvents.Add(audit);
        await db.SaveChangesAsync();

        await transaction.CreateSavepointAsync("before_update");
        var updateError = await Assert.ThrowsAsync<PostgresException>(() => db.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE transport_erp.audit_events SET \"Reason\" = {"forbidden"} WHERE \"Id\" = {audit.Id}"));
        Assert.Contains("append-only", updateError.MessageText, StringComparison.OrdinalIgnoreCase);
        await transaction.RollbackToSavepointAsync("before_update");

        await transaction.CreateSavepointAsync("before_delete");
        var deleteError = await Assert.ThrowsAsync<PostgresException>(() => db.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM transport_erp.audit_events WHERE \"Id\" = {audit.Id}"));
        Assert.Contains("append-only", deleteError.MessageText, StringComparison.OrdinalIgnoreCase);
        await transaction.RollbackToSavepointAsync("before_delete");
        await transaction.RollbackAsync();
    }

    private static TransportErpDbContext CreateDb(string connection)
        => new(new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseNpgsql(connection)
            .Options);

    private static string? GetConnection()
        => Environment.GetEnvironmentVariable("TRANSPORTERP_TEST_CONNSTR")
            ?? Environment.GetEnvironmentVariable("TRANSPORTERP_P1_POSTGRES_CONNECTION");

    private static async Task<(Guid CompanyId, Guid BranchId)> SeedScopeAsync(
        TransportErpDbContext db,
        string suffix)
    {
        var currency = new Currency
        {
            Id = Guid.NewGuid(),
            Code = $"T{suffix}{Guid.NewGuid():N}"[..3].ToUpperInvariant(),
            NameAr = "عملة اختبار",
            MinorUnit = 2,
            IsBase = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Code = $"AUD-{suffix}-{Guid.NewGuid():N}"[..18],
            LegalNameAr = "شركة تدقيق اختبار",
            BaseCurrencyId = currency.Id,
            DefaultCalendarId = Guid.NewGuid(),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(),
            CompanyId = company.Id,
            Code = "MAIN",
            NameAr = "الفرع الرئيسي",
            Timezone = "Asia/Aden",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        db.Currencies.Add(currency);
        db.Companies.Add(company);
        db.Branches.Add(branch);
        await db.SaveChangesAsync();
        return (company.Id, branch.Id);
    }
}
