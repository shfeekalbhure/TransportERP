namespace TransportERP.Contracts.Identity;

public sealed record CreateIdentitySessionRequest(
    string UserNameOrEmail,
    string Password,
    Guid? CompanyId,
    Guid? BranchId,
    string DeviceId);

public sealed record RefreshIdentitySessionRequest(string RefreshToken, string DeviceId);

public sealed record RevokeIdentitySessionRequest(string? Reason = null);

public sealed record IdentitySessionResponse(
    Guid SessionId,
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt,
    Guid UserId,
    string DisplayName,
    Guid CompanyId,
    Guid? BranchId,
    string DeviceId);
