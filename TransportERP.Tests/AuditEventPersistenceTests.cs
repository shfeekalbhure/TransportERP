using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class AuditEventPersistenceTests
{
    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Audit_event_append_only_hash_chain_and_company_filter_work()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var (companyA, branchA) = await SeedScopeAsync(db, "A");
        var (companyB, branchB) = await SeedScopeAsync(db, "B");
        var service = new AuditEventService(db);

        await service.AppendAuditEventAsync(new AuditEventDraft(
            "TEST_A", "SUCCESS", "TEST_ENTITY", CompanyId: companyA, BranchId: branchA));
        await service.AppendAuditEventAsync(new AuditEventDraft(
            "TEST_B", "SUCCESS", "TEST_ENTITY", CompanyId: companyB, BranchId: branchB));

        var chainA = await service.VerifyHashChainAsync(companyA, branchA);
        var chainB = await service.VerifyHashChainAsync(companyB, branchB);
        Assert.True(chainA.IsValid, chainA.FailureReason);
        Assert.True(chainB.IsValid, chainB.FailureReason);

        var companyEvents = await service.GetAuditEventsAsync(new AuditEventQuery(
            CompanyId: companyA, Action: "TEST_A", Take: 10));
        Assert.Single(companyEvents);
        Assert.Equal(companyA, companyEvents[0].CompanyId);
        Assert.DoesNotContain(companyEvents, x => x.CompanyId == companyB);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Canonical_hash_chain_isolated_by_company_branch_and_device_stream()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var (company, branch) = await SeedScopeAsync(db, "S");
        var service = new AuditEventService(db);
        var deviceA = "audit-device-a";
        var deviceB = "audit-device-b";

        await service.AppendAuditEventAsync(new AuditEventDraft("STREAM_A1", "SUCCESS", "TEST_ENTITY",
            CompanyId: company, BranchId: branch, DeviceId: deviceA));
        await service.AppendAuditEventAsync(new AuditEventDraft("STREAM_B1", "SUCCESS", "TEST_ENTITY",
            CompanyId: company, BranchId: branch, DeviceId: deviceB));
        await service.AppendAuditEventAsync(new AuditEventDraft("STREAM_A2", "SUCCESS", "TEST_ENTITY",
            CompanyId: company, BranchId: branch, DeviceId: deviceA));

        var streamA = await service.VerifyHashChainAsync(company, branch, deviceA);
        var streamB = await service.VerifyHashChainAsync(company, branch, deviceB);
        var partialScope = await service.VerifyHashChainAsync(company, branch);

        Assert.True(streamA.IsValid, streamA.FailureReason);
        Assert.Equal(2, streamA.EventCount);
        Assert.Equal(AuditEventService.GetStreamKey(company, branch, deviceA), streamA.StreamKey);
        Assert.True(streamB.IsValid, streamB.FailureReason);
        Assert.Equal(1, streamB.EventCount);
        Assert.Equal(AuditEventService.GetStreamKey(company, branch, deviceB), streamB.StreamKey);
        Assert.True(partialScope.IsValid, partialScope.FailureReason);
        Assert.Equal(3, partialScope.EventCount);
        Assert.Null(partialScope.StreamKey);
        Assert.Equal(2, await db.AuditStreamHeads.CountAsync(x =>
            x.StreamKey == AuditEventService.GetStreamKey(company, branch, deviceA) ||
            x.StreamKey == AuditEventService.GetStreamKey(company, branch, deviceB)));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Concurrent_appends_to_one_stream_are_serializable_and_verify()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var seedDb = CreateDb(connection);
        await seedDb.Database.MigrateAsync();
        var (company, branch) = await SeedScopeAsync(seedDb, "C");
        var device = $"audit-concurrent-{Guid.NewGuid():N}";

        var tasks = Enumerable.Range(0, 4).Select(async index =>
        {
            await using var db = CreateDb(connection);
            var service = new AuditEventService(db);
            return await service.AppendAuditEventAsync(new AuditEventDraft(
                $"CONCURRENT_{index}", "SUCCESS", "TEST_ENTITY", CompanyId: company,
                BranchId: branch, DeviceId: device, CorrelationId: Guid.NewGuid()));
        });

        var appended = await Task.WhenAll(tasks);
        Assert.Equal(4, appended.Length);

        await using var verifyDb = CreateDb(connection);
        var result = await new AuditEventService(verifyDb).VerifyHashChainAsync(company, branch, device);
        Assert.True(result.IsValid, result.FailureReason);
        Assert.Equal(4, result.EventCount);
        var latest = await verifyDb.AuditEvents.AsNoTracking()
            .Where(x => x.CompanyId == company && x.BranchId == branch && x.DeviceId == device)
            .OrderByDescending(x => x.SequenceNo).FirstAsync();
        var head = await verifyDb.AuditStreamHeads.AsNoTracking()
            .SingleAsync(x => x.StreamKey == AuditEventService.GetStreamKey(company, branch, device));
        Assert.Equal(latest.Hash, head.LastHash);
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task Caller_rollback_reverts_both_audit_event_and_stream_head()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        Guid company;
        Guid branch;
        AuditEvent first;
        const string device = "audit-head-rollback";
        await using (var db = CreateDb(connection))
        {
            await db.Database.MigrateAsync();
            (company, branch) = await SeedScopeAsync(db, "HEADRB");
            first = await new AuditEventService(db).AppendAuditEventAsync(new AuditEventDraft(
                "HEAD_BASE", "SUCCESS", "TEST_ENTITY", CompanyId: company, BranchId: branch, DeviceId: device));

            await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            await new AuditEventService(db).AppendAuditEventAsync(new AuditEventDraft(
                "HEAD_ROLLBACK", "SUCCESS", "TEST_ENTITY", CompanyId: company, BranchId: branch, DeviceId: device));
            await transaction.RollbackAsync();
        }

        await using var verifyDb = CreateDb(connection);
        var streamKey = AuditEventService.GetStreamKey(company, branch, device);
        var head = await verifyDb.AuditStreamHeads.AsNoTracking().SingleAsync(x => x.StreamKey == streamKey);
        Assert.Equal(first.Hash, head.LastHash);
        Assert.Equal(1, await verifyDb.AuditEvents.CountAsync(x =>
            x.CompanyId == company && x.BranchId == branch && x.DeviceId == device));
        Assert.False(await verifyDb.AuditEvents.AnyAsync(x =>
            x.CompanyId == company && x.BranchId == branch && x.DeviceId == device && x.Action == "HEAD_ROLLBACK"));
    }

    [Fact]
    [Trait("Category", "PostgreSQL")]
    public async Task PostgreSQL_trigger_blocks_update_and_delete_of_audit_event()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();

        await using var db = CreateDb(connection);
        await db.Database.MigrateAsync();
        var (company, branch) = await SeedScopeAsync(db, "T");
        var audit = await new AuditEventService(db).AppendAuditEventAsync(new AuditEventDraft(
            "TRIGGER_TEST", "SUCCESS", "TEST_ENTITY", CompanyId: company, BranchId: branch,
            DeviceId: "trigger-device"));
        await using var transaction = await db.Database.BeginTransactionAsync();

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
        => PostgreSqlTestEnvironment.CreateDbContext(connection);

    private static async Task<(Guid CompanyId, Guid BranchId)> SeedScopeAsync(
        TransportErpDbContext db,
        string suffix)
    {
        for (var attempt = 0; attempt < 8; attempt++)
        {
            var now = DateTimeOffset.UtcNow;
            var currency = new Currency
            {
                Id = Guid.NewGuid(),
                Code = Guid.NewGuid().ToString("N")[..3].ToUpperInvariant(),
                NameAr = "عملة اختبار",
                MinorUnit = 2,
                IsBase = true,
                CreatedAt = now,
                UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Code = $"AUD-{suffix}-{Guid.NewGuid():N}"[..18],
                LegalNameAr = "شركة تدقيق اختبار",
                BaseCurrencyId = currency.Id,
                DefaultCalendarId = Guid.NewGuid(),
                CreatedAt = now,
                UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            var branch = new Branch
            {
                Id = Guid.NewGuid(),
                CompanyId = company.Id,
                Code = "MAIN",
                NameAr = "الفرع الرئيسي",
                Timezone = "Asia/Aden",
                CreatedAt = now,
                UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            db.Currencies.Add(currency);
            db.Companies.Add(company);
            db.Branches.Add(branch);
            try
            {
                await db.SaveChangesAsync();
                return (company.Id, branch.Id);
            }
            catch (Exception ex) when (IsUniqueViolation(ex) && attempt < 7)
            {
                db.ChangeTracker.Clear();
                await Task.Delay(10 * (attempt + 1));
            }
        }

        throw new InvalidOperationException("Unable to seed a unique PostgreSQL test scope after retries.");
    }

    private static bool IsUniqueViolation(Exception exception)
    {
        for (var current = exception; current is not null; current = current.InnerException)
        {
            if (current is PostgresException { SqlState: "23505" })
                return true;
        }

        return false;
    }
}
