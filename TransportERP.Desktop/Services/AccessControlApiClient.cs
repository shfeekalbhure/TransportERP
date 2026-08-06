using System.Net.Http.Json;
using TransportERP.Contracts.AccessControl;

namespace TransportERP.Desktop.Services;

/// <summary>عميل Desktop الوحيد لمسار التحكم بالوصول؛ لا يصل إلى Infrastructure أو قاعدة بيانات.</summary>
public sealed class AccessControlApiClient(HttpClient httpClient)
{
    public async Task<PagedResult<RoleDto>> SearchRolesAsync(PagedQuery query, CancellationToken cancellationToken = default)
    {
        var url = $"api/access-control/roles?page={query.Page}&pageSize={query.PageSize}&search={Uri.EscapeDataString(query.Search ?? string.Empty)}&status={Uri.EscapeDataString(query.Status ?? string.Empty)}";
        return await httpClient.GetFromJsonAsync<PagedResult<RoleDto>>(url, cancellationToken)
            ?? throw new InvalidOperationException("استجابة الأدوار فارغة.");
    }

    public async Task<PagedResult<PermissionDto>> SearchPermissionsAsync(PagedQuery query, CancellationToken cancellationToken = default) =>
        await httpClient.GetFromJsonAsync<PagedResult<PermissionDto>>($"api/access-control/permissions?page={query.Page}&pageSize={query.PageSize}", cancellationToken)
        ?? throw new InvalidOperationException("استجابة الصلاحيات فارغة.");

    public async Task<PagedResult<DataScopeDto>> SearchDataScopesAsync(PagedQuery query, CancellationToken cancellationToken = default) =>
        await httpClient.GetFromJsonAsync<PagedResult<DataScopeDto>>($"api/access-control/data-scopes?page={query.Page}&pageSize={query.PageSize}", cancellationToken)
        ?? throw new InvalidOperationException("استجابة نطاقات الوصول فارغة.");
}