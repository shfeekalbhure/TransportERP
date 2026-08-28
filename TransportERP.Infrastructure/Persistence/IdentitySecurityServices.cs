using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

public interface IEffectivePermissionResolver
{
    Task<bool> HasPermissionAsync(Guid userId, Guid companyId, Guid? branchId, string permissionCode,
        CancellationToken cancellationToken = default);
}

public sealed class EffectivePermissionResolver(TransportErpDbContext db) : IEffectivePermissionResolver
{
    public async Task<bool> HasPermissionAsync(Guid userId, Guid companyId, Guid? branchId, string permissionCode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(permissionCode)) return false;

        var userActive = await db.Users.AsNoTracking().AnyAsync(x => x.Id == userId && x.Status == "ACTIVE" &&
            (!x.CompanyId.HasValue || x.CompanyId == companyId) &&
            (!x.BranchId.HasValue || x.BranchId == branchId), cancellationToken);
        if (!userActive) return false;

        var permission = await db.Permissions.AsNoTracking()
            .Where(x => x.Code == permissionCode && x.Status == "ACTIVE")
            .Select(x => new { x.Id, x.ScopeType })
            .SingleOrDefaultAsync(cancellationToken);
        if (permission is null || permission.ScopeType is not ("PLATFORM" or "COMPANY" or "BRANCH")) return false;

        static bool Applies(Guid? scopedCompany, Guid? scopedBranch, Guid company, Guid? branch) =>
            (!scopedCompany.HasValue || scopedCompany == company) &&
            (!scopedBranch.HasValue || scopedBranch == branch);

        var overrides = await db.UserPermissionOverrides.AsNoTracking()
            .Where(x => x.UserId == userId && x.PermissionId == permission.Id)
            .Select(x => new { x.IsAllowed, x.CompanyId, x.BranchId })
            .ToListAsync(cancellationToken);
        if (overrides.Any(x => x.BranchId.HasValue && !x.CompanyId.HasValue)) return false;
        if (!await BranchScopesAreValidAsync(overrides.Where(x => x.BranchId.HasValue)
                .Select(x => new ScopePair(x.CompanyId!.Value, x.BranchId!.Value)), cancellationToken)) return false;
        var applicableOverrides = overrides.Where(x => Applies(x.CompanyId, x.BranchId, companyId, branchId)).ToArray();
        if (applicableOverrides.Any(x => !x.IsAllowed)) return false;
        if (applicableOverrides.Any(x => x.IsAllowed &&
                OverrideScope(x.CompanyId, x.BranchId) == permission.ScopeType)) return true;

        var grants = await (
            from ur in db.UserRoles.AsNoTracking()
            join role in db.Roles.AsNoTracking() on ur.RoleId equals role.Id
            join rp in db.RolePermissions.AsNoTracking() on role.Id equals rp.RoleId
            where ur.UserId == userId && role.Status == "ACTIVE" && rp.PermissionId == permission.Id
            select new { UserCompany = ur.CompanyId, UserBranch = ur.BranchId, RoleCompany = role.CompanyId,
                GrantScope = rp.ScopeType, GrantCompany = rp.CompanyId, GrantBranch = rp.BranchId })
            .ToListAsync(cancellationToken);

        if (grants.Any(x => x.UserBranch.HasValue && !x.UserCompany.HasValue)) return false;
        if (!await BranchScopesAreValidAsync(grants.Where(x => x.UserBranch.HasValue)
                .Select(x => new ScopePair(x.UserCompany!.Value, x.UserBranch!.Value)), cancellationToken)) return false;
        if (!await BranchScopesAreValidAsync(grants.Where(x => x.GrantBranch.HasValue)
                .Select(x => new ScopePair(x.GrantCompany ?? Guid.Empty, x.GrantBranch!.Value)), cancellationToken)) return false;

        return grants.Any(x =>
            x.GrantScope == permission.ScopeType && ScopeShapeValid(x.GrantScope, x.GrantCompany, x.GrantBranch) &&
            Applies(x.UserCompany, x.UserBranch, companyId, branchId) &&
            (!x.RoleCompany.HasValue || x.RoleCompany == companyId) &&
            Applies(x.GrantCompany, x.GrantBranch, companyId, branchId));
    }

    private static bool ScopeShapeValid(string scopeType, Guid? companyId, Guid? branchId) => scopeType switch
    {
        "PLATFORM" => !companyId.HasValue && !branchId.HasValue,
        "COMPANY" => companyId.HasValue && !branchId.HasValue,
        "BRANCH" => companyId.HasValue && branchId.HasValue,
        _ => false
    };

    private static string? OverrideScope(Guid? companyId, Guid? branchId)
        => branchId.HasValue ? companyId.HasValue ? "BRANCH" : null
            : companyId.HasValue ? "COMPANY" : "PLATFORM";

    private async Task<bool> BranchScopesAreValidAsync(IEnumerable<ScopePair> pairs, CancellationToken ct)
    {
        var distinct = pairs.Distinct().ToArray();
        if (distinct.Length == 0) return true;
        foreach (var pair in distinct)
            if (!await db.Branches.AsNoTracking().AnyAsync(x => x.Id == pair.BranchId && x.CompanyId == pair.CompanyId, ct))
                return false;
        return true;
    }

    private sealed record ScopePair(Guid CompanyId, Guid BranchId);
}

public sealed record ValidatedSecurityScope(User User, Company Company, Branch? Branch);

public sealed class TenantScopeResolver(TransportErpDbContext db, IEffectivePermissionResolver permissions)
{
    public async Task<ValidatedSecurityScope?> ResolveAsync(User user, Guid? requestedCompanyId, Guid? requestedBranchId,
        CancellationToken cancellationToken = default)
    {
        var companyId = user.CompanyId ?? requestedCompanyId;
        if (!companyId.HasValue) return null;
        if (user.CompanyId.HasValue && requestedCompanyId.HasValue && requestedCompanyId != user.CompanyId) return null;

        var branchId = user.BranchId ?? requestedBranchId;
        if (user.BranchId.HasValue && requestedBranchId.HasValue && requestedBranchId != user.BranchId) return null;

        var company = await db.Companies.AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == companyId && x.Status == "ACTIVE", cancellationToken);
        if (company is null) return null;
        Branch? branch = null;
        if (branchId.HasValue)
        {
            branch = await db.Branches.AsNoTracking().SingleOrDefaultAsync(
                x => x.Id == branchId && x.CompanyId == companyId && x.Status == "ACTIVE", cancellationToken);
            if (branch is null) return null;
        }

        if (!user.CompanyId.HasValue && !await permissions.HasPermissionAsync(
                user.Id, companyId.Value, branchId, "auth.scope.select", cancellationToken)) return null;

        return new ValidatedSecurityScope(user, company, branch);
    }
}
