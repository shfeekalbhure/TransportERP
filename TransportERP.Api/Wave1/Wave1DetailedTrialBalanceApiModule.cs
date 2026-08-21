using System.Security.Claims;
using TransportERP.Contracts.Wave1;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1DetailedTrialBalanceApiModule
{
    public static IEndpointRouteBuilder MapWave1DetailedTrialBalance(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/accounting/reports/detailed-trial-balance")
            .RequireAuthorization("Authenticated");

        group.MapPost("/query", async (
            DetailedTrialBalanceQueryRequest request,
            HttpContext httpContext,
            Wave1DetailedTrialBalanceService service,
            CancellationToken cancellationToken) =>
        {
            if (!HasPermission(httpContext.User, "ACC058.View"))
                return Forbidden("PERMISSION_DENIED", httpContext);
            if (!TryResolveScope(httpContext.User, request.BranchId, out var companyId, out var branchId))
                return Forbidden("SCOPE_DENIED", httpContext);

            try
            {
                var response = await service.QueryAsync(companyId, branchId, request, cancellationToken);
                return Results.Ok(response);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new
                {
                    ErrorCode = "INVALID_FILTER",
                    Message = ex.Message,
                    CorrelationId = GetCorrelationId(httpContext)
                });
            }
        });

        group.MapPost("/drill-down", async (
            DetailedTrialBalanceDrillDownRequest request,
            HttpContext httpContext,
            Wave1DetailedTrialBalanceService service,
            CancellationToken cancellationToken) =>
        {
            if (!HasPermission(httpContext.User, "ACC058.DrillDown"))
                return Forbidden("PERMISSION_DENIED", httpContext);
            if (!TryResolveScope(httpContext.User, request.BranchId, out var companyId, out var branchId))
                return Forbidden("SCOPE_DENIED", httpContext);

            try
            {
                var response = await service.DrillDownAsync(companyId, branchId, request, cancellationToken);
                return Results.Ok(response);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new
                {
                    ErrorCode = "INVALID_FILTER",
                    Message = ex.Message,
                    CorrelationId = GetCorrelationId(httpContext)
                });
            }
        });

        return app;
    }

    private static bool TryResolveScope(
        ClaimsPrincipal principal,
        Guid? requestedBranchId,
        out Guid companyId,
        out Guid? branchId)
    {
        companyId = default;
        branchId = null;

        if (!Guid.TryParse(principal.FindFirstValue("company_id"), out companyId))
            return false;

        var branchClaim = principal.FindFirstValue("branch_id");
        if (Guid.TryParse(branchClaim, out var claimedBranchId))
        {
            if (requestedBranchId.HasValue && requestedBranchId.Value != claimedBranchId)
                return false;
            branchId = claimedBranchId;
            return true;
        }

        branchId = requestedBranchId;
        return true;
    }

    private static bool HasPermission(ClaimsPrincipal principal, string permission)
        => principal.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role &&
            string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));

    private static IResult Forbidden(string errorCode, HttpContext context)
        => Results.Json(new
        {
            ErrorCode = errorCode,
            CorrelationId = GetCorrelationId(context)
        }, statusCode: StatusCodes.Status403Forbidden);

    private static Guid GetCorrelationId(HttpContext context)
        => Guid.TryParse(context.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var id)
            ? id
            : Guid.NewGuid();
}
