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
/// Resolves only the authoritative Greenfield membership-scoped grants. Legacy
/// user_roles and user_permission_overrides remain physically preserved but are
/// intentionally not request-time authorization sources.
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
        if (userId == Guid.Empty || companyId == Guid.Empty || string.IsNullOrWhiteSpace(permissionCode))
            return false;

        var now = DateTimeOffset.UtcNow;
        var permission = await db.Permissions.AsNoTracking()
            .Where(x => x.Code == permissionCode && x.Status == "ACTIVE")
            .Select(x => new { x.Id, x.ScopeType })
            .SingleOrDefaultAsync(cancellationToken);
        if (permission is null || !IsKnownScope(permission.ScopeType)) return false;

        // The current API contract has no client-authoritative membership id. Fail
        // closed by selecting only an exact persistent scope row; W2 may expose an
        // explicit server-validated membership selector without widening this rule.
        var memberships = await db.Set<UserMembership>().AsNoTracking()
            .Where(x => x.UserId == userId && x.CompanyId == companyId &&
                        x.Status == "ACTIVE" && x.ValidFrom <= now &&
                        (x.ValidTo == null || x.ValidTo >= now) &&
                        x.BranchId == branchId)
            .Select(x => new { x.Id, x.ScopeType, x.SecurityVersion, x.BranchId })
            .Take(2)
            .ToListAsync(cancellationToken);

        if (memberships.Count != 1) return false;
        var membership = memberships[0];
        if (membership.SecurityVersion < 1 ||
            (membership.ScopeType == "COMPANY") != (membership.BranchId is null) ||
            (membership.ScopeType == "BRANCH") != (membership.BranchId is not null))
            return false;

        var direct = await db.Set<UserPermissionGrant>().AsNoTracking()
            .Where(x => x.MembershipId == membership.Id && x.UserId == userId &&
                        x.CompanyId == companyId && x.BranchId == branchId &&
                        x.PermissionId == permission.Id && x.Status == "ACTIVE" &&
                        x.ValidFrom <= now && (x.ValidTo == null || x.ValidTo >= now))
            .Select(x => x.Effect)
            .ToListAsync(cancellationToken);

        if (direct.Any(x => x == "DENY")) return false;
        if (direct.Any(x => x == "ALLOW")) return true;
        if (direct.Any(x => x is not ("ALLOW" or "DENY"))) return false;

        return await (
            from grant in db.Set<UserRoleGrant>().AsNoTracking()
            join role in db.Roles.AsNoTracking() on grant.RoleId equals role.Id
            join rolePermission in db.RolePermissions.AsNoTracking() on role.Id equals rolePermission.RoleId
            where grant.MembershipId == membership.Id && grant.UserId == userId &&
                  grant.CompanyId == companyId && grant.BranchId == branchId &&
                  grant.Status == "ACTIVE" && grant.ValidFrom <= now &&
                  (grant.ValidTo == null || grant.ValidTo >= now) &&
                  role.Status == "ACTIVE" &&
                  (!role.CompanyId.HasValue || role.CompanyId == companyId) &&
                  rolePermission.PermissionId == permission.Id &&
                  rolePermission.ScopeType == permission.ScopeType
            select grant.Id)
            .AnyAsync(cancellationToken);
    }

    private static bool IsKnownScope(string scopeType)
        => scopeType is "PLATFORM" or "COMPANY" or "BRANCH";
}
