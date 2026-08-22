namespace TransportERP.Contracts.Wave1;

public sealed record NumberSequenceDto(
    Guid Id,
    Guid CompanyId,
    Guid? BranchId,
    string DocumentType,
    string? Prefix,
    long NextValue,
    string ResetPolicy,
    string Status,
    long Version);

public sealed record UpdateNumberSequenceRequest(
    string? Prefix,
    string ResetPolicy,
    string Status,
    long ExpectedVersion,
    string Reason);

public sealed record NumberReservationCommandRequest(
    string IdempotencyKey,
    string? Reason = null);

public sealed record NumberReservationTransitionCommandRequest(
    string IdempotencyKey,
    string? Reason = null);

public sealed record ProtectedNumberSequenceActionRequest(
    long NextValue,
    long ExpectedVersion,
    string Reason);
