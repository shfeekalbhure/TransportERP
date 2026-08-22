using System.Text.Json.Serialization;

namespace TransportERP.Contracts.Wave1;

public sealed record NumberSequenceDto(
    Guid Id,
    Guid CompanyId,
    Guid? BranchId,
    string DocumentType,
    string? Prefix,
    [property: JsonIgnore] long NextValue,
    string ResetPolicy,
    string Status,
    long Version)
{
    public string Code { get; init; } = string.Empty;
    public string ArabicName { get; init; } = string.Empty;
    public string? EnglishName { get; init; }
    public string? Notes { get; init; }
    public Guid? FiscalYearId { get; init; }
    public string Scope { get; init; } = "COMPANY";
    public long LastNumber { get; init; }
}

public sealed record UpdateNumberSequenceRequest(
    string? Prefix,
    string ResetPolicy,
    string Status,
    long ExpectedVersion,
    string Reason,
    string? Code = null,
    string? ArabicName = null,
    string? EnglishName = null,
    string? Notes = null,
    Guid? FiscalYearId = null);

public sealed record NumberReservationCommandRequest(
    string IdempotencyKey,
    string? Reason = null);

public sealed record NumberReservationTransitionCommandRequest(
    string IdempotencyKey,
    string? Reason = null);

public sealed record ProtectedNumberSequenceActionRequest(
    long LastNumber,
    long ExpectedVersion,
    string Reason)
{
    [JsonIgnore]
    public long NextValue => checked(LastNumber + 1);
}
