using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace TransportERP.Api.Security;

public static class SecurityPolicies
{
    public const string Authenticated = "Authenticated";
    public const string PermissionPrefix = "Permission:";
    public static string Permission(string code) => PermissionPrefix + code;
}

public sealed record ActiveSecurityContextRequirement : IAuthorizationRequirement;
public sealed record PermissionRequirement(string PermissionCode) : IAuthorizationRequirement;

public sealed class SecurityAuthorizationHandler(ICurrentSecurityContext security)
    : IAuthorizationHandler
{
    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        var current = await security.ResolveAsync(context.User);
        foreach (var requirement in context.PendingRequirements.ToArray())
        {
            if (requirement is ActiveSecurityContextRequirement && current is not null)
                context.Succeed(requirement);
            else if (requirement is PermissionRequirement permission && current is not null &&
                     await security.HasPermissionAsync(current, permission.PermissionCode))
                context.Succeed(requirement);
        }
    }
}

public sealed class PermissionPolicyProvider(
    IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(SecurityPolicies.PermissionPrefix, StringComparison.Ordinal))
        {
            var code = policyName[SecurityPolicies.PermissionPrefix.Length..];
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new ActiveSecurityContextRequirement(), new PermissionRequirement(code))
                .Build();
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }
        return base.GetPolicyAsync(policyName);
    }
}
