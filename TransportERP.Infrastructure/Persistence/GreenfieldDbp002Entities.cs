namespace TransportERP.Infrastructure.Persistence;

/// <summary>
/// Authoritative Greenfield tenant membership. Legacy User.CompanyId/BranchId stays
/// physically present during MISSION-03 but is not request-time authorization.
/// </summary>
public sealed class UserMembership
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public string ScopeType { get; set; } = "COMPANY";
    public string Status { get; set; } = "ACTIVE";
    public long SecurityVersion { get; set; } = 1;
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? RevokedBy { get; set; }
    public string? RevokeReason { get; set; }
    public long ConcurrencyVersion { get; set; } = 1;
}

public sealed class UserRoleGrant
{
    public Guid Id { get; set; }
    public Guid MembershipId { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid RoleId { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
    public Guid GrantedBy { get; set; }
    public Guid? RevokedBy { get; set; }
    public string? Reason { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long ConcurrencyVersion { get; set; } = 1;
}

public sealed class UserPermissionGrant
{
    public Guid Id { get; set; }
    public Guid MembershipId { get; set; }
    public Guid UserId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? BranchId { get; set; }
    public Guid PermissionId { get; set; }
    public string Effect { get; set; } = "DENY";
    public string Status { get; set; } = "ACTIVE";
    public DateTimeOffset ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
    public Guid GrantedBy { get; set; }
    public Guid? RevokedBy { get; set; }
    public string? Reason { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public long ConcurrencyVersion { get; set; } = 1;
}
