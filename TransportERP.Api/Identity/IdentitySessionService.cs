using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TransportERP.Api.Security;
using TransportERP.Contracts.Identity;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Identity;

public sealed class IdentitySessionException(string code) : Exception(code) { public string Code { get; } = code; }

public sealed class IdentityPasswordSentinel
{
    public IdentityPasswordSentinel(IPasswordHasher<User> passwordHasher)
        => Hash = passwordHasher.HashPassword(new User(), "TransportERP-Dummy-Password");

    public string Hash { get; }
}

public sealed class IdentitySessionService(TransportErpDbContext db, IPasswordHasher<User> passwordHasher,
    IdentityPasswordSentinel sentinel, TenantScopeResolver scopeResolver, AuditEventService audit,
    IOptions<TransportSecurityOptions> options)
{
    public const int MaxPasswordLength = 1024;
    public const int MaxRefreshTokenLength = 256;
    private const string InvalidRefreshPartitionSeed = "<invalid-refresh>";
    private readonly TransportSecurityOptions settings = options.Value;
    private readonly string dummyHash = sentinel.Hash;

    public Task<IdentitySessionResponse> CreateAsync(CreateIdentitySessionRequest request,
        Guid correlationId, string? ip, CancellationToken ct)
        => CreateCoreAsync(request, NormalizeLogin(request.UserNameOrEmail), NormalizeDevice(request.DeviceId),
            correlationId, ip, ct);

    internal Task<IdentitySessionResponse> CreateNormalizedAsync(CreateIdentitySessionRequest request,
        string? normalizedLogin, string? normalizedDevice, Guid correlationId, string? ip, CancellationToken ct)
        => CreateCoreAsync(request, normalizedLogin, normalizedDevice, correlationId, ip, ct);

    private async Task<IdentitySessionResponse> CreateCoreAsync(CreateIdentitySessionRequest request,
        string? normalizedLogin, string? deviceId, Guid correlationId, string? ip, CancellationToken ct)
    {
        EnsureLocalMode();
        if (request.Password is { Length: > MaxPasswordLength })
        {
            await AuditAsync("IdentityLogin", "FAILURE", null, null, null, null, correlationId,
                "INVALID_CREDENTIALS", ip, ct);
            throw InvalidCredentials();
        }
        if (normalizedLogin is null || deviceId is null || string.IsNullOrWhiteSpace(request.Password))
        {
            _ = passwordHasher.VerifyHashedPassword(new User(), dummyHash, request.Password ?? string.Empty);
            await AuditAsync("IdentityLogin", "FAILURE", null, null, null, null, correlationId, "INVALID_CREDENTIALS", ip, ct);
            throw InvalidCredentials();
        }

        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        Guid[] candidateIds;
        if (request.CompanyId.HasValue)
        {
            candidateIds = await db.Users.AsNoTracking()
                .Where(x => x.CompanyId == request.CompanyId &&
                            (x.NormalizedUserName == normalizedLogin || x.NormalizedEmail == normalizedLogin))
                .Select(x => x.Id).Take(2).ToArrayAsync(ct);
            if (candidateIds.Length == 0)
                candidateIds = await db.Users.AsNoTracking()
                    .Where(x => x.CompanyId == null &&
                                (x.NormalizedUserName == normalizedLogin || x.NormalizedEmail == normalizedLogin))
                    .Select(x => x.Id).Take(2).ToArrayAsync(ct);
        }
        else
            candidateIds = await db.Users.AsNoTracking()
                .Where(x => x.CompanyId == null &&
                            (x.NormalizedUserName == normalizedLogin || x.NormalizedEmail == normalizedLogin))
                .Select(x => x.Id).Take(2).ToArrayAsync(ct);
        var candidateId = candidateIds.Length == 1 ? candidateIds[0] : (Guid?)null;
        User? user = null;
        if (candidateId.HasValue)
        {
            user = await db.Users.FromSqlInterpolated(
                $"SELECT * FROM transport_erp.users WHERE \"Id\" = {candidateId.Value} FOR UPDATE").SingleOrDefaultAsync(ct);
        }

        var verification = VerifyPassword(user, request.Password);
        var now = DateTimeOffset.UtcNow;
        var accountUsable = user is { Status: "ACTIVE" } && !(user.LockoutEnd > now);
        if (user is null || verification == PasswordVerificationResult.Failed || !accountUsable)
        {
            if (user is { Status: "ACTIVE" } && !(user.LockoutEnd > now) && verification == PasswordVerificationResult.Failed)
            {
                user.AccessFailedCount++;
                if (user.AccessFailedCount >= settings.MaxFailures)
                {
                    user.AccessFailedCount = 0;
                    user.LockoutEnd = now.AddMinutes(settings.LockoutMinutes);
                }
                Touch(user, now);
                await db.SaveChangesAsync(ct);
            }
            await AuditAsync("IdentityLogin", "FAILURE", user?.Id, user?.CompanyId, user?.BranchId, null,
                correlationId, user is null ? "INVALID_CREDENTIALS" : !accountUsable ? "ACCOUNT_DISABLED_OR_LOCKED" : "INVALID_CREDENTIALS", ip, ct);
            await transaction.CommitAsync(ct);
            throw InvalidCredentials();
        }

        var scope = await scopeResolver.ResolveAsync(user, request.CompanyId, request.BranchId, ct);
        if (scope is null)
        {
            await AuditAsync("IdentityLogin", "FAILURE", user.Id, user.CompanyId, user.BranchId, null,
                correlationId, "SCOPE_DENIED", ip, ct);
            await transaction.CommitAsync(ct);
            throw InvalidCredentials();
        }

        if (verification == PasswordVerificationResult.SuccessRehashNeeded)
            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
        user.AccessFailedCount = 0;
        user.LockoutEnd = null;
        user.LastLoginAt = now;
        Touch(user, now);
        var refresh = NewRefreshToken();
        var session = NewSession(user, scope.Company.Id, scope.Branch?.Id, deviceId, refresh.Hash, Guid.NewGuid(), now);
        db.AuthSessions.Add(session);
        await db.SaveChangesAsync(ct);
        await AuditAsync("IdentityLogin", "SUCCESS", user.Id, session.CompanyId, session.BranchId, session.DeviceId,
            correlationId, "SESSION_CREATED", ip, ct);
        await transaction.CommitAsync(ct);
        return ToResponse(session, user, refresh.Raw);
    }

    public Task<IdentitySessionResponse> RefreshAsync(RefreshIdentitySessionRequest request,
        Guid correlationId, string? ip, CancellationToken ct)
        => RefreshCoreAsync(request, NormalizeDevice(request.DeviceId), correlationId, ip, ct);

    internal Task<IdentitySessionResponse> RefreshNormalizedAsync(RefreshIdentitySessionRequest request,
        string? normalizedDevice, Guid correlationId, string? ip, CancellationToken ct)
        => RefreshCoreAsync(request, normalizedDevice, correlationId, ip, ct);

    private async Task<IdentitySessionResponse> RefreshCoreAsync(RefreshIdentitySessionRequest request,
        string? deviceId, Guid correlationId, string? ip, CancellationToken ct)
    {
        EnsureLocalMode();
        if (deviceId is null || !TryNormalizeRefreshToken(request.RefreshToken, out var refreshToken))
        {
            await AuditAsync("IdentityRefresh", "FAILURE", null, null, null, null, correlationId, "REFRESH_TOKEN_INVALID", ip, ct);
            throw InvalidRefresh();
        }
        var hash = HashToken(refreshToken);
        try
        {
            await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
            var old = await LockSessionByRefreshHashAsync(hash, ct);
            if (old is null)
            {
                await AuditAsync("IdentityRefresh", "FAILURE", null, null, null, null, correlationId, "REFRESH_TOKEN_INVALID", ip, ct);
                await transaction.CommitAsync(ct);
                throw InvalidRefresh();
            }
            var now = DateTimeOffset.UtcNow;
            if (old.RevokedAt.HasValue) return await RejectTrustedRefreshAsync(old, "REFRESH_TOKEN_REUSE", correlationId, ip, transaction, ct);
            if (old.RefreshTokenExpiresAt <= now) return await RejectTrustedRefreshAsync(old, "REFRESH_TOKEN_EXPIRED", correlationId, ip, transaction, ct);
            if (!string.Equals(old.DeviceId, deviceId, StringComparison.Ordinal)) return await RejectTrustedRefreshAsync(old, "DEVICE_MISMATCH", correlationId, ip, transaction, ct);

            var user = await db.Users.SingleOrDefaultAsync(x => x.Id == old.UserId, ct);
            var scope = user is null ? null : await scopeResolver.ResolveAsync(user, old.CompanyId, old.BranchId, ct);
            if (user is null || user.Status != "ACTIVE" || user.LockoutEnd > now || scope is null ||
                !FixedEquals(user.SecurityStamp, old.SecurityStampAtIssue) || user.AuthVersion != old.AuthVersionAtIssue)
                return await RejectTrustedRefreshAsync(old, "SECURITY_CONTEXT_CHANGED", correlationId, ip, transaction, ct);

            var refresh = NewRefreshToken();
            var replacement = NewSession(user, old.CompanyId, old.BranchId, old.DeviceId, refresh.Hash, old.RefreshTokenFamilyId, now);
            old.RevokedAt = now; old.RevokeReason = "ROTATED"; old.ReplacedBySessionId = replacement.Id;
            old.LastUsedAt = now; old.UpdatedAt = now; old.RowVersion = RandomNumberGenerator.GetBytes(16);
            db.AuthSessions.Add(replacement);
            await db.SaveChangesAsync(ct);
            await AuditAsync("IdentityRefresh", "SUCCESS", user.Id, old.CompanyId, old.BranchId, old.DeviceId,
                correlationId, "SESSION_ROTATED", ip, ct);
            await transaction.CommitAsync(ct);
            return ToResponse(replacement, user, refresh.Raw);
        }
        catch (DbUpdateConcurrencyException)
        {
            db.ChangeTracker.Clear();
            await RevokeAfterRefreshRaceAsync(hash, correlationId, ip, ct);
            throw InvalidRefresh();
        }
    }

    public async Task RevokeAsync(Guid sessionId, CurrentSecurityContext current, string? reason,
        Guid correlationId, string? ip, CancellationToken ct)
    {
        EnsureLocalMode();
        if (current.SessionId != sessionId) throw new IdentitySessionException("SCOPE_DENIED");
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        var session = await db.AuthSessions.FromSqlInterpolated(
            $"SELECT * FROM transport_erp.auth_sessions WHERE \"Id\" = {sessionId} FOR UPDATE").SingleOrDefaultAsync(ct);
        if (session is null || session.UserId != current.UserId) throw new IdentitySessionException("SESSION_NOT_FOUND");
        if (!session.RevokedAt.HasValue)
        {
            session.RevokedAt = DateTimeOffset.UtcNow;
            session.RevokeReason = LimitReason(reason, "SELF_REVOKED");
            session.UpdatedAt = session.RevokedAt.Value; session.RowVersion = RandomNumberGenerator.GetBytes(16);
            await db.SaveChangesAsync(ct);
        }
        await AuditAsync("IdentitySessionRevoke", "SUCCESS", session.UserId, session.CompanyId, session.BranchId,
            session.DeviceId, correlationId, session.RevokeReason ?? "SELF_REVOKED", ip, ct);
        await transaction.CommitAsync(ct);
    }

    private async Task<IdentitySessionResponse> RejectTrustedRefreshAsync(AuthSession session, string reason,
        Guid correlationId, string? ip, Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction, CancellationToken ct)
    {
        await RevokeFamilyAsync(session.RefreshTokenFamilyId, reason, DateTimeOffset.UtcNow, ct);
        await AuditAsync("IdentityRefresh", "FAILURE", session.UserId, session.CompanyId, session.BranchId,
            session.DeviceId, correlationId, reason, ip, ct);
        await transaction.CommitAsync(ct);
        throw InvalidRefresh();
    }

    private async Task RevokeAfterRefreshRaceAsync(string hash, Guid correlationId, string? ip, CancellationToken ct)
    {
        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
        var session = await LockSessionByRefreshHashAsync(hash, ct);
        if (session is null)
            await AuditAsync("IdentityRefresh", "FAILURE", null, null, null, null, correlationId, "REFRESH_TOKEN_INVALID", ip, ct);
        else
        {
            await RevokeFamilyAsync(session.RefreshTokenFamilyId, "REFRESH_TOKEN_REUSE", DateTimeOffset.UtcNow, ct);
            await AuditAsync("IdentityRefresh", "FAILURE", session.UserId, session.CompanyId, session.BranchId,
                session.DeviceId, correlationId, "REFRESH_TOKEN_REUSE", ip, ct);
        }
        await transaction.CommitAsync(ct);
    }

    private Task<AuthSession?> LockSessionByRefreshHashAsync(string hash, CancellationToken ct)
        => db.AuthSessions.FromSqlInterpolated(
            $"SELECT * FROM transport_erp.auth_sessions WHERE \"RefreshTokenHash\" = {hash} FOR UPDATE").SingleOrDefaultAsync(ct);

    private PasswordVerificationResult VerifyPassword(User? user, string password)
    {
        try { return user is null ? passwordHasher.VerifyHashedPassword(new User(), dummyHash, password) : passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password); }
        catch (FormatException) { return PasswordVerificationResult.Failed; }
    }

    private AuthSession NewSession(User user, Guid companyId, Guid? branchId, string deviceId,
        string refreshHash, Guid familyId, DateTimeOffset now) => new()
    {
        Id = Guid.NewGuid(), UserId = user.Id, CompanyId = companyId, BranchId = branchId, DeviceId = deviceId,
        Mode = "LOCAL", SecurityStampAtIssue = user.SecurityStamp, AuthVersionAtIssue = user.AuthVersion,
        RefreshTokenHash = refreshHash, RefreshTokenFamilyId = familyId, IssuedAt = now,
        AccessTokenExpiresAt = now.AddMinutes(settings.AccessTokenMinutes), RefreshTokenExpiresAt = now.AddDays(settings.RefreshTokenDays),
        CreatedAt = now, UpdatedAt = now, RowVersion = RandomNumberGenerator.GetBytes(16)
    };

    private IdentitySessionResponse ToResponse(AuthSession session, User user, string rawRefresh) => new(
        session.Id, CreateAccessToken(session, user), session.AccessTokenExpiresAt, rawRefresh,
        session.RefreshTokenExpiresAt, user.Id, user.DisplayName, session.CompanyId, session.BranchId, session.DeviceId);

    private string CreateAccessToken(AuthSession session, User user)
    {
        var claims = new List<Claim> { new(JwtRegisteredClaimNames.Sub, user.Id.ToString()), new("sid", session.Id.ToString()),
            new("company_id", session.CompanyId.ToString()), new("device_id", session.DeviceId),
            new("security_stamp", session.SecurityStampAtIssue), new("auth_version", session.AuthVersionAtIssue.ToString()) };
        if (session.BranchId.HasValue) claims.Add(new Claim("branch_id", session.BranchId.Value.ToString()));
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SigningKey)) { KeyId = settings.SigningKeyId };
        var descriptor = new SecurityTokenDescriptor { Subject = new ClaimsIdentity(claims), Issuer = settings.Issuer,
            Audience = settings.Audience, NotBefore = session.IssuedAt.UtcDateTime, Expires = session.AccessTokenExpiresAt.UtcDateTime,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256) };
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }

    private async Task RevokeFamilyAsync(Guid familyId, string reason, DateTimeOffset now, CancellationToken ct)
    {
        var active = await db.AuthSessions.Where(x => x.RefreshTokenFamilyId == familyId && x.RevokedAt == null).ToListAsync(ct);
        foreach (var item in active) { item.RevokedAt = now; item.RevokeReason = reason; item.UpdatedAt = now; item.RowVersion = RandomNumberGenerator.GetBytes(16); }
        if (active.Count > 0) await db.SaveChangesAsync(ct);
    }

    private async Task AuditAsync(string action, string outcome, Guid? userId, Guid? companyId, Guid? branchId,
        string? deviceId, Guid correlationId, string reason, string? ip, CancellationToken ct)
        => _ = await audit.AppendAuditEventAsync(new AuditEventDraft(action, outcome, nameof(AuthSession), ActorUserId: userId,
            CompanyId: companyId, BranchId: branchId, CorrelationId: correlationId, DeviceId: deviceId, Reason: reason, Ip: ip), ct);

    private static (string Raw, string Hash) NewRefreshToken() { var raw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)); return (raw, HashToken(raw)); }
    private static string HashToken(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token))).ToLowerInvariant();
    private static bool FixedEquals(string left, string right) => CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(left), Encoding.UTF8.GetBytes(right));
    public static string? NormalizeLogin(string? value) => string.IsNullOrWhiteSpace(value) || value.Trim().Length > 320 ? null : value.Trim().ToUpperInvariant();
    public static string? NormalizeDevice(string? value) => string.IsNullOrWhiteSpace(value) || value.Trim().Length > 120 ? null : value.Trim();
    public static string HashRefreshPartition(string? token)
        => TryNormalizeRefreshToken(token, out var normalized)
            ? HashToken(normalized)
            : HashToken(InvalidRefreshPartitionSeed);
    public static bool IsValidRefreshToken(string? token) => TryNormalizeRefreshToken(token, out _);
    private static bool TryNormalizeRefreshToken(string? token, out string normalized)
    {
        normalized = string.Empty;
        if (string.IsNullOrWhiteSpace(token)) return false;
        var value = token.Trim();
        if (value.Length > MaxRefreshTokenLength) return false;
        try
        {
            if (Convert.FromBase64String(value).Length != 32) return false;
        }
        catch (FormatException)
        {
            return false;
        }
        normalized = value;
        return true;
    }
    private static string LimitReason(string? value, string fallback) { var v = string.IsNullOrWhiteSpace(value) ? fallback : value.Trim(); return v[..Math.Min(v.Length, 200)]; }
    private static void Touch(User user, DateTimeOffset now) { user.UpdatedAt = now; user.RowVersion = RandomNumberGenerator.GetBytes(16); }
    private static IdentitySessionException InvalidCredentials() => new("INVALID_CREDENTIALS");
    private static IdentitySessionException InvalidRefresh() => new("REFRESH_TOKEN_INVALID");
    private void EnsureLocalMode() { if (settings.Mode != TransportAuthMode.LocalSessions) throw new IdentitySessionException("LOCAL_AUTH_DISABLED"); }
}
