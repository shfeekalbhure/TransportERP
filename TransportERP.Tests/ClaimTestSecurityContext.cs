using System.Security.Claims;
using TransportERP.Api.Security;

namespace TransportERP.Tests;

// Test double for legacy business/API contract tests. Production authorization never uses claims as grants.
internal sealed class ClaimTestSecurityContext : ICurrentSecurityContext
{
    public Task<CurrentSecurityContext?> ResolveAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        TestPrincipalAccessor.Slot.Value = principal;
        if (principal.Identity?.IsAuthenticated != true ||
            !Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier) ?? principal.FindFirstValue("sub"), out var userId) ||
            !Guid.TryParse(principal.FindFirstValue("company_id"), out var companyId))
            return Task.FromResult<CurrentSecurityContext?>(null);
        Guid? branchId = Guid.TryParse(principal.FindFirstValue("branch_id"), out var branch) ? branch : null;
        return Task.FromResult<CurrentSecurityContext?>(new(userId, companyId, branchId, null,
            principal.FindFirstValue("device_id"), false));
    }

    public Task<bool> HasPermissionAsync(CurrentSecurityContext context, string permissionCode,
        CancellationToken cancellationToken = default)
    {
        var principal = TestPrincipalAccessor.Current;
        return Task.FromResult(principal?.Claims.Any(x => x.Type == "permission" && x.Value == permissionCode) == true);
    }

    internal static class TestPrincipalAccessor { public static readonly AsyncLocal<ClaimsPrincipal?> Slot = new(); public static ClaimsPrincipal? Current => Slot.Value; }
}
