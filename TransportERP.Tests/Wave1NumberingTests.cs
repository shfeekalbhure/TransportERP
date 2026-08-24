using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Numbering;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

public sealed class Wave1NumberingTests
{
    [Fact]
    public async Task Numbering_lifecycle_is_idempotent_non_reusable_concurrent_and_audited()
    {
        await using var db = CreateDb();
        var now = DateTimeOffset.UtcNow;
        var companyId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var sequence = new NumberSequenceEntity
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId,
            BranchId = branchId,
            DocumentType = "WAYBILL",
            Prefix = "WB-",
            NextValue = 1,
            ResetPolicy = "NONE",
            Status = "ACTIVE",
            Version = 1,
            CreatedAt = now,
            UpdatedAt = now
        };
        db.Set<NumberSequenceEntity>().Add(sequence);
        await db.SaveChangesAsync();

        var context = new OperationContext(Guid.NewGuid(), companyId, branchId, Guid.NewGuid());
        var service = new Wave1NumberingService(db);

        var listed = await service.ListAsync(context);
        Assert.Equal(sequence.Id, Assert.Single(listed).Id);

        var updated = await service.UpdateAsync(
            context,
            sequence.Id,
            new UpdateNumberSequenceRequest("WB26-", "NONE", "ACTIVE", 1, "تحديث بادئة الاختبار"));
        Assert.NotNull(updated);
        Assert.Equal(2, updated!.Version);
        Assert.Equal("WB26-", updated.Prefix);

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => service.UpdateAsync(
            context,
            sequence.Id,
            new UpdateNumberSequenceRequest("OLD-", "NONE", "ACTIVE", 1, "نسخة قديمة")));

        var first = await service.ReserveAsync(
            context,
            sequence.Id,
            new NumberReservationCommandRequest("reserve-1", "الحجز الأول"));
        Assert.Equal((ulong)1, first.NumberValue);
        Assert.Equal("WB26-00000001", first.RenderedNumber);

        var firstRetry = await service.ReserveAsync(
            context,
            sequence.Id,
            new NumberReservationCommandRequest("reserve-1", "إعادة المحاولة"));
        Assert.Equal(first.Id, firstRetry.Id);
        Assert.Single(db.Set<NumberReservationEntity>().Where(x => x.IdempotencyKey == "reserve-1"));

        var cancelled = await service.CancelAsync(
            context,
            first.Id,
            new NumberReservationTransitionCommandRequest("cancel-1", "إلغاء للاختبار"));
        Assert.Equal(NumberReservationStates.Void, cancelled.State);

        var second = await service.ReserveAsync(
            context,
            sequence.Id,
            new NumberReservationCommandRequest("reserve-2", "الحجز الثاني"));
        Assert.Equal((ulong)2, second.NumberValue);
        Assert.NotEqual(first.NumberValue, second.NumberValue);

        var currentSequence = await db.Set<NumberSequenceEntity>().SingleAsync(x => x.Id == sequence.Id);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ProtectedActionAsync(
            context,
            sequence.Id,
            new ProtectedNumberSequenceActionRequest(1, currentSequence.Version, "محاولة رجوع ممنوعة")));

        var advanced = await service.ProtectedActionAsync(
            context,
            sequence.Id,
            new ProtectedNumberSequenceActionRequest(9, currentSequence.Version, "تقديم آمن للتسلسل"));
        Assert.NotNull(advanced);
        Assert.Equal(10, advanced!.NextValue);

        var third = await service.ReserveAsync(
            context,
            sequence.Id,
            new NumberReservationCommandRequest("reserve-3", "حجز بعد التقديم"));
        Assert.Equal((ulong)10, third.NumberValue);

        var thirdEntity = await db.Set<NumberReservationEntity>().SingleAsync(x => x.Id == third.Id);
        thirdEntity.WaybillId = Guid.NewGuid();
        await db.SaveChangesAsync();

        var committed = await service.CommitAsync(
            context,
            third.Id,
            new NumberReservationTransitionCommandRequest("commit-3", "اعتماد الرقم"));
        Assert.Equal(NumberReservationStates.Committed, committed.State);

        await Assert.ThrowsAsync<WaybillPersistenceException>(() => service.CancelAsync(
            context,
            third.Id,
            new NumberReservationTransitionCommandRequest("cancel-after-commit", "يجب الرفض")));

        var audits = await db.AuditEvents
            .OrderBy(x => x.OccurredAt)
            .ThenBy(x => x.Id)
            .ToListAsync();
        Assert.Contains(audits, x => x.Action == "NumberSequence.Update");
        Assert.Contains(audits, x => x.Action == "NumberSequence.Reserve");
        Assert.Contains(audits, x => x.Action == "NumberReservation.Cancel");
        Assert.Contains(audits, x => x.Action == "NumberSequence.ProtectedAction");
        Assert.Contains(audits, x => x.Action == "NumberReservation.Commit");
        Assert.All(audits, x => Assert.Equal(AuditEventService.ComputeHash(x), x.Hash));
        for (var i = 1; i < audits.Count; i++)
            Assert.Equal(audits[i - 1].Hash, audits[i].PreviousHash);
    }

    [Fact]
    public async Task Numbering_scope_hides_other_company_and_branch_sequences()
    {
        await using var db = CreateDb();
        var now = DateTimeOffset.UtcNow;
        var companyId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var otherBranch = Guid.NewGuid();
        db.Set<NumberSequenceEntity>().AddRange(
            Sequence(companyId, null, "GLOBAL", now),
            Sequence(companyId, branchId, "LOCAL", now),
            Sequence(companyId, otherBranch, "OTHER-BRANCH", now),
            Sequence(Guid.NewGuid(), branchId, "OTHER-COMPANY", now));
        await db.SaveChangesAsync();

        var service = new Wave1NumberingService(db);
        var rows = await service.ListAsync(new OperationContext(Guid.NewGuid(), companyId, branchId, Guid.NewGuid()));

        Assert.Equal(2, rows.Count);
        Assert.Contains(rows, x => x.DocumentType == "GLOBAL");
        Assert.Contains(rows, x => x.DocumentType == "LOCAL");
        Assert.DoesNotContain(rows, x => x.DocumentType == "OTHER-BRANCH");
        Assert.DoesNotContain(rows, x => x.DocumentType == "OTHER-COMPANY");
    }

    private static NumberSequenceEntity Sequence(Guid companyId, Guid? branchId, string documentType, DateTimeOffset now)
        => new()
        {
            Id = Guid.NewGuid(),
            CompanyId = companyId,
            BranchId = branchId,
            DocumentType = documentType,
            Prefix = null,
            NextValue = 1,
            ResetPolicy = "NONE",
            Status = "ACTIVE",
            Version = 1,
            CreatedAt = now,
            UpdatedAt = now
        };

    private static TransportErpDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<TransportErpDbContext>()
            .UseInMemoryDatabase($"wave1-numbering-{Guid.NewGuid():N}")
            .ReplaceService<IModelCustomizer, TransportErpP2CombinedModelCustomizer>()
            .Options;
        return new TransportErpDbContext(options);
    }
}
