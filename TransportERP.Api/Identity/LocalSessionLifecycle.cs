using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Contracts.Identity;

namespace TransportERP.Api.Identity;

public enum LocalAuthenticationStatus
{
    Succeeded,
    InvalidCredentials,
    Disabled,
    ScopeDenied
}

public enum LocalSessionFailure
{
    None,
    InvalidCredentials,
    AccountDisabled,
    ScopeDenied,
    RefreshInvalid,
    RefreshExpired,
    RefreshReuseDetected,
    DeviceMismatch,
    SessionRevoked,
    SessionExpired,
    SecurityContextChanged,
    PersistenceUnavailable
}

public enum LocalRefreshRotationStatus
{
    Rotated,
    Invalid,
    Expired,
    ReuseDetectedAndFamilyRevoked,
    DeviceMismatch,
    Revoked
}

public sealed record LocalAuthoritySnapshot(
    Guid UserId,
    string DisplayName,
    Guid CompanyId,
    Guid? BranchId,
    long SecurityVersion,
    bool IsActive = true);

public sealed record LocalAuthenticationResult(
    LocalAuthenticationStatus Status,
    LocalAuthoritySnapshot? Authority = null);

public sealed record LocalSessionRecord(
    Guid SessionId,
    Guid FamilyId,
    Guid UserId,
    Guid CompanyId,
    Guid? BranchId,
    string DeviceId,
    long SecurityVersionAtIssue,
    string RefreshTokenHash,
    DateTimeOffset AccessTokenExpiresAt,
    DateTimeOffset RefreshTokenExpiresAt,
    DateTimeOffset? RevokedAt = null,
    string? RevokeReason = null,
    Guid? ReplacedBySessionId = null);

public sealed record LocalRefreshRotationRequest(
    string PresentedTokenHash,
    Guid ExpectedSessionId,
    LocalSessionRecord Replacement,
    DateTimeOffset Now);

public sealed record LocalRefreshRotationResult(
    LocalRefreshRotationStatus Status,
    LocalSessionRecord? Session = null);

public sealed record LocalSessionAuditIntent(
    Guid EventId,
    string Action,
    string Reason,
    DateTimeOffset OccurredAt);

public sealed record LocalAccessTokenDescriptor(
    Guid SessionId,
    Guid UserId,
    Guid CompanyId,
    Guid? BranchId,
    string DeviceId,
    long SecurityVersion,
    DateTimeOffset IssuedAt,
    DateTimeOffset ExpiresAt);

public sealed record LocalIssuedAccessToken(string Token, DateTimeOffset ExpiresAt);

public sealed record LocalSessionTokenResult(
    LocalSessionTokenResponse? Tokens,
    LocalSessionFailure Failure,
    LocalCredentialDisposition CredentialDisposition)
{
    public bool Succeeded => Tokens is not null && Failure == LocalSessionFailure.None;

    public static LocalSessionTokenResult Denied(LocalSessionFailure failure)
        => new(null, failure, LocalCredentialDisposition.ClearAndSuspendOffline);
}

public sealed record LocalAccessValidation(
    bool Succeeded,
    LocalSessionFailure Failure,
    LocalCredentialDisposition CredentialDisposition)
{
    public static LocalAccessValidation Allow()
        => new(true, LocalSessionFailure.None, LocalCredentialDisposition.Keep);

    public static LocalAccessValidation Deny(LocalSessionFailure failure)
        => new(false, failure, LocalCredentialDisposition.ClearAndSuspendOffline);
}

public interface ILocalIdentityAuthority
{
    Task<LocalAuthenticationResult> AuthenticateAsync(
        string userNameOrEmail,
        string password,
        Guid companyId,
        Guid? branchId,
        CancellationToken cancellationToken = default);

    Task<LocalAuthoritySnapshot?> GetCurrentAsync(
        Guid userId,
        Guid companyId,
        Guid? branchId,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Persistence boundary for DBP-003. Every mutation and its audit intent must
/// commit atomically or leave no state change. A reused or concurrently
/// consumed refresh token revokes the complete token family in that same
/// transaction. No production implementation is registered until DBP-003 is
/// approved.
/// </summary>
public interface ILocalSessionStore
{
    Task CreateWithAuditAsync(
        LocalSessionRecord session,
        LocalSessionAuditIntent audit,
        CancellationToken cancellationToken = default);

    Task<LocalSessionRecord?> FindByRefreshTokenHashAsync(
        string refreshTokenHash,
        CancellationToken cancellationToken = default);

    Task<LocalSessionRecord?> FindBySessionIdAsync(
        Guid sessionId,
        CancellationToken cancellationToken = default);

    Task<LocalRefreshRotationResult> RotateWithAuditAsync(
        LocalRefreshRotationRequest request,
        LocalSessionAuditIntent audit,
        CancellationToken cancellationToken = default);

    Task RevokeSessionWithAuditAsync(
        Guid sessionId,
        string reason,
        DateTimeOffset now,
        LocalSessionAuditIntent audit,
        CancellationToken cancellationToken = default);

    Task RevokeFamilyWithAuditAsync(
        Guid familyId,
        string reason,
        DateTimeOffset now,
        LocalSessionAuditIntent audit,
        CancellationToken cancellationToken = default);
}

public interface ILocalAccessTokenIssuer
{
    LocalIssuedAccessToken Issue(LocalAccessTokenDescriptor descriptor);
}

public sealed record LocalSessionLifecycleOptions(
    TimeSpan AccessTokenLifetime,
    TimeSpan RefreshTokenLifetime)
{
    public static LocalSessionLifecycleOptions SecureDefault { get; }
        = new(TimeSpan.FromMinutes(10), TimeSpan.FromDays(14));
}

public sealed record LocalAccessTokenOptions(
    string Issuer,
    string Audience,
    string SigningKey);

/// <summary>
/// Issues narrow selector claims only. Roles and permissions intentionally do
/// not enter the token; request-time persistent RBAC remains authoritative.
/// </summary>
public sealed class JwtLocalAccessTokenIssuer(LocalAccessTokenOptions options) : ILocalAccessTokenIssuer
{
    public LocalIssuedAccessToken Issue(LocalAccessTokenDescriptor descriptor)
    {
        if (string.IsNullOrWhiteSpace(options.Issuer) ||
            string.IsNullOrWhiteSpace(options.Audience) ||
            string.IsNullOrWhiteSpace(options.SigningKey) ||
            options.SigningKey.Length < 32)
            throw new InvalidOperationException("Local access-token configuration is invalid.");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, descriptor.UserId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("session_id", descriptor.SessionId.ToString()),
            new("company_id", descriptor.CompanyId.ToString()),
            new("device_id", descriptor.DeviceId),
            new("security_version", descriptor.SecurityVersion.ToString()),
            new("auth_mode", "local")
        };
        if (descriptor.BranchId.HasValue)
            claims.Add(new Claim("branch_id", descriptor.BranchId.Value.ToString()));

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            options.Issuer,
            options.Audience,
            claims,
            descriptor.IssuedAt.UtcDateTime,
            descriptor.ExpiresAt.UtcDateTime,
            credentials);
        return new(new JwtSecurityTokenHandler().WriteToken(token), descriptor.ExpiresAt);
    }
}

/// <summary>
/// Code-only local-authority lifecycle. Its store and identity adapters are
/// explicit boundaries; the application must fail closed until durable DBP-003
/// adapters are approved and registered.
/// </summary>
public sealed class LocalSessionLifecycleService(
    ILocalIdentityAuthority identities,
    ILocalSessionStore sessions,
    ILocalAccessTokenIssuer accessTokens,
    TimeProvider timeProvider,
    LocalSessionLifecycleOptions options)
{
    public async Task<LocalSessionTokenResult> LoginAsync(
        LocalLoginRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.UserNameOrEmail) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.DeviceId))
            return LocalSessionTokenResult.Denied(LocalSessionFailure.InvalidCredentials);

        var authentication = await identities.AuthenticateAsync(
            request.UserNameOrEmail.Trim(), request.Password, request.CompanyId,
            request.BranchId, cancellationToken);
        if (authentication.Status != LocalAuthenticationStatus.Succeeded ||
            authentication.Authority is null)
            return LocalSessionTokenResult.Denied(authentication.Status switch
            {
                LocalAuthenticationStatus.Disabled => LocalSessionFailure.AccountDisabled,
                LocalAuthenticationStatus.ScopeDenied => LocalSessionFailure.ScopeDenied,
                _ => LocalSessionFailure.InvalidCredentials
            });

        var authority = authentication.Authority;
        if (!authority.IsActive || authority.CompanyId != request.CompanyId ||
            authority.BranchId != request.BranchId)
            return LocalSessionTokenResult.Denied(LocalSessionFailure.ScopeDenied);

        var now = timeProvider.GetUtcNow();
        return await CreateSessionAsync(
            authority, request.DeviceId.Trim(), Guid.NewGuid(), now, cancellationToken);
    }

    public async Task<LocalSessionTokenResult> RefreshAsync(
        LocalRefreshRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken) ||
            string.IsNullOrWhiteSpace(request.DeviceId))
            return LocalSessionTokenResult.Denied(LocalSessionFailure.RefreshInvalid);

        var now = timeProvider.GetUtcNow();
        var presentedHash = HashRefreshToken(request.RefreshToken);
        var current = await sessions.FindByRefreshTokenHashAsync(
            presentedHash, cancellationToken);
        if (current is null)
            return LocalSessionTokenResult.Denied(LocalSessionFailure.RefreshInvalid);
        if (current.RevokedAt.HasValue || current.ReplacedBySessionId.HasValue)
        {
            await RevokeFamilyAsync(current.FamilyId, "REFRESH_REUSE", now, cancellationToken);
            return LocalSessionTokenResult.Denied(LocalSessionFailure.RefreshReuseDetected);
        }
        if (current.RefreshTokenExpiresAt <= now)
        {
            await RevokeFamilyAsync(current.FamilyId, "REFRESH_EXPIRED", now, cancellationToken);
            return LocalSessionTokenResult.Denied(LocalSessionFailure.RefreshExpired);
        }
        if (!string.Equals(current.DeviceId, request.DeviceId.Trim(), StringComparison.Ordinal))
        {
            await RevokeFamilyAsync(current.FamilyId, "DEVICE_MISMATCH", now, cancellationToken);
            return LocalSessionTokenResult.Denied(LocalSessionFailure.DeviceMismatch);
        }

        var authority = await identities.GetCurrentAsync(
            current.UserId, current.CompanyId, current.BranchId, cancellationToken);
        if (!AuthorityMatches(current, authority))
        {
            await RevokeFamilyAsync(current.FamilyId, "SECURITY_CONTEXT_CHANGED", now, cancellationToken);
            return LocalSessionTokenResult.Denied(LocalSessionFailure.SecurityContextChanged);
        }

        var refreshToken = NewRefreshToken();
        var replacement = NewSession(
            authority!, current.DeviceId, current.FamilyId, refreshToken.Hash, now);
        var rotation = await sessions.RotateWithAuditAsync(
            new LocalRefreshRotationRequest(presentedHash, current.SessionId, replacement, now),
            Audit("SESSION_REFRESH_ROTATED", "ROTATED", now),
            cancellationToken);
        if (rotation.Status != LocalRefreshRotationStatus.Rotated || rotation.Session is null)
            return LocalSessionTokenResult.Denied(rotation.Status switch
            {
                LocalRefreshRotationStatus.Expired => LocalSessionFailure.RefreshExpired,
                LocalRefreshRotationStatus.DeviceMismatch => LocalSessionFailure.DeviceMismatch,
                LocalRefreshRotationStatus.Revoked => LocalSessionFailure.SessionRevoked,
                LocalRefreshRotationStatus.ReuseDetectedAndFamilyRevoked => LocalSessionFailure.RefreshReuseDetected,
                _ => LocalSessionFailure.RefreshInvalid
            });

        return Issue(rotation.Session, authority!, refreshToken.Raw, now);
    }

    public Task LogoutAsync(
        Guid currentSessionId,
        CancellationToken cancellationToken = default)
    {
        var now = timeProvider.GetUtcNow();
        return sessions.RevokeSessionWithAuditAsync(
            currentSessionId, "LOGOUT", now,
            Audit("SESSION_LOGOUT", "LOGOUT", now), cancellationToken);
    }

    public Task RevokeCurrentSessionAsync(
        Guid currentSessionId,
        string reason,
        CancellationToken cancellationToken = default)
    {
        var now = timeProvider.GetUtcNow();
        var normalizedReason = NormalizeReason(reason, "CURRENT_SESSION_REVOKED");
        return sessions.RevokeSessionWithAuditAsync(
            currentSessionId, normalizedReason, now,
            Audit("SESSION_REVOKED", normalizedReason, now), cancellationToken);
    }

    public async Task RevokeSessionFamilyAsync(
        Guid currentSessionId,
        string reason,
        CancellationToken cancellationToken = default)
    {
        var session = await sessions.FindBySessionIdAsync(currentSessionId, cancellationToken);
        if (session is not null)
        {
            var now = timeProvider.GetUtcNow();
            await RevokeFamilyAsync(
                session.FamilyId, NormalizeReason(reason, "SESSION_FAMILY_REVOKED"),
                now, cancellationToken);
        }
    }

    public async Task<LocalAccessValidation> ValidateAccessAsync(
        LocalAccessTokenDescriptor descriptor,
        CancellationToken cancellationToken = default)
    {
        var now = timeProvider.GetUtcNow();
        if (descriptor.ExpiresAt <= now)
            return LocalAccessValidation.Deny(LocalSessionFailure.SessionExpired);

        var session = await sessions.FindBySessionIdAsync(
            descriptor.SessionId, cancellationToken);
        if (session is null || session.RevokedAt.HasValue)
            return LocalAccessValidation.Deny(LocalSessionFailure.SessionRevoked);
        if (session.UserId != descriptor.UserId ||
            session.CompanyId != descriptor.CompanyId ||
            session.BranchId != descriptor.BranchId ||
            !string.Equals(session.DeviceId, descriptor.DeviceId, StringComparison.Ordinal) ||
            session.SecurityVersionAtIssue != descriptor.SecurityVersion)
            return LocalAccessValidation.Deny(LocalSessionFailure.SecurityContextChanged);

        var authority = await identities.GetCurrentAsync(
            session.UserId, session.CompanyId, session.BranchId, cancellationToken);
        if (!AuthorityMatches(session, authority))
        {
            await RevokeFamilyAsync(
                session.FamilyId, "SECURITY_CONTEXT_CHANGED", now, cancellationToken);
            return LocalAccessValidation.Deny(LocalSessionFailure.SecurityContextChanged);
        }

        return LocalAccessValidation.Allow();
    }

    public Task<LocalAccessValidation> ValidateOfflineMutationAsync(
        LocalAccessTokenDescriptor descriptor,
        CancellationToken cancellationToken = default)
        => ValidateAccessAsync(descriptor, cancellationToken);

    private async Task<LocalSessionTokenResult> CreateSessionAsync(
        LocalAuthoritySnapshot authority,
        string deviceId,
        Guid familyId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var refreshToken = NewRefreshToken();
        var session = NewSession(authority, deviceId, familyId, refreshToken.Hash, now);
        await sessions.CreateWithAuditAsync(
            session, Audit("SESSION_CREATED", "LOGIN", now), cancellationToken);
        return Issue(session, authority, refreshToken.Raw, now);
    }

    private Task RevokeFamilyAsync(
        Guid familyId,
        string reason,
        DateTimeOffset now,
        CancellationToken cancellationToken)
        => sessions.RevokeFamilyWithAuditAsync(
            familyId, reason, now,
            Audit("SESSION_FAMILY_REVOKED", reason, now), cancellationToken);

    private static LocalSessionAuditIntent Audit(
        string action,
        string reason,
        DateTimeOffset now)
        => new(Guid.NewGuid(), action, reason, now);

    private LocalSessionTokenResult Issue(
        LocalSessionRecord session,
        LocalAuthoritySnapshot authority,
        string refreshToken,
        DateTimeOffset now)
    {
        var descriptor = new LocalAccessTokenDescriptor(
            session.SessionId, session.UserId, session.CompanyId, session.BranchId,
            session.DeviceId, session.SecurityVersionAtIssue, now,
            session.AccessTokenExpiresAt);
        var access = accessTokens.Issue(descriptor);
        return new LocalSessionTokenResult(
            new LocalSessionTokenResponse(
                session.SessionId, session.FamilyId, access.Token, access.ExpiresAt,
                refreshToken, session.RefreshTokenExpiresAt, session.UserId,
                session.CompanyId, session.BranchId, session.DeviceId),
            LocalSessionFailure.None,
            LocalCredentialDisposition.Keep);
    }

    private LocalSessionRecord NewSession(
        LocalAuthoritySnapshot authority,
        string deviceId,
        Guid familyId,
        string refreshTokenHash,
        DateTimeOffset now)
        => new(
            Guid.NewGuid(), familyId, authority.UserId, authority.CompanyId,
            authority.BranchId, deviceId, authority.SecurityVersion,
            refreshTokenHash, now.Add(options.AccessTokenLifetime),
            now.Add(options.RefreshTokenLifetime));

    private static bool AuthorityMatches(
        LocalSessionRecord session,
        LocalAuthoritySnapshot? authority)
        => authority is { IsActive: true } &&
           authority.UserId == session.UserId &&
           authority.CompanyId == session.CompanyId &&
           authority.BranchId == session.BranchId &&
           authority.SecurityVersion == session.SecurityVersionAtIssue;

    private static (string Raw, string Hash) NewRefreshToken()
    {
        var raw = Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(32));
        return (raw, HashRefreshToken(raw));
    }

    public static string HashRefreshToken(string rawToken)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));

    private static string NormalizeReason(string? reason, string fallback)
        => string.IsNullOrWhiteSpace(reason) ? fallback : reason.Trim()[..Math.Min(reason.Trim().Length, 120)];
}
