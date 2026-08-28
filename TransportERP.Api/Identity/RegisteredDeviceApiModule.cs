using TransportERP.Api.Security;
using TransportERP.Contracts.Identity;

namespace TransportERP.Api.Identity;

public static class RegisteredDevicePermissionCodes
{
    public const string Register = "devices.register";
    public const string Read = "devices.read";
    public const string Manage = "devices.manage";
}

public static class RegisteredDeviceApiModule
{
    public static IEndpointRouteBuilder MapRegisteredDevices(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/devices", async Task<IResult> (RegisterDeviceRequest request, HttpContext http,
            ICurrentSecurityContext security, RegisteredDeviceService devices, CancellationToken ct) =>
            await Execute(http, security, (current, correlation) => devices.RegisterAsync(current, request, correlation, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(RegisteredDevicePermissionCodes.Register));

        app.MapGet("/api/v1/devices/current", async Task<IResult> (HttpContext http,
            ICurrentSecurityContext security, RegisteredDeviceService devices, CancellationToken ct) =>
        {
            var current = await security.ResolveAsync(http.User, ct);
            if (current is null) return Results.Unauthorized();
            if (!current.IsLocalSession || current.RegisteredDeviceId is null ||
                current.DeviceCredentialVersion is null)
                return Results.Json(new { ErrorCode = "DEVICE_BINDING_REQUIRED", CorrelationId = CorrelationId(http) },
                    statusCode: StatusCodes.Status403Forbidden);
            var result = await devices.CurrentAsync(current, ct);
            return result is null ? Results.NotFound(new { ErrorCode = "DEVICE_NOT_FOUND", CorrelationId = CorrelationId(http) }) : Results.Ok(result);
        }).RequireAuthorization(SecurityPolicies.Authenticated);

        app.MapGet("/api/v1/devices", async Task<IResult> (HttpContext http,
            ICurrentSecurityContext security, RegisteredDeviceService devices, CancellationToken ct) =>
            await Execute(http, security, (current, _) => devices.ListAsync(current, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(RegisteredDevicePermissionCodes.Read));

        MapStatus(app, "approve", (d, id, c, correlation, ct) => d.ApproveAsync(id, c, correlation, ct));
        MapStatus(app, "suspend", (d, id, c, correlation, ct) => d.SuspendAsync(id, c, correlation, ct));
        MapStatus(app, "reactivate", (d, id, c, correlation, ct) => d.ReactivateAsync(id, c, correlation, ct));
        MapStatus(app, "revoke", (d, id, c, correlation, ct) => d.RevokeAsync(id, c, correlation, ct));

        app.MapPost("/api/v1/devices/{deviceId:guid}/assignments", async Task<IResult> (Guid deviceId,
            AddDeviceAssignmentRequest request, HttpContext http, ICurrentSecurityContext security,
            RegisteredDeviceService devices, CancellationToken ct) => await Execute(http, security,
            (current, correlation) => devices.AddAssignmentAsync(deviceId, request, current, correlation, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(RegisteredDevicePermissionCodes.Manage));

        app.MapDelete("/api/v1/devices/{deviceId:guid}/assignments/{assignmentId:guid}", async Task<IResult> (
            Guid deviceId, Guid assignmentId, HttpContext http, ICurrentSecurityContext security,
            RegisteredDeviceService devices, CancellationToken ct) => await ExecuteNoContent(http, security,
            (current, correlation) => devices.RemoveAssignmentAsync(deviceId, assignmentId, current, correlation, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(RegisteredDevicePermissionCodes.Manage));

        app.MapPost("/api/v1/devices/{deviceId:guid}:rotate-credential", async Task<IResult> (Guid deviceId,
            RotateDeviceCredentialRequest request, HttpContext http, ICurrentSecurityContext security,
            RegisteredDeviceService devices, CancellationToken ct) => await Execute(http, security,
            (current, correlation) => devices.RotateCredentialAsync(deviceId, request, current, correlation, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(RegisteredDevicePermissionCodes.Manage));
        return app;
    }

    private static void MapStatus(IEndpointRouteBuilder app, string action,
        Func<RegisteredDeviceService, Guid, CurrentSecurityContext, Guid, CancellationToken, Task<RegisteredDeviceResponse>> mutation)
        => app.MapPost($"/api/v1/devices/{{deviceId:guid}}:{action}", async Task<IResult> (Guid deviceId,
            HttpContext http, ICurrentSecurityContext security, RegisteredDeviceService devices, CancellationToken ct) =>
            await Execute(http, security, (current, correlation) => mutation(devices, deviceId, current, correlation, ct)))
            .RequireAuthorization(SecurityPolicies.Permission(RegisteredDevicePermissionCodes.Manage));

    private static async Task<IResult> Execute<T>(HttpContext http, ICurrentSecurityContext security,
        Func<CurrentSecurityContext, Guid, Task<T>> action)
    {
        var current = await security.ResolveAsync(http.User, http.RequestAborted);
        if (current is null) return Results.Unauthorized();
        var correlationId = CorrelationId(http);
        try { return Results.Ok(await action(current, correlationId)); }
        catch (RegisteredDeviceException ex) { return Error(ex.Code, correlationId); }
    }

    private static async Task<IResult> ExecuteNoContent(HttpContext http, ICurrentSecurityContext security,
        Func<CurrentSecurityContext, Guid, Task> action)
    {
        var current = await security.ResolveAsync(http.User, http.RequestAborted);
        if (current is null) return Results.Unauthorized();
        var correlationId = CorrelationId(http);
        try { await action(current, correlationId); return Results.NoContent(); }
        catch (RegisteredDeviceException ex) { return Error(ex.Code, correlationId); }
    }

    private static IResult Error(string code, Guid correlationId) => code switch
    {
        "DEVICE_NOT_FOUND" or "ASSIGNMENT_NOT_FOUND" => Results.NotFound(new { ErrorCode = code, CorrelationId = correlationId }),
        "DEVICE_REGISTRATION_CONFLICT" or "DEVICE_VERSION_CONFLICT" or "DEVICE_STATE_INVALID" or "DEVICE_REVOKED" =>
            Results.Json(new { ErrorCode = code, CorrelationId = correlationId }, statusCode: StatusCodes.Status409Conflict),
        _ => Results.BadRequest(new { ErrorCode = code, CorrelationId = correlationId })
    };

    private static Guid CorrelationId(HttpContext context)
        => Guid.TryParse(context.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var id) ? id : Guid.NewGuid();
}
