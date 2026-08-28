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
        db.UserRoles.Add(new UserRole
        {
            UserId = userId,
            RoleId = role.Id,
            CompanyId = companyId,
            BranchId = branchId,
            CreatedAt = now,
            UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        });
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
        await db.SaveChangesAsync();
    }
}
