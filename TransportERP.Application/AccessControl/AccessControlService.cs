using TransportERP.Contracts.AccessControl;

namespace TransportERP.Application.AccessControl;

public interface IAccessControlService
{
    Task<PagedResult<RoleDto>> SearchRolesAsync(PagedQuery query, CancellationToken cancellationToken = default);
    Task<PagedResult<PermissionDto>> SearchPermissionsAsync(PagedQuery query, CancellationToken cancellationToken = default);
    Task<PagedResult<DataScopeDto>> SearchDataScopesAsync(PagedQuery query, CancellationToken cancellationToken = default);
    Task<OperationResult> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);
    Task<OperationResult> CreatePermissionAsync(CreatePermissionRequest request, CancellationToken cancellationToken = default);
    Task<OperationResult> CreateDataScopeAsync(CreateDataScopeRequest request, CancellationToken cancellationToken = default);
}

/// <summary>مسار تشغيلي حقيقي بلا محاكاة: يصرح بغياب التخزين المعتمد حتى تُعتمد طبقة الحفظ.</summary>
public sealed class ApprovedStorageBlockedAccessControlService : IAccessControlService
{
    private const string Blocker = "مانع التخزين المعتمد: لا توجد طبقة تخزين معتمدة لخدمات الأدوار والصلاحيات ونطاقات الوصول.";

    public Task<PagedResult<RoleDto>> SearchRolesAsync(PagedQuery query, CancellationToken cancellationToken = default) =>
        Task.FromResult(new PagedResult<RoleDto>([], query.Page, query.PageSize, 0, AccessControlStorageState.ApprovedStorageUnavailable, Blocker));

    public Task<PagedResult<PermissionDto>> SearchPermissionsAsync(PagedQuery query, CancellationToken cancellationToken = default) =>
        Task.FromResult(new PagedResult<PermissionDto>([], query.Page, query.PageSize, 0, AccessControlStorageState.ApprovedStorageUnavailable, Blocker));

    public Task<PagedResult<DataScopeDto>> SearchDataScopesAsync(PagedQuery query, CancellationToken cancellationToken = default) =>
        Task.FromResult(new PagedResult<DataScopeDto>([], query.Page, query.PageSize, 0, AccessControlStorageState.ApprovedStorageUnavailable, Blocker));

    public Task<OperationResult> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default) => Task.FromResult(OperationResult.StorageBlocked());
    public Task<OperationResult> CreatePermissionAsync(CreatePermissionRequest request, CancellationToken cancellationToken = default) => Task.FromResult(OperationResult.StorageBlocked());
    public Task<OperationResult> CreateDataScopeAsync(CreateDataScopeRequest request, CancellationToken cancellationToken = default) => Task.FromResult(OperationResult.StorageBlocked());
}