namespace TransportERP.Contracts.Tracking;

/// <summary>
/// Shared append-only movement metadata. Domain payload such as quantity or trip references
/// remains in the owning module; this envelope standardizes identity, scope, time, retry, and reversal.
/// </summary>
public sealed record MovementEnvelope(
    Guid EventId,
    Guid CompanyId,
    Guid BranchId,
    string EntityType,
    Guid EntityId,
    string EventType,
    DateTimeOffset OccurredAt,
    DateTimeOffset RecordedAt,
    Guid RecordedBy,
    Guid CorrelationId,
    string? ClientOperationId,
    Guid? ReversesEventId)
{
    public void EnsureComplete()
    {
        if (EventId == Guid.Empty || CompanyId == Guid.Empty || BranchId == Guid.Empty ||
            EntityId == Guid.Empty || RecordedBy == Guid.Empty || CorrelationId == Guid.Empty)
        {
            throw new ArgumentException("Movement identity, scope, actor, entity, and correlation are required.");
        }
        if (string.IsNullOrWhiteSpace(EntityType) || string.IsNullOrWhiteSpace(EventType))
        {
            throw new ArgumentException("Movement entity type and event type are required.");
        }
        if (OccurredAt == default || RecordedAt == default)
        {
            throw new ArgumentException("Movement occurrence and recording timestamps are required.");
        }
        if (ReversesEventId == EventId)
        {
            throw new ArgumentException("A movement event cannot reverse itself.", nameof(ReversesEventId));
        }
        if (ClientOperationId is { Length: > 0 } && string.IsNullOrWhiteSpace(ClientOperationId))
        {
            throw new ArgumentException("Client operation identity cannot be whitespace.", nameof(ClientOperationId));
        }
    }
}
