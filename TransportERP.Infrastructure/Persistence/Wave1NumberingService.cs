using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Numbering;
using TransportERP.Contracts.Wave1;

namespace TransportERP.Infrastructure.Persistence;

public sealed class Wave1NumberingService(TransportErpDbContext db)
{
    private DbSet<NumberSequenceEntity> Sequences => db.Set<NumberSequenceEntity>();
    private DbSet<NumberReservationEntity> Reservations => db.Set<NumberReservationEntity>();

    public async Task<IReadOnlyList<NumberSequenceDto>> ListAsync(
        OperationContext context,
        CancellationToken ct = default)
    {
        context.EnsureComplete();
        return await Sequences.AsNoTracking()
            .Where(x => x.CompanyId == context.CompanyId && (x.BranchId == null || x.BranchId == context.BranchId))
            .OrderBy(x => x.DocumentType)
            .ThenBy(x => x.BranchId)
            .Select(x => new NumberSequenceDto(
                x.Id, x.CompanyId, x.BranchId, x.DocumentType, x.Prefix,
                x.NextValue, x.ResetPolicy, x.Status, x.Version))
            .ToListAsync(ct);
    }

    public async Task<NumberSequenceDto?> UpdateAsync(
        OperationContext context,
        Guid id,
        UpdateNumberSequenceRequest request,
        CancellationToken ct = default)
    {
        context.EnsureComplete();
        ValidateUpdate(request);
        return await ExecuteAsync(async () =>
        {
            var sequence = await ScopedSequenceAsync(context, id, ct);
            if (sequence is null) return null;
            if (sequence.Version != request.ExpectedVersion)
                throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");

            var before = JsonSerializer.Serialize(ToDto(sequence));
            sequence.Prefix = NormalizeOptional(request.Prefix, 30, "INVALID_PREFIX");
            sequence.ResetPolicy = NormalizeRequired(request.ResetPolicy, 40, "INVALID_RESET_POLICY");
            sequence.Status = NormalizeStatus(request.Status);
            sequence.Version++;
            sequence.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "NumberSequence.Update", sequence.Id, before, JsonSerializer.Serialize(ToDto(sequence)), request.Reason, ct);
            await db.SaveChangesAsync(ct);
            return ToDto(sequence);
        }, ct);
    }

    public Task<NumberReservationDto> ReserveAsync(
        OperationContext context,
        Guid sequenceId,
        NumberReservationCommandRequest request,
        CancellationToken ct = default)
        => ExecuteRequiredAsync(async () =>
        {
            var reservationService = new EfNumberReservationService(db);
            var result = await reservationService.ReserveAsync(
                context,
                new NumberReservationRequest(sequenceId, request.IdempotencyKey, request.Reason),
                ct);
            await AppendAuditAsync(context, "NumberSequence.Reserve", result.Id, null, JsonSerializer.Serialize(result), request.Reason, ct);
            await db.SaveChangesAsync(ct);
            return result;
        }, ct);

    public Task<NumberReservationDto> CommitAsync(
        OperationContext context,
        Guid reservationId,
        NumberReservationTransitionCommandRequest request,
        CancellationToken ct = default)
        => ExecuteRequiredAsync(async () =>
        {
            var reservationService = new EfNumberReservationService(db);
            var result = await reservationService.CommitAsync(
                context,
                new NumberReservationTransitionRequest(reservationId, request.IdempotencyKey, request.Reason),
                ct);
            await AppendAuditAsync(context, "NumberReservation.Commit", result.Id, null, JsonSerializer.Serialize(result), request.Reason, ct);
            await db.SaveChangesAsync(ct);
            return result;
        }, ct);

    public Task<NumberReservationDto> CancelAsync(
        OperationContext context,
        Guid reservationId,
        NumberReservationTransitionCommandRequest request,
        CancellationToken ct = default)
        => ExecuteRequiredAsync(async () =>
        {
            var reservationService = new EfNumberReservationService(db);
            var result = await reservationService.VoidAsync(
                context,
                new NumberReservationTransitionRequest(reservationId, request.IdempotencyKey, request.Reason),
                ct);
            await AppendAuditAsync(context, "NumberReservation.Cancel", result.Id, null, JsonSerializer.Serialize(result), request.Reason, ct);
            await db.SaveChangesAsync(ct);
            return result;
        }, ct);

    public async Task<NumberSequenceDto?> ProtectedActionAsync(
        OperationContext context,
        Guid id,
        ProtectedNumberSequenceActionRequest request,
        CancellationToken ct = default)
    {
        context.EnsureComplete();
        if (request.NextValue < 1) throw new ArgumentException("INVALID_NEXT_VALUE");
        if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("REASON_REQUIRED");

        return await ExecuteAsync(async () =>
        {
            var sequence = await ScopedSequenceAsync(context, id, ct);
            if (sequence is null) return null;
            if (sequence.Version != request.ExpectedVersion)
                throw new DbUpdateConcurrencyException("CONCURRENCY_CONFLICT");

            var maximumAllocated = await Reservations.AsNoTracking()
                .Where(x => x.SequenceId == sequence.Id)
                .Select(x => (long?)x.NumberValue)
                .MaxAsync(ct) ?? 0;
            var minimumSafe = Math.Max(sequence.NextValue, maximumAllocated + 1);
            if (request.NextValue < minimumSafe)
                throw new InvalidOperationException("NUMBER_REUSE_FORBIDDEN");

            var before = JsonSerializer.Serialize(ToDto(sequence));
            sequence.NextValue = request.NextValue;
            sequence.Version++;
            sequence.UpdatedAt = DateTimeOffset.UtcNow;
            await AppendAuditAsync(context, "NumberSequence.ProtectedAction", sequence.Id, before, JsonSerializer.Serialize(ToDto(sequence)), request.Reason.Trim(), ct);
            await db.SaveChangesAsync(ct);
            return ToDto(sequence);
        }, ct);
    }

    private async Task<NumberSequenceEntity?> ScopedSequenceAsync(OperationContext context, Guid id, CancellationToken ct)
        => await Sequences.SingleOrDefaultAsync(
            x => x.Id == id && x.CompanyId == context.CompanyId && (x.BranchId == null || x.BranchId == context.BranchId), ct);

    private async Task<T?> ExecuteAsync<T>(Func<Task<T?>> action, CancellationToken ct)
    {
        await using var transaction = await BeginTransactionIfSupportedAsync(ct);
        try
        {
            var result = await action();
            if (transaction is not null) await transaction.CommitAsync(ct);
            return result;
        }
        catch
        {
            if (transaction is not null) await transaction.RollbackAsync(ct);
            throw;
        }
    }

    private async Task<T> ExecuteRequiredAsync<T>(Func<Task<T>> action, CancellationToken ct)
    {
        await using var transaction = await BeginTransactionIfSupportedAsync(ct);
        try
        {
            var result = await action();
            if (transaction is not null) await transaction.CommitAsync(ct);
            return result;
        }
        catch
        {
            if (transaction is not null) await transaction.RollbackAsync(ct);
            throw;
        }
    }

    private async Task<IDbContextTransaction?> BeginTransactionIfSupportedAsync(CancellationToken ct)
        => db.Database.IsRelational()
            ? await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct)
            : null;

    private async Task AppendAuditAsync(
        OperationContext context,
        string action,
        Guid entityId,
        string? beforeJson,
        string? afterJson,
        string? reason,
        CancellationToken ct)
    {
        var previousHash = await db.AuditEvents.AsNoTracking()
            .Where(x => x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.DeviceId == null)
            .OrderByDescending(x => x.OccurredAt)
            .ThenByDescending(x => x.Id)
            .Select(x => x.Hash)
            .FirstOrDefaultAsync(ct);

        var audit = new AuditEvent
        {
            Id = Guid.NewGuid(),
            OccurredAt = DateTimeOffset.UtcNow,
            ActorUserId = context.UserId,
            CompanyId = context.CompanyId,
            BranchId = context.BranchId,
            Action = action,
            Outcome = "SUCCESS",
            EntityType = action.StartsWith("NumberSequence", StringComparison.Ordinal) ? "NumberSequence" : "NumberReservation",
            EntityId = entityId,
            CorrelationId = context.CorrelationId,
            BeforeJson = beforeJson,
            AfterJson = afterJson,
            Reason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim(),
            PreviousHash = previousHash
        };
        audit.Hash = AuditEventService.ComputeHash(audit);
        db.AuditEvents.Add(audit);
    }

    private static void ValidateUpdate(UpdateNumberSequenceRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Reason)) throw new ArgumentException("REASON_REQUIRED");
        _ = NormalizeOptional(request.Prefix, 30, "INVALID_PREFIX");
        _ = NormalizeRequired(request.ResetPolicy, 40, "INVALID_RESET_POLICY");
        _ = NormalizeStatus(request.Status);
    }

    private static string? NormalizeOptional(string? value, int max, string code)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var normalized = value.Trim();
        if (normalized.Length > max) throw new ArgumentException(code);
        return normalized;
    }

    private static string NormalizeRequired(string value, int max, string code)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(code);
        var normalized = value.Trim().ToUpperInvariant();
        if (normalized.Length > max) throw new ArgumentException(code);
        return normalized;
    }

    private static string NormalizeStatus(string value)
    {
        var normalized = NormalizeRequired(value, 20, "INVALID_STATUS");
        if (normalized is not ("ACTIVE" or "INACTIVE")) throw new ArgumentException("INVALID_STATUS");
        return normalized;
    }

    private static NumberSequenceDto ToDto(NumberSequenceEntity x)
        => new(x.Id, x.CompanyId, x.BranchId, x.DocumentType, x.Prefix, x.NextValue, x.ResetPolicy, x.Status, x.Version);
}
