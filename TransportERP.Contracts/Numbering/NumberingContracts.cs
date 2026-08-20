using TransportERP.Contracts.Core;

namespace TransportERP.Contracts.Numbering;

public static class NumberReservationStates
{
    public const string Reserved = "RESERVED";
    public const string Committed = "COMMITTED";
    public const string Void = "VOID";

    public static bool IsKnown(string? value) => value is Reserved or Committed or Void;
}

/// <summary>
/// Requests one authoritative number reservation from a server-owned sequence.
/// The IdempotencyKey identifies one logical operation and must return the same reservation on retry.
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
    string State)
{
    public void EnsureValid()
    {
        if (Id == Guid.Empty || SequenceId == Guid.Empty)
            throw new ArgumentException("Reservation and sequence identities are required.");
        if (string.IsNullOrWhiteSpace(RenderedNumber))
            throw new ArgumentException("Rendered number is required.", nameof(RenderedNumber));
        if (!NumberReservationStates.IsKnown(State))
            throw new ArgumentException("Unknown number reservation state.", nameof(State));
    }
}

/// <summary>
/// Server-authoritative numbering boundary. Implementations must guarantee atomic reservation,
/// idempotent retry by logical operation, and permanent non-reuse of committed or voided numbers.
/// Persistence is intentionally outside the shared contract layer.
/// </summary>
public interface INumberReservationService
{
    ValueTask<NumberReservationDto> ReserveAsync(
        OperationContext context,
        NumberReservationRequest request,
        CancellationToken cancellationToken = default);
}
