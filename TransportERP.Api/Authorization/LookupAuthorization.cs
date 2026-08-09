using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TransportERP.Api.Authorization;

public static class LookupClaims
{
    public const string Permission = "transporterp.permission";
    public const string Company = "transporterp.company";
    public const string Branch = "transporterp.branch";
    public const string ReadPolicy = "TransportERP.Lookup.Read";
    public const string ReadPermission = "lookup.read";
}

public sealed class LookupReadRequirement : IAuthorizationRequirement;

/// <summary>Accepts only an authenticated principal carrying a server-issued lookup permission.</summary>
public sealed class LookupReadAuthorizationHandler : AuthorizationHandler<LookupReadRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LookupReadRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated == true &&
            context.User.HasClaim(LookupClaims.Permission, LookupClaims.ReadPermission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public static class LookupPrincipalExtensions
{
    public static bool TryGetTrustedScope(this ClaimsPrincipal principal, out string company, out string branch)
    {
        company = principal.FindFirst(LookupClaims.Company)?.Value ?? string.Empty;
        branch = principal.FindFirst(LookupClaims.Branch)?.Value ?? string.Empty;
        return principal.Identity?.IsAuthenticated == true &&
               !string.IsNullOrWhiteSpace(company) &&
               !string.IsNullOrWhiteSpace(branch);
    }
}
