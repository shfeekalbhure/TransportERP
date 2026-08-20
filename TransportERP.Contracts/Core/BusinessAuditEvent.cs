using System.Text.Json;

namespace TransportERP.Contracts.Core;

/// <summary>
/// Shared append-only audit contract. Persistence and API emission are intentionally outside the contract layer.
/// </summary>
public sealed record BusinessAuditEvent(
    Guid EventId,
    Guid ActorId,
    DateTimeOffset OccurredAt,
    Guid CompanyId,
    Guid BranchId,
    string EntityType,
    Guid RecordId,
    string Action,
    Guid CorrelationId,
    string? Reason,
    JsonElement? BeforeState,
    JsonElement? AfterState)
{
    public void EnsureComplete()
    {
        if (EventId == Guid.Empty || ActorId == Guid.Empty || CompanyId == Guid.Empty ||
            BranchId == Guid.Empty || RecordId == Guid.Empty || CorrelationId == Guid.Empty)
        {
            throw new ArgumentException("Audit identity, actor, scope, record, and correlation are required.");
        }

        if (OccurredAt == default)
        {
            throw new ArgumentException("An occurrence time is required.", nameof(OccurredAt));
        }

        if (string.IsNullOrWhiteSpace(EntityType))
        {
            throw new ArgumentException("An entity type is required.", nameof(EntityType));
        }

        if (string.IsNullOrWhiteSpace(Action))
        {
            throw new ArgumentException("An action is required.", nameof(Action));
        }
    }
}

public interface IBusinessAuditWriter
{
    ValueTask AppendAsync(BusinessAuditEvent auditEvent, CancellationToken cancellationToken = default);
}
