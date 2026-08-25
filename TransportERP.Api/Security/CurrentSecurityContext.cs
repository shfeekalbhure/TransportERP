using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TransportERP.Contracts.Core;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Security;

public sealed record CurrentSecurityContext(
    Guid UserId, Guid CompanyId, Guid? BranchId, Guid? SessionId, string? DeviceId, bool IsLocalSession)
{
    public OperationContext ToOperationContext(Guid correlationId)
        => BranchId.HasValue
            ? new OperationContext(UserId, CompanyId, BranchId.Value, correlationId)
            : throw new InvalidOperationException("A branch-scoped operation requires a branch.");
}

public interface ICurrentSecurityContext
{
    Task<CurrentSecurityContext?> ResolveAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionAsync(CurrentSecurityContext context, string permissionCode,
        CancellationToken cancellationToken = default);
}

public sealed class CurrentSecurityContextService(
    TransportErpDbContext db,
    IEffectivePermissionResolver permissions,
    IOptions<TransportSecurityOptions> options) : ICurrentSecurityContext
{
    public async Task<CurrentSecurityContext?> ResolveAsync(ClaimsPrincipal principal,
        CancellationToken cancellationToken = default)
    {
        if (principal.Identity?.IsAuthenticated != true ||
            !TryGuid(principal, ClaimTypes.NameIdentifier, "sub", out var userId) ||
            !TryGuid(principal, "company_id", null, out var companyId)) return null;
        var branchId = TryGuid(principal, "branch_id", null, out var branch) ? branch : (Guid?)null;

        var user = await db.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == userId && x.Status == "ACTIVE",
            cancellationToken);
        if (user is null || (user.CompanyId.HasValue && user.CompanyId != companyId) ||
            (user.BranchId.HasValue && user.BranchId != branchId)) return null;
        if (!await db.Companies.AsNoTracking().AnyAsync(x => x.Id == companyId && x.Status == "ACTIVE", cancellationToken))
            return null;
        if (branchId.HasValue && !await db.Branches.AsNoTracking().AnyAsync(
                x => x.Id == branchId && x.CompanyId == companyId && x.Status == "ACTIVE", cancellationToken)) return null;

        if (!user.CompanyId.HasValue && !await permissions.HasPermissionAsync(
                user.Id, companyId, branchId, "auth.scope.select", cancellationToken)) return null;

        if (options.Value.Mode == TransportAuthMode.ExternalAuthority)
            return new CurrentSecurityContext(userId, companyId, branchId, null, principal.FindFirstValue("device_id"), false);

        if (!TryGuid(principal, "sid", JwtClaimTypes.SessionId, out var sessionId)) return null;
        var stamp = principal.FindFirstValue("security_stamp");
        if (string.IsNullOrEmpty(stamp) || !int.TryParse(principal.FindFirstValue("auth_version"), out var authVersion)) return null;
        var now = DateTimeOffset.UtcNow;
        var session = await db.AuthSessions.AsNoTracking().SingleOrDefaultAsync(x => x.Id == sessionId &&
            x.UserId == userId && x.CompanyId == companyId && x.BranchId == branchId && x.Mode == "LOCAL" &&
            x.RevokedAt == null && x.AccessTokenExpiresAt > now, cancellationToken);
        if (session is null) return null;
        if (authVersion != user.AuthVersion || authVersion != session.AuthVersionAtIssue ||
            !FixedEquals(stamp, user.SecurityStamp) || !FixedEquals(stamp, session.SecurityStampAtIssue)) return null;
        var tokenDevice = principal.FindFirstValue("device_id");
        if (!string.Equals(tokenDevice, session.DeviceId, StringComparison.Ordinal)) return null;
        return new CurrentSecurityContext(userId, companyId, branchId, sessionId, session.DeviceId, true);
    }

    public Task<bool> HasPermissionAsync(CurrentSecurityContext context, string permissionCode,
        CancellationToken cancellationToken = default)
        => permissions.HasPermissionAsync(context.UserId, context.CompanyId, context.BranchId, permissionCode, cancellationToken);

    private static bool TryGuid(ClaimsPrincipal principal, string first, string? second, out Guid value)
    {
        var raw = principal.FindFirstValue(first) ?? (second is null ? null : principal.FindFirstValue(second));
        return Guid.TryParse(raw, out value);
    }

    private static bool FixedEquals(string left, string right)
        => CryptographicOperations.FixedTimeEquals(System.Text.Encoding.UTF8.GetBytes(left), System.Text.Encoding.UTF8.GetBytes(right));
}

internal static class JwtClaimTypes
{
    public const string SessionId = "http://schemas.microsoft.com/ws/2008/06/identity/claims/sessionid";
}
