using Microsoft.EntityFrameworkCore;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

internal static class PersistentRbacTestSeeder
{
    public static async Task GrantBranchPermissionAsync(
        TransportErpDbContext db,
        Guid userId,
        Guid companyId,
        Guid branchId,
        string permissionCode)
    {
        var now = DateTimeOffset.UtcNow;
        var permission = await db.Permissions.SingleOrDefaultAsync(x => x.Code == permissionCode);
        if (permission is null)
        {
            permission = new Permission
            {
                Id = Guid.NewGuid(),
                Code = permissionCode,
                NameAr = $"اختبار {permissionCode}",
                Resource = "test",
                Action = "execute",
                ScopeType = "BRANCH",
                IsSystem = true,
                Status = "ACTIVE",
                CreatedAt = now,
                UpdatedAt = now,
                RowVersion = Guid.NewGuid().ToByteArray()
            };
            db.Permissions.Add(permission);
        }
        else if (permission.Status != "ACTIVE" || permission.ScopeType != "BRANCH")
        {
            throw new InvalidOperationException(
                $"Test permission {permissionCode} has incompatible persistent scope/status.");
        }

        var membership = await db.Set<UserMembership>()
            .SingleOrDefaultAsync(x => x.UserId == userId && x.CompanyId == companyId && x.BranchId == branchId);
        if (membership is null)
        {
            membership = new UserMembership
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CompanyId = companyId,
                BranchId = branchId,
                ScopeType = "BRANCH",
                Status = "ACTIVE",
                SecurityVersion = 1,
                ValidFrom = now,
                CreatedAt = now,
                UpdatedAt = now,
                CreatedBy = userId,
                ConcurrencyVersion = 1
            };
            db.Set<UserMembership>().Add(membership);
        }

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Code = $"TEST-{Guid.NewGuid():N}",
            NameAr = "دور اختبار RBAC",
            IsSystem = false,
            CompanyId = companyId,
            Status = "ACTIVE",
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        db.Roles.Add(role);
        db.RolePermissions.Add(new RolePermission
        {
            RoleId = role.Id,
            PermissionId = permission.Id,
            ScopeType = "BRANCH",
            CompanyId = companyId,
            BranchId = branchId,
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        });
        db.Set<UserRoleGrant>().Add(new UserRoleGrant
        {
            Id = Guid.NewGuid(),
            MembershipId = membership.Id,
            UserId = userId,
            CompanyId = companyId,
            BranchId = branchId,
            RoleId = role.Id,
            Status = "ACTIVE",
            ValidFrom = now,
            GrantedBy = userId,
            CreatedAt = now,
            UpdatedAt = now,
            ConcurrencyVersion = 1
        });
        await db.SaveChangesAsync();
    }
}
