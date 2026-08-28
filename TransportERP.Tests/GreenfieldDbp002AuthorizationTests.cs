using Microsoft.EntityFrameworkCore;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Tests;

[Collection("PostgreSql")]
public sealed class GreenfieldDbp002AuthorizationTests
{
    [Fact]
    public async Task Legacy_role_assignment_is_not_authority_and_new_membership_grants_are_authoritative()
    {
        var connection = PostgreSqlTestEnvironment.RequireConnection();
        await using var db = PostgreSqlTestEnvironment.CreateDbContext(connection);
        await db.Database.MigrateAsync();

        var now = DateTimeOffset.UtcNow;
        var currency = new Currency
        {
            Id = Guid.NewGuid(), Code = $"C{Guid.NewGuid():N}"[..3].ToUpperInvariant(), NameAr = "DBP002 Currency",
            MinorUnit = 2, IsBase = true, Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var company = new Company
        {
            Id = Guid.NewGuid(), Code = $"D2-{Guid.NewGuid():N}"[..18], LegalNameAr = "DBP002 Company",
            BaseCurrencyId = currency.Id, DefaultCalendarId = Guid.NewGuid(), Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var branch = new Branch
        {
            Id = Guid.NewGuid(), CompanyId = company.Id, Code = $"B{Guid.NewGuid():N}"[..12], NameAr = "DBP002 Branch",
            Timezone = "Asia/Aden", Status = "ACTIVE", CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var user = new User
        {
            Id = Guid.NewGuid(), UserName = $"dbp002-{Guid.NewGuid():N}", NormalizedUserName = $"DBP002{Guid.NewGuid():N}".ToUpperInvariant(),
            DisplayName = "DBP002 User", PasswordHash = "test-only", Status = "ACTIVE",
            CompanyId = company.Id, BranchId = branch.Id, CreatedAt = now, UpdatedAt = now,
            RowVersion = Guid.NewGuid().ToByteArray()
        };
        var permission = new Permission
        {
            Id = Guid.NewGuid(), Code = $"dbp002.permission.{Guid.NewGuid():N}", NameAr = "DBP002 Permission",
            Resource = "dbp002", Action = "read", ScopeType = "BRANCH", IsSystem = false, Status = "ACTIVE",
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };
        var role = new Role
        {
            Id = Guid.NewGuid(), Code = $"DBP002-{Guid.NewGuid():N}"[..24], NameAr = "DBP002 Role", CompanyId = company.Id,
            Status = "ACTIVE", CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        };

        db.AddRange(currency, company, branch, user, permission, role);
        db.UserRoles.Add(new UserRole
        {
            UserId = user.Id, RoleId = role.Id, CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        });
        db.RolePermissions.Add(new RolePermission
        {
            RoleId = role.Id, PermissionId = permission.Id, ScopeType = "BRANCH", CompanyId = company.Id, BranchId = branch.Id,
            CreatedAt = now, UpdatedAt = now, RowVersion = Guid.NewGuid().ToByteArray()
        });
        await db.SaveChangesAsync();

        var resolver = new PersistentPermissionResolver(db);
        Assert.False(await resolver.HasPermissionAsync(user.Id, company.Id, branch.Id, permission.Code));

        var membership = new UserMembership
        {
            Id = Guid.NewGuid(), UserId = user.Id, CompanyId = company.Id, BranchId = branch.Id,
            ScopeType = "BRANCH", Status = "ACTIVE", SecurityVersion = 1, ValidFrom = now.AddMinutes(-1),
            CreatedAt = now, UpdatedAt = now, CreatedBy = user.Id, ConcurrencyVersion = 1
        };
        db.Set<UserMembership>().Add(membership);
        db.Set<UserRoleGrant>().Add(new UserRoleGrant
        {
            Id = Guid.NewGuid(), MembershipId = membership.Id, UserId = user.Id, CompanyId = company.Id, BranchId = branch.Id,
            RoleId = role.Id, Status = "ACTIVE", ValidFrom = now.AddMinutes(-1), GrantedBy = user.Id,
            CreatedAt = now, UpdatedAt = now, ConcurrencyVersion = 1
        });
        await db.SaveChangesAsync();

        Assert.True(await resolver.HasPermissionAsync(user.Id, company.Id, branch.Id, permission.Code));

        db.Set<UserPermissionGrant>().Add(new UserPermissionGrant
        {
            Id = Guid.NewGuid(), MembershipId = membership.Id, UserId = user.Id, CompanyId = company.Id, BranchId = branch.Id,
            PermissionId = permission.Id, Effect = "DENY", Status = "ACTIVE", ValidFrom = now.AddMinutes(-1),
            GrantedBy = user.Id, CreatedAt = now, UpdatedAt = now, ConcurrencyVersion = 1
        });
        await db.SaveChangesAsync();

        Assert.False(await resolver.HasPermissionAsync(user.Id, company.Id, branch.Id, permission.Code));
    }
}
