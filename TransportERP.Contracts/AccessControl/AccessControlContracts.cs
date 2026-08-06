namespace TransportERP.Contracts.AccessControl;

public enum AccessControlStorageState
{
    Available = 0,
    ApprovedStorageUnavailable = 1
}

public sealed record PagedQuery(int Page = 1, int PageSize = 50, string? Search = null, string? Status = null);
public sealed record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount, AccessControlStorageState StorageState, string? Blocker = null);

public sealed record RoleDto(string Id, string Code, string NameAr, string? NameEn, string Status, int UserCount, DateTimeOffset UpdatedAt);
public sealed record PermissionDto(string Id, string RoleId, string Module, string Screen, string AccessLevel, string Status);
public sealed record DataScopeDto(string Id, string SubjectType, string SubjectId, string CompanyId, string? BranchId, string? OrganizationalUnitId, bool CanRead, bool CanEdit, string Status);

public sealed record CreateRoleRequest(string Code, string NameAr, string? NameEn, string? Description);
public sealed record UpdateRoleRequest(string NameAr, string? NameEn, string? Description, string Status);
public sealed record CreatePermissionRequest(string RoleId, string Module, string Screen, string AccessLevel);
public sealed record CreateDataScopeRequest(string SubjectType, string SubjectId, string CompanyId, string? BranchId, string? OrganizationalUnitId, bool CanRead, bool CanEdit);

public sealed record OperationResult(bool Succeeded, string? ErrorCode = null, string? Message = null)
{
    public static OperationResult StorageBlocked() => new(false, "APPROVED_STORAGE_UNAVAILABLE", "مانع التخزين المعتمد: لا توجد طبقة تخزين معتمدة لهذه البيانات.");
}