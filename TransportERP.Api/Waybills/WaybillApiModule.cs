using Microsoft.AspNetCore.Http.HttpResults;
using TransportERP.Api.Security;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Numbering;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Waybills;

public static class WaybillApiModule
{
    public static IServiceCollection AddP2C01AWaybillFoundation(this IServiceCollection services)
    {
        services.AddScoped<IWaybillRepository, ConcurrencySafeWaybillRepository>();
        services.AddScoped<IOperationalPartyRepository, EfOperationalPartyRepository>();
        services.AddScoped<INumberReservationService, EfNumberReservationService>();
        services.AddScoped<IWaybillUnitOfWork, EfWaybillUnitOfWork>();
        services.AddScoped<IWaybillAuditSink, EfWaybillAuditSink>();
        services.AddScoped<WaybillApplicationService>();
        return services;
    }

    public static IEndpointRouteBuilder MapP2C01AWaybillFoundation(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1");

        group.MapPost("/waybills/drafts", async Task<IResult> (
            CreateWaybillDraftRequest request,
            HttpContext http,
            ICurrentSecurityContext security,
            WaybillApplicationService service,
            CancellationToken ct) =>
        {
            var (context, failure) = await TryContextAsync(http, security, ct);
            if (failure is not null) return failure;
            return await Execute(context, () => service.CreateDraftAsync(context, request, ct));
        }).RequireAuthorization(SecurityPolicies.Permission(WaybillPermissionCodes.Create));

        group.MapPut("/waybills/{waybillId:guid}/draft", async Task<IResult> (
            Guid waybillId,
            UpdateWaybillDraftRequest request,
            HttpContext http,
            ICurrentSecurityContext security,
            WaybillApplicationService service,
            CancellationToken ct) =>
        {
            var (context, failure) = await TryContextAsync(http, security, ct);
            if (failure is not null) return failure;
            return await Execute(context, () => service.UpdateDraftAsync(context, waybillId, request, ct));
        }).RequireAuthorization(SecurityPolicies.Permission(WaybillPermissionCodes.Edit));

        group.MapGet("/operational-parties", async Task<IResult> (
            string? query,
            int? skip,
            int? take,
            HttpContext http,
            ICurrentSecurityContext security,
            WaybillApplicationService service,
            CancellationToken ct) =>
        {
            var (context, failure) = await TryContextAsync(http, security, ct);
            if (failure is not null) return failure;
            return await Execute(context, () => service.SearchPartiesAsync(context,
                new OperationalPartySearchRequest(query, skip ?? 0, take ?? 50), ct));
        }).RequireAuthorization(SecurityPolicies.Permission(WaybillPermissionCodes.PartyView));

        group.MapPost("/operational-parties", async Task<IResult> (
            OperationalPartyCreateRequest request,
            HttpContext http,
            ICurrentSecurityContext security,
            WaybillApplicationService service,
            CancellationToken ct) =>
        {
            var (context, failure) = await TryContextAsync(http, security, ct);
            if (failure is not null) return failure;
            return await Execute(context, () => service.CreatePartyAsync(context, request, ct));
        }).RequireAuthorization(SecurityPolicies.Permission(WaybillPermissionCodes.PartyCreate));

        group.MapPost("/waybills/{waybillId:guid}:validate", async Task<IResult> (
            Guid waybillId,
            ValidateWaybillRequest request,
            HttpContext http,
            ICurrentSecurityContext security,
            WaybillApplicationService service,
            CancellationToken ct) =>
        {
            var (context, failure) = await TryContextAsync(http, security, ct);
            if (failure is not null) return failure;
            return await Execute(context, () => service.ValidateAsync(context, waybillId, request, ct));
        }).RequireAuthorization(SecurityPolicies.Permission(WaybillPermissionCodes.Validate));

        group.MapPost("/waybills/{waybillId:guid}:submit", async Task<IResult> (
            Guid waybillId,
            SubmitWaybillRequest request,
            HttpContext http,
            ICurrentSecurityContext security,
            WaybillApplicationService service,
            CancellationToken ct) =>
        {
            var (context, failure) = await TryContextAsync(http, security, ct);
            if (failure is not null) return failure;
            return await Execute(context, () => service.SubmitAsync(context, waybillId, request, ct));
        }).RequireAuthorization(SecurityPolicies.Permission(WaybillPermissionCodes.Submit));

        group.MapPost("/waybills/{waybillId:guid}:approve", async Task<IResult> (
            Guid waybillId,
            ApproveWaybillRequest request,
            HttpContext http,
            ICurrentSecurityContext security,
            WaybillApplicationService service,
            CancellationToken ct) =>
        {
            var (context, failure) = await TryContextAsync(http, security, ct);
            if (failure is not null) return failure;
            return await Execute(context, () => service.ApproveAsync(context, waybillId, request, ct));
        }).RequireAuthorization(SecurityPolicies.Permission(WaybillPermissionCodes.Approve));

        group.MapPost("/waybills/{waybillId:guid}:return", async Task<IResult> (
            Guid waybillId,
            ReturnWaybillRequest request,
            HttpContext http,
            ICurrentSecurityContext security,
            WaybillApplicationService service,
            CancellationToken ct) =>
        {
            var (context, failure) = await TryContextAsync(http, security, ct);
            if (failure is not null) return failure;
            return await Execute(context, () => service.ReturnForCorrectionAsync(context, waybillId, request, ct));
        }).RequireAuthorization(SecurityPolicies.Permission(WaybillPermissionCodes.Return));

        group.MapPost("/waybills/{waybillId:guid}:cancel", async Task<IResult> (
            Guid waybillId,
            CancelWaybillRequest request,
            HttpContext http,
            ICurrentSecurityContext security,
            WaybillApplicationService service,
            CancellationToken ct) =>
        {
            var (context, failure) = await TryContextAsync(http, security, ct);
            if (failure is not null) return failure;
            return await Execute(context, () => service.CancelAsync(context, waybillId, request, ct));
        }).RequireAuthorization(SecurityPolicies.Permission(WaybillPermissionCodes.Cancel));

        return app;
    }

    private static async Task<IResult> Execute<T>(OperationContext context, Func<Task<T>> action)
    {
        try
        {
            return Results.Ok(await action());
        }
        catch (WaybillValidationException ex)
        {
            return Results.BadRequest(new { ErrorCode = "VALIDATION_ERROR", BlockingErrors = ex.Errors, CorrelationId = context.CorrelationId });
        }
        catch (WaybillRuleException ex)
        {
            return MapError(ex.Code, context.CorrelationId);
        }
        catch (WaybillApplicationException ex)
        {
            return MapError(ex.Code, context.CorrelationId);
        }
        catch (WaybillPersistenceException ex)
        {
            return MapError(ex.Code, context.CorrelationId);
        }
        catch (ArgumentException)
        {
            return Results.BadRequest(new { ErrorCode = "VALIDATION_ERROR", CorrelationId = context.CorrelationId });
        }
    }

    private static IResult MapError(string code, Guid correlationId) => code switch
    {
        "NOT_FOUND" => Results.NotFound(new { ErrorCode = code, CorrelationId = correlationId }),
        "SCOPE_DENIED" => Forbidden(correlationId),
        "CONCURRENCY_CONFLICT" or "IDEMPOTENCY_CONFLICT" or "NUMBERING_CONCURRENCY" =>
            Results.Json(new { ErrorCode = code, CorrelationId = correlationId }, statusCode: StatusCodes.Status409Conflict),
        "INVALID_STATE" or "NUMBERING_UNAVAILABLE" or "NUMBERING_COMMIT_FAILED" or
        "NUMBER_ALREADY_VOID" or "COMMITTED_NUMBER_CANNOT_VOID" or "NUMBER_RESERVATION_UNLINKED" =>
            Results.Json(new { ErrorCode = code, CorrelationId = correlationId }, statusCode: StatusCodes.Status409Conflict),
        _ => Results.BadRequest(new { ErrorCode = code, CorrelationId = correlationId })
    };

    private static IResult Forbidden(Guid correlationId)
        => Results.Json(new { ErrorCode = "SCOPE_DENIED", CorrelationId = correlationId }, statusCode: StatusCodes.Status403Forbidden);

    private static async Task<(OperationContext Context, IResult? Failure)> TryContextAsync(
        HttpContext http, ICurrentSecurityContext security, CancellationToken ct)
    {
        var current = await security.ResolveAsync(http.User, ct);
        if (current is null || !current.BranchId.HasValue) return (default!, Results.Unauthorized());
        var correlationId = Guid.TryParse(http.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var parsed)
            ? parsed : Guid.NewGuid();
        return (current.ToOperationContext(correlationId), null);
    }
}
