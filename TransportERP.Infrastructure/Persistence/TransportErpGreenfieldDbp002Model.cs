using Microsoft.EntityFrameworkCore;

namespace TransportERP.Infrastructure.Persistence;

internal static class TransportErpGreenfieldDbp002Model
{
    internal static void Configure(ModelBuilder mb)
    {
        var membership = mb.Entity<UserMembership>();
        membership.ToTable("user_memberships", t =>
        {
            t.HasCheckConstraint("ck_user_memberships_scope", "(\"ScopeType\" = 'COMPANY' AND \"BranchId\" IS NULL) OR (\"ScopeType\" = 'BRANCH' AND \"BranchId\" IS NOT NULL)");
            t.HasCheckConstraint("ck_user_memberships_status", "\"Status\" IN ('ACTIVE','SUSPENDED','REVOKED')");
            t.HasCheckConstraint("ck_user_memberships_security_version", "\"SecurityVersion\" >= 1");
            t.HasCheckConstraint("ck_user_memberships_valid_range", "\"ValidTo\" IS NULL OR \"ValidTo\" >= \"ValidFrom\"");
            t.HasCheckConstraint("ck_user_memberships_revoked_shape", "\"Status\" <> 'REVOKED' OR (\"ValidTo\" IS NOT NULL AND \"RevokedBy\" IS NOT NULL AND btrim(coalesce(\"RevokeReason\", '')) <> '')");
            t.HasCheckConstraint("ck_user_memberships_concurrency", "\"ConcurrencyVersion\" >= 1");
        });
        membership.HasKey(x => x.Id);
        membership.HasAlternateKey(x => new { x.Id, x.UserId, x.CompanyId });
        membership.Property(x => x.ScopeType).HasMaxLength(12).IsRequired();
        membership.Property(x => x.Status).HasMaxLength(12).IsRequired();
        membership.Property(x => x.ValidFrom).HasColumnType("timestamptz");
        membership.Property(x => x.ValidTo).HasColumnType("timestamptz");
        membership.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        membership.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        membership.Property(x => x.RevokeReason).HasMaxLength(500);
        membership.HasOne<User>().WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        membership.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
        membership.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId }).HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);
        membership.HasOne<User>().WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Restrict);
        membership.HasOne<User>().WithMany().HasForeignKey(x => x.RevokedBy).OnDelete(DeleteBehavior.Restrict);
        membership.HasIndex(x => new { x.UserId, x.CompanyId, x.BranchId }).IsUnique().AreNullsDistinct(false);
        membership.HasIndex(x => new { x.UserId, x.Status, x.CompanyId });
        membership.HasIndex(x => new { x.CompanyId, x.BranchId, x.Status });
        membership.HasIndex(x => new { x.CompanyId, x.UpdatedAt });

        var roleGrant = mb.Entity<UserRoleGrant>();
        roleGrant.ToTable("user_role_grants", t =>
        {
            t.HasCheckConstraint("ck_user_role_grants_status", "\"Status\" IN ('ACTIVE','SUSPENDED','REVOKED')");
            t.HasCheckConstraint("ck_user_role_grants_valid_range", "\"ValidTo\" IS NULL OR \"ValidTo\" >= \"ValidFrom\"");
            t.HasCheckConstraint("ck_user_role_grants_concurrency", "\"ConcurrencyVersion\" >= 1");
            t.HasCheckConstraint("ck_user_role_grants_revoke_shape", "\"Status\" <> 'REVOKED' OR (\"ValidTo\" IS NOT NULL AND \"RevokedBy\" IS NOT NULL AND btrim(coalesce(\"Reason\", '')) <> '')");
        });
        roleGrant.HasKey(x => x.Id);
        roleGrant.Property(x => x.Status).HasMaxLength(12).IsRequired();
        roleGrant.Property(x => x.ValidFrom).HasColumnType("timestamptz");
        roleGrant.Property(x => x.ValidTo).HasColumnType("timestamptz");
        roleGrant.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        roleGrant.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        roleGrant.Property(x => x.Reason).HasMaxLength(500);
        roleGrant.HasOne<UserMembership>().WithMany().HasForeignKey(x => new { x.MembershipId, x.UserId, x.CompanyId }).HasPrincipalKey(x => new { x.Id, x.UserId, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);
        roleGrant.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId }).HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);
        roleGrant.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Restrict);
        roleGrant.HasOne<User>().WithMany().HasForeignKey(x => x.GrantedBy).OnDelete(DeleteBehavior.Restrict);
        roleGrant.HasOne<User>().WithMany().HasForeignKey(x => x.RevokedBy).OnDelete(DeleteBehavior.Restrict);
        roleGrant.HasIndex(x => new { x.MembershipId, x.RoleId }).IsUnique().HasFilter("\"Status\" = 'ACTIVE'");
        roleGrant.HasIndex(x => new { x.CompanyId, x.BranchId, x.Status });
        roleGrant.HasIndex(x => new { x.UserId, x.Status });

        var permissionGrant = mb.Entity<UserPermissionGrant>();
        permissionGrant.ToTable("user_permission_grants", t =>
        {
            t.HasCheckConstraint("ck_user_permission_grants_effect", "\"Effect\" IN ('ALLOW','DENY')");
            t.HasCheckConstraint("ck_user_permission_grants_status", "\"Status\" IN ('ACTIVE','SUSPENDED','REVOKED')");
            t.HasCheckConstraint("ck_user_permission_grants_valid_range", "\"ValidTo\" IS NULL OR \"ValidTo\" >= \"ValidFrom\"");
            t.HasCheckConstraint("ck_user_permission_grants_concurrency", "\"ConcurrencyVersion\" >= 1");
            t.HasCheckConstraint("ck_user_permission_grants_revoke_shape", "\"Status\" <> 'REVOKED' OR (\"ValidTo\" IS NOT NULL AND \"RevokedBy\" IS NOT NULL AND btrim(coalesce(\"Reason\", '')) <> '')");
        });
        permissionGrant.HasKey(x => x.Id);
        permissionGrant.Property(x => x.Effect).HasMaxLength(5).IsRequired();
        permissionGrant.Property(x => x.Status).HasMaxLength(12).IsRequired();
        permissionGrant.Property(x => x.ValidFrom).HasColumnType("timestamptz");
        permissionGrant.Property(x => x.ValidTo).HasColumnType("timestamptz");
        permissionGrant.Property(x => x.CreatedAt).HasColumnType("timestamptz");
        permissionGrant.Property(x => x.UpdatedAt).HasColumnType("timestamptz");
        permissionGrant.Property(x => x.Reason).HasMaxLength(500);
        permissionGrant.HasOne<UserMembership>().WithMany().HasForeignKey(x => new { x.MembershipId, x.UserId, x.CompanyId }).HasPrincipalKey(x => new { x.Id, x.UserId, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);
        permissionGrant.HasOne<Branch>().WithMany().HasForeignKey(x => new { x.BranchId, x.CompanyId }).HasPrincipalKey(x => new { x.Id, x.CompanyId }).OnDelete(DeleteBehavior.Restrict);
        permissionGrant.HasOne<Permission>().WithMany().HasForeignKey(x => x.PermissionId).OnDelete(DeleteBehavior.Restrict);
        permissionGrant.HasOne<User>().WithMany().HasForeignKey(x => x.GrantedBy).OnDelete(DeleteBehavior.Restrict);
        permissionGrant.HasOne<User>().WithMany().HasForeignKey(x => x.RevokedBy).OnDelete(DeleteBehavior.Restrict);
        permissionGrant.HasIndex(x => new { x.MembershipId, x.PermissionId }).IsUnique().HasFilter("\"Status\" = 'ACTIVE'");
        permissionGrant.HasIndex(x => new { x.CompanyId, x.BranchId, x.Status });
        permissionGrant.HasIndex(x => new { x.UserId, x.Status });
    }
}
