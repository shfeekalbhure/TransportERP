using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TransportERP.Application.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Security;

public interface ICurrentRequestSecurityResolver
{
    Task<RequestSecurityResolution> ResolveAsync(
        HttpContext http,
        string permissionCode,
        CancellationToken cancellationToken = default);
}

public sealed record RequestSecurityResolution(OperationContext? Context, IResult? Failure)
{
    public bool Succeeded => Context is not null && Failure is null;
}

/// <summary>
/// Reconciles authenticated claim selectors with current persistent user, tenant
/// and RBAC state. Claims may narrow a request but never establish or widen its
/// server authority.
/// </summary>
public sealed class CurrentRequestSecurityResolver(
    TransportErpDbContext db,
    IEffectivePermissionResolver permissions) : ICurrentRequestSecurityResolver
{
    public async Task<RequestSecurityResolution> ResolveAsync(
        HttpContext http,
        string permissionCode,
        CancellationToken cancellationToken = default)
    {
        if (http.User.Identity?.IsAuthenticated != true)
            return new(null, Results.Unauthorized());

        if (!TryGuid(http.User, ClaimTypes.NameIdentifier, "sub", out var userId) ||
            !TryGuid(http.User, "company_id", null, out var companyId) ||
            !TryGuid(http.User, "branch_id", null, out var branchId))
            return new(null, Results.Unauthorized());

        var correlationId = Guid.TryParse(
            http.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var parsed)
                ? parsed
                : Guid.NewGuid();
        var context = new OperationContext(userId, companyId, branchId, correlationId);

        // Preserve the current token contract as a narrowing hint, but require
        // the persistent decision below before any product action runs.
        if (!HasPermissionHint(http.User, permissionCode))
            return Denied(context);

        var currentScopeExists = await db.Users.AsNoTracking().AnyAsync(x =>
                x.Id == userId && x.Status == "ACTIVE" &&
                x.CompanyId == companyId &&
                (!x.BranchId.HasValue || x.BranchId == branchId), cancellationToken) &&
            await db.Companies.AsNoTracking().AnyAsync(
                x => x.Id == companyId && x.Status == "ACTIVE", cancellationToken) &&
            await db.Branches.AsNoTracking().AnyAsync(
                x => x.Id == branchId && x.CompanyId == companyId && x.Status == "ACTIVE",
                cancellationToken);
        if (!currentScopeExists)
            return Denied(context);

        if (!await permissions.HasPermissionAsync(
                userId, companyId, branchId, permissionCode, cancellationToken))
            return Denied(context);

        return new(context, null);
    }

    private static RequestSecurityResolution Denied(OperationContext context)
        => new(null, Results.Json(
            new { ErrorCode = "SCOPE_DENIED", context.CorrelationId },
            statusCode: StatusCodes.Status403Forbidden));

    private static bool TryGuid(
        ClaimsPrincipal principal,
        string first,
        string? second,
        out Guid value)
    {
        var raw = principal.FindFirstValue(first) ??
            (second is null ? null : principal.FindFirstValue(second));
        return Guid.TryParse(raw, out value);
    }

    private static bool HasPermissionHint(ClaimsPrincipal principal, string permission)
        => principal.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role &&
            string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));
}
