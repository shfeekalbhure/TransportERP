using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

public interface IEffectivePermissionResolver
{
    Task<bool> HasPermissionAsync(
        Guid userId,
        Guid companyId,
        Guid? branchId,
        string permissionCode,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Resolves the existing persistent RBAC model at request time. Token permission
/// claims can narrow an API request, but cannot replace this decision or widen it.
/// Malformed or tenant-inconsistent scope rows fail closed.
/// </summary>
public sealed class PersistentPermissionResolver(TransportErpDbContext db) : IEffectivePermissionResolver
{
    public async Task<bool> HasPermissionAsync(
        Guid userId,
        Guid companyId,
        Guid? branchId,
        string permissionCode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(permissionCode)) return false;

        var permission = await db.Permissions.AsNoTracking()
            .Where(x => x.Code == permissionCode && x.Status == "ACTIVE")
            .Select(x => new { x.Id, x.ScopeType })
            .SingleOrDefaultAsync(cancellationToken);
        if (permission is null || !IsKnownScope(permission.ScopeType)) return false;

        var overrides = await db.UserPermissionOverrides.AsNoTracking()
            .Where(x => x.UserId == userId && x.PermissionId == permission.Id)
            .Select(x => new ScopedDecision(x.CompanyId, x.BranchId, x.IsAllowed))
            .ToListAsync(cancellationToken);
        if (!await ScopeRowsAreValidAsync(overrides, cancellationToken)) return false;

        var applicableOverrides = overrides
            .Where(x => AppliesToContext(x.CompanyId, x.BranchId, companyId, branchId))
            .ToArray();
        if (applicableOverrides.Any(x => !x.IsAllowed)) return false;
        if (applicableOverrides.Any(x => x.IsAllowed &&
                ScopeType(x.CompanyId, x.BranchId) == permission.ScopeType))
            return true;

        var grants = await (
            from userRole in db.UserRoles.AsNoTracking()
            join role in db.Roles.AsNoTracking() on userRole.RoleId equals role.Id
            join rolePermission in db.RolePermissions.AsNoTracking() on role.Id equals rolePermission.RoleId
            where userRole.UserId == userId && role.Status == "ACTIVE" &&
                  rolePermission.PermissionId == permission.Id
            select new PermissionGrant(
                userRole.CompanyId,
                userRole.BranchId,
                role.CompanyId,
                rolePermission.ScopeType,
                rolePermission.CompanyId,
                rolePermission.BranchId))
            .ToListAsync(cancellationToken);

        if (!await ScopeRowsAreValidAsync(
                grants.Select(x => new ScopedDecision(x.UserCompanyId, x.UserBranchId, true)),
                cancellationToken) ||
            !await ScopeRowsAreValidAsync(
                grants.Select(x => new ScopedDecision(x.GrantCompanyId, x.GrantBranchId, true)),
                cancellationToken))
            return false;

        return grants.Any(x =>
            x.GrantScopeType == permission.ScopeType &&
            ScopeType(x.GrantCompanyId, x.GrantBranchId) == x.GrantScopeType &&
            AppliesToContext(x.UserCompanyId, x.UserBranchId, companyId, branchId) &&
            (!x.RoleCompanyId.HasValue || x.RoleCompanyId == companyId) &&
            AppliesToContext(x.GrantCompanyId, x.GrantBranchId, companyId, branchId));
    }

    private async Task<bool> ScopeRowsAreValidAsync(
        IEnumerable<ScopedDecision> rows,
        CancellationToken cancellationToken)
    {
        var materialized = rows.ToArray();
        if (materialized.Any(x => x.BranchId.HasValue && !x.CompanyId.HasValue)) return false;

        foreach (var scope in materialized
                     .Where(x => x.BranchId.HasValue)
                     .Select(x => new { CompanyId = x.CompanyId!.Value, BranchId = x.BranchId!.Value })
                     .Distinct())
        {
            if (!await db.Branches.AsNoTracking().AnyAsync(
                    x => x.Id == scope.BranchId && x.CompanyId == scope.CompanyId,
                    cancellationToken))
                return false;
        }

        return true;
    }

    private static bool AppliesToContext(
        Guid? scopedCompanyId,
        Guid? scopedBranchId,
        Guid companyId,
        Guid? branchId)
        => (!scopedCompanyId.HasValue || scopedCompanyId == companyId) &&
           (!scopedBranchId.HasValue || scopedBranchId == branchId);

    private static bool IsKnownScope(string scopeType)
        => scopeType is "PLATFORM" or "COMPANY" or "BRANCH";

    private static string? ScopeType(Guid? companyId, Guid? branchId)
        => branchId.HasValue
            ? companyId.HasValue ? "BRANCH" : null
            : companyId.HasValue ? "COMPANY" : "PLATFORM";

    private sealed record ScopedDecision(Guid? CompanyId, Guid? BranchId, bool IsAllowed);

    private sealed record PermissionGrant(
        Guid? UserCompanyId,
        Guid? UserBranchId,
        Guid? RoleCompanyId,
        string GrantScopeType,
        Guid? GrantCompanyId,
        Guid? GrantBranchId);
}
