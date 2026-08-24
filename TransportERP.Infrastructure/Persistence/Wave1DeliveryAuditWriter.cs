using System.Data;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TransportERP.Infrastructure.Persistence;

public sealed record Wave1DeliveryAuditContext(
    Guid? ActorUserId,
    Guid? CompanyId,
    Guid? BranchId,
    Guid CorrelationId,
    string? DeviceId = null,
    string? Ip = null);

public sealed class Wave1DeliveryAuditWriter(TransportErpDbContext db)
{
    public async Task AppendSuccessAsync(
        string screenId,
        string operation,
        object filters,
        Wave1DeliveryAuditContext context,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(screenId)) throw new ArgumentException("SCREEN_ID_REQUIRED", nameof(screenId));
        if (string.IsNullOrWhiteSpace(operation)) throw new ArgumentException("AUDIT_OPERATION_REQUIRED", nameof(operation));

        await using var tx = await BeginAsync(ct);
        try
        {
            var previous = await db.AuditEvents.AsNoTracking()
                .Where(x => x.CompanyId == context.CompanyId && x.BranchId == context.BranchId && x.DeviceId == context.DeviceId)
                .OrderByDescending(x => x.OccurredAt)
                .ThenByDescending(x => x.Id)
                .Select(x => x.Hash)
                .FirstOrDefaultAsync(ct);

            var evt = new AuditEvent
            {
                Id = Guid.NewGuid(),
                OccurredAt = DateTimeOffset.UtcNow,
                ActorUserId = context.ActorUserId,
                CompanyId = context.CompanyId,
                BranchId = context.BranchId,
                Action = $"{screenId}.{operation}",
                Outcome = "SUCCESS",
                EntityType = "Wave1DeliveryAccess",
                EntityId = null,
                CorrelationId = context.CorrelationId,
                DeviceId = string.IsNullOrWhiteSpace(context.DeviceId) ? null : context.DeviceId.Trim(),
                AfterJson = JsonSerializer.Serialize(new
                {
                    ScreenId = screenId,
                    Operation = operation,
                    Filters = filters,
                    Context = new { context.CompanyId, context.BranchId }
                }),
                Ip = string.IsNullOrWhiteSpace(context.Ip) ? null : context.Ip.Trim(),
                PreviousHash = previous,
                Hash = string.Empty
            };
            evt.Hash = AuditEventService.ComputeHash(evt);
            db.AuditEvents.Add(evt);
            await db.SaveChangesAsync(ct);
            if (tx is not null) await tx.CommitAsync(ct);
        }
        catch
        {
            if (tx is not null) await tx.RollbackAsync(ct);
            throw;
        }
    }

    private async Task<IDbContextTransaction?> BeginAsync(CancellationToken ct)
        => db.Database.IsRelational()
            ? await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct)
            : null;
}
