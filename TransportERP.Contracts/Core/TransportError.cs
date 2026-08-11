namespace TransportERP.Contracts.Core;

public enum TransportErrorCode
{
    ValidationFailed,
    PermissionDenied,
    ScopeDenied,
    ConcurrencyConflict,
    NotFound,
    // W1-CORE LIMITED CROSS-CUTTING AMENDMENT UNDER W1-SETUP-ORG (ORG-OD-003A).
    Conflict,
    StateTransitionInvalid,
    ApprovalStateInvalid,
    PeriodClosed,
    SelfApprovalDenied,
    DuplicateNumber,
    NumberSequenceInactive,
    NumberingStateInvalid,
    IdempotencyConflict
}

/// <summary>
/// Safe, structured error metadata. MessageKey is resolved by the presentation layer;
/// this contract deliberately carries no raw technical exception detail.
/// </summary>
public sealed record TransportError(
    TransportErrorCode Code,
    Guid CorrelationId,
    string MessageKey)
{
    public void EnsureComplete()
    {
        if (!Enum.IsDefined(Code))
        {
            throw new ArgumentException("A defined transport error code is required.", nameof(Code));
        }

        if (CorrelationId == Guid.Empty)
        {
            throw new ArgumentException("A correlation identifier is required.", nameof(CorrelationId));
        }

        if (string.IsNullOrWhiteSpace(MessageKey))
        {
            throw new ArgumentException("A message key is required.", nameof(MessageKey));
        }
    }
}
