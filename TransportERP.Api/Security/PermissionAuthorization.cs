using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
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
    internal const string ActiveContextInvalidItem = "TransportERP.Security.ActiveContextInvalid";

    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        var current = await security.ResolveAsync(context.User);
        if (current is null && context.User.Identity?.IsAuthenticated == true &&
            context.Resource is HttpContext http)
            http.Items[ActiveContextInvalidItem] = true;
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

public sealed record AuthorizationErrorResponse(string ErrorCode, Guid CorrelationId);

public sealed class TransportAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler fallback = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (!authorizeResult.Forbidden)
        {
            await fallback.HandleAsync(next, context, policy, authorizeResult);
            return;
        }

        if (context.Items.ContainsKey(SecurityAuthorizationHandler.ActiveContextInvalidItem))
        {
            await fallback.HandleAsync(next, context, policy, PolicyAuthorizationResult.Challenge());
            return;
        }

        var correlationId = Guid.TryParse(
            context.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var suppliedCorrelationId)
            ? suppliedCorrelationId
            : Guid.NewGuid();
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsJsonAsync(
            new AuthorizationErrorResponse("SCOPE_DENIED", correlationId),
            cancellationToken: context.RequestAborted);
    }
}
