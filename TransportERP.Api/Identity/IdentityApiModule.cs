using TransportERP.Api.Security;
using TransportERP.Contracts.Identity;

namespace TransportERP.Api.Identity;

public static class IdentityApiModule
{
    public static IEndpointRouteBuilder MapIdentitySessions(this IEndpointRouteBuilder app, TransportAuthMode mode)
    {
        if (mode != TransportAuthMode.LocalSessions) return app;

        app.MapPost("/api/v1/auth/sessions", async Task<IResult> (CreateIdentitySessionRequest request, HttpContext http,
            IdentityRateLimiter limiter, CancellationToken ct) =>
        {
            var correlationId = CorrelationId(http);
            var ip = http.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var login = IdentitySessionService.NormalizeLogin(request.UserNameOrEmail);
            var device = IdentitySessionService.NormalizeDevice(request.DeviceId);
            var decision = await limiter.TryAcquireLoginAsync(ip, login ?? "<invalid-login>", device ?? "<invalid-device>", ct);
            if (!decision.IsAcquired)
                return RateLimited(http, correlationId, decision.RetryAfter);
            try
            {
                var service = http.RequestServices.GetRequiredService<IdentitySessionService>();
                return Results.Ok(await service.CreateNormalizedAsync(request, login, device, correlationId,
                    ip, ct));
            }
            catch (IdentitySessionException)
            {
                return Results.Json(new { ErrorCode = "INVALID_CREDENTIALS", CorrelationId = correlationId }, statusCode: 401);
            }
        }).AllowAnonymous();

        app.MapPost("/api/v1/auth/sessions:refresh", async Task<IResult> (RefreshIdentitySessionRequest request, HttpContext http,
            IdentityRateLimiter limiter, CancellationToken ct) =>
        {
            var correlationId = CorrelationId(http);
            var ip = http.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var device = IdentitySessionService.NormalizeDevice(request.DeviceId);
            var refreshPartition = IdentitySessionService.HashRefreshPartition(request.RefreshToken);
            var decision = await limiter.TryAcquireRefreshAsync(ip, refreshPartition,
                device ?? "<invalid-device>", ct);
            if (!decision.IsAcquired)
                return RateLimited(http, correlationId, decision.RetryAfter);
            try
            {
                var service = http.RequestServices.GetRequiredService<IdentitySessionService>();
                return Results.Ok(await service.RefreshNormalizedAsync(request, device,
                    correlationId,
                    ip, ct));
            }
            catch (IdentitySessionException)
            {
                return Results.Json(new { ErrorCode = "REFRESH_TOKEN_INVALID", CorrelationId = correlationId }, statusCode: 401);
            }
        }).AllowAnonymous();

        app.MapPost("/api/v1/auth/sessions/{sessionId:guid}:revoke", async Task<IResult> (Guid sessionId,
            RevokeIdentitySessionRequest? request, HttpContext http, ICurrentSecurityContext security,
            IdentitySessionService service, CancellationToken ct) =>
        {
            var correlationId = CorrelationId(http);
            var current = await security.ResolveAsync(http.User, ct);
            if (current is null) return Results.Unauthorized();
            try
            {
                await service.RevokeAsync(sessionId, current, request?.Reason, correlationId,
                    http.Connection.RemoteIpAddress?.ToString(), ct);
                return Results.NoContent();
            }
            catch (IdentitySessionException ex)
            {
                return ex.Code == "SESSION_NOT_FOUND" ? Results.NotFound() :
                    Results.Json(new { ErrorCode = ex.Code, CorrelationId = correlationId }, statusCode: 403);
            }
        }).RequireAuthorization(SecurityPolicies.Authenticated);

        return app;
    }

    private static Guid CorrelationId(HttpContext context)
        => Guid.TryParse(context.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var id) ? id : Guid.NewGuid();

    private static IResult RateLimited(HttpContext context, Guid correlationId, TimeSpan? retryAfter)
    {
        var seconds = Math.Max(1, (int)Math.Ceiling((retryAfter ?? TimeSpan.FromSeconds(1)).TotalSeconds));
        context.Response.Headers["Retry-After"] = seconds.ToString(System.Globalization.CultureInfo.InvariantCulture);
        return Results.Json(new { ErrorCode = "RATE_LIMITED", CorrelationId = correlationId }, statusCode: 429);
    }
}
