using System.Text.Json;

namespace TransportERP.Contracts.Core;

/// <summary>
/// Shared append-only audit contract. Persistence and API emission are intentionally outside W1-CORE.
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

/// <summary>
/// Contract boundary for appending an audit event. No persistence implementation is provided here.
/// </summary>
public interface IBusinessAuditWriter
{
    ValueTask AppendAsync(BusinessAuditEvent auditEvent, CancellationToken cancellationToken = default);
}
