namespace TransportERP.Contracts.Numbering;

/// <summary>
/// Requests one authoritative number reservation from a server-owned sequence.
/// The IdempotencyKey must identify the logical operation and must return the same reservation on retry.
/// </summary>
public sealed record NumberReservationRequest(
    Guid SequenceId,
    string IdempotencyKey,
    string? Reason = null)
{
    public void EnsureValid()
    {
        if (SequenceId == Guid.Empty)
            throw new ArgumentException("A sequence is required.", nameof(SequenceId));
        if (string.IsNullOrWhiteSpace(IdempotencyKey))
            throw new ArgumentException("An idempotency key is required.", nameof(IdempotencyKey));
    }
}

public sealed record NumberReservationDto(
    Guid Id,
    Guid SequenceId,
    ulong NumberValue,
    string RenderedNumber,
    string State);
