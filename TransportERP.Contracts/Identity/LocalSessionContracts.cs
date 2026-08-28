namespace TransportERP.Contracts.Identity;

public sealed record LocalLoginRequest(
    string UserNameOrEmail,
    string Password,
    Guid CompanyId,
    Guid? BranchId,
    string DeviceId);

public sealed record LocalRefreshRequest(string RefreshToken, string DeviceId);

public sealed record LocalSessionTokenResponse(
    Guid SessionId,
    Guid SessionFamilyId,
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt,
    Guid UserId,
    Guid CompanyId,
    Guid? BranchId,
    string DeviceId);

/// <summary>
/// Mandatory client behavior after a server-side session failure. Clients must
/// never continue to submit their offline queue after ClearAndSuspendOffline.
/// </summary>
public enum LocalCredentialDisposition
{
    Keep,
    ClearAndSuspendOffline
}

public sealed record LocalSessionFailureResponse(
    string ErrorCode,
    LocalCredentialDisposition CredentialDisposition);
