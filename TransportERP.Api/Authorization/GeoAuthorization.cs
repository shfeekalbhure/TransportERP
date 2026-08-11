using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using TransportERP.Contracts.Core;

namespace TransportERP.Api.Authorization;

public static class GeoClaims
{
    public const string UserId = "sub";
    public const string CompanyId = "transporterp.company_id";
    public const string BranchId = "transporterp.branch_id";
    public const string Permission = LookupClaims.Permission;

    public static bool HasPermission(this ClaimsPrincipal principal, string permission) =>
        principal.Identity?.IsAuthenticated == true && principal.HasClaim(Permission, permission);

    public static bool TryGetOperationContext(this ClaimsPrincipal principal, HttpRequest request, out OperationContext context)
    {
        context = default!;
        if (!Guid.TryParse(principal.FindFirstValue(UserId), out var userId) ||
            !Guid.TryParse(principal.FindFirstValue(CompanyId), out var companyId) ||
            !Guid.TryParse(principal.FindFirstValue(BranchId), out var branchId)) return false;
        var correlation = request.Headers.TryGetValue("X-Correlation-Id", out var value) && Guid.TryParse(value, out var parsed) ? parsed : Guid.CreateVersion7();
        context = new OperationContext(userId, companyId, branchId, correlation); return true;
    }
}
