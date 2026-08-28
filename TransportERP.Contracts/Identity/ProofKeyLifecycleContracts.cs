using System.Text.Json;

namespace TransportERP.Contracts.Identity;

public sealed record CreateProofKeyChallengeRequest(
    Guid ChangeRequestId,
    string ChangeType,
    int? ExpectedProofKeyVersion,
    JsonElement NewPublicJwk);

public sealed record ProofKeyChallengeResponse(
    Guid ChallengeId,
    Guid ChangeRequestId,
    string ChangeType,
    int? ExpectedProofKeyVersion,
    string NewProofKeyThumbprint,
    DateTimeOffset IssuedAt,
    DateTimeOffset ExpiresAt,
    string? Challenge);

public sealed record ChangeProofKeyRequest(
    Guid ChallengeId,
    Guid ChangeRequestId,
    string ChangeType,
    int? ExpectedProofKeyVersion,
    JsonElement NewPublicJwk,
    string? Reason);

public sealed record ProofKeyChangeResponse(
    Guid RegisteredDeviceId,
    Guid ChangeRequestId,
    string ChangeType,
    string ProofKeyThumbprint,
    int ProofKeyVersion,
    DateTimeOffset ChangedAt);
