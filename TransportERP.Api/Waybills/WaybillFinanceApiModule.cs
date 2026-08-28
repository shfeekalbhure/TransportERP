using TransportERP.Api.Security;
using TransportERP.Application.Waybills;
using TransportERP.Contracts.Core;
using TransportERP.Contracts.Waybills;
using TransportERP.Domain.Waybills;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Waybills;

public static class WaybillFinanceApiModule
{
    public static IServiceCollection AddP2C01BWaybillFinance(this IServiceCollection services)
    {
        services.AddScoped<IWaybillFinanceStore, EfWaybillFinanceStore>();
        services.AddScoped<WaybillFinanceApplicationService>();
        return services;
    }

    public static IEndpointRouteBuilder MapP2C01BWaybillFinance(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1");

        // W2-P2C01-011 — SetPaymentPlan
        group.MapPut("/waybills/{waybillId:guid}/payment-plan", async Task<IResult> (
            Guid waybillId,
            SetPaymentPlanRequest request,
            HttpContext http,
            ICurrentSecurityContext security,
            WaybillFinanceApplicationService service,
            CancellationToken ct) =>
        {
            var (context, failure) = await TryContextAsync(http, security, ct);
            if (failure is not null) return failure;
            return await Execute(context, () => service.SetPaymentPlanAsync(context, waybillId, request, ct));
        }).RequireAuthorization(SecurityPolicies.Permission(WaybillFinancePermissionCodes.PaymentPlan));

        // W2-P2C01-012 — RecordCollection
        group.MapPost("/waybills/{waybillId:guid}/collections", async Task<IResult> (
            Guid waybillId,
            RecordCollectionRequest request,
            HttpContext http,
            ICurrentSecurityContext security,
            WaybillFinanceApplicationService service,
            CancellationToken ct) =>
        {
            var (context, failure) = await TryContextAsync(http, security, ct);
            if (failure is not null) return failure;
            return await Execute(context, () => service.RecordCollectionAsync(context, waybillId, request, ct));
        }).RequireAuthorization(SecurityPolicies.Permission(WaybillFinancePermissionCodes.CollectionCreate));

        // W2-P2C01-013 — ReverseCollection
        group.MapPost("/collections/{collectionId:guid}:reverse", async Task<IResult> (
            Guid collectionId,
            ReverseCollectionRequest request,
            HttpContext http,
            ICurrentSecurityContext security,
            WaybillFinanceApplicationService service,
            CancellationToken ct) =>
        {
            var (context, failure) = await TryContextAsync(http, security, ct);
            if (failure is not null) return failure;
            return await Execute(context, () => service.ReverseCollectionAsync(context, collectionId, request, ct));
        }).RequireAuthorization(SecurityPolicies.Permission(WaybillFinancePermissionCodes.CollectionReverse));

        return app;
    }

    private static async Task<IResult> Execute<T>(OperationContext context, Func<Task<T>> action)
    {
        try
        {
            return Results.Ok(await action());
        }
        catch (WaybillFinancialRuleException ex)
        {
            return MapError(ex.Code, context.CorrelationId);
        }
        catch (WaybillFinanceApplicationException ex)
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
        "CONCURRENCY_CONFLICT" or "IDEMPOTENCY_CONFLICT" or "DUPLICATE_OPERATION" or
        "ALREADY_REVERSED" or "PERIOD_CLOSED" or "INVALID_STATE" =>
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
