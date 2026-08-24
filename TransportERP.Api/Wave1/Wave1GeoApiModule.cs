using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TransportERP.Contracts.Geo;
using TransportERP.Infrastructure.Persistence;

namespace TransportERP.Api.Wave1;

public static class Wave1GeoApiModule
{
    public static IEndpointRouteBuilder MapWave1Geography(this IEndpointRouteBuilder app)
    {
        MapGovernorates(app);
        MapDirectorates(app);
        MapCities(app);
        MapAreas(app);
        return app;
    }

    private static void MapCountries(IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/general/countries").RequireAuthorization("Authenticated");
        g.MapGet("", ([AsParameters] PagedQueryRequest q, HttpContext h, Wave1GeoService s, CancellationToken ct) => ReadList(Wave1GeoResource.Countries, "GEN003.View", q, h, s, ct));
        g.MapGet("/{id:guid}", (Guid id, HttpContext h, Wave1GeoService s, CancellationToken ct) => ReadOne(Wave1GeoResource.Countries, "GEN003.View", id, h, s, ct));
        g.MapPost("", (CreateCountryRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Create(Wave1GeoResource.Countries, "GEN003.Create", r, h, s, ct));
        g.MapPut("/{id:guid}", (Guid id, UpdateCountryRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Update(Wave1GeoResource.Countries, "GEN003.Edit", id, r, h, s, ct));
        g.MapPost("/{id:guid}/disable", (Guid id, DisableRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Disable(Wave1GeoResource.Countries, "GEN003.Disable", id, r, h, s, ct));
    }

    private static void MapGovernorates(IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/general/governorates").RequireAuthorization("Authenticated");
        g.MapGet("", ([AsParameters] PagedQueryRequest q, HttpContext h, Wave1GeoService s, CancellationToken ct) => ReadList(Wave1GeoResource.Governorates, "GEN004.View", q, h, s, ct));
        g.MapGet("/{id:guid}", (Guid id, HttpContext h, Wave1GeoService s, CancellationToken ct) => ReadOne(Wave1GeoResource.Governorates, "GEN004.View", id, h, s, ct));
        g.MapPost("", (CreateGovernorateRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Create(Wave1GeoResource.Governorates, "GEN004.Create", r, h, s, ct));
        g.MapPut("/{id:guid}", (Guid id, UpdateGovernorateRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Update(Wave1GeoResource.Governorates, "GEN004.Edit", id, r, h, s, ct));
        g.MapPost("/{id:guid}/disable", (Guid id, DisableRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Disable(Wave1GeoResource.Governorates, "GEN004.Disable", id, r, h, s, ct));
    }

    private static void MapDirectorates(IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/general/directorates").RequireAuthorization("Authenticated");
        g.MapGet("", ([AsParameters] PagedQueryRequest q, HttpContext h, Wave1GeoService s, CancellationToken ct) => ReadList(Wave1GeoResource.Directorates, "GEN005.View", q, h, s, ct));
        g.MapGet("/{id:guid}", (Guid id, HttpContext h, Wave1GeoService s, CancellationToken ct) => ReadOne(Wave1GeoResource.Directorates, "GEN005.View", id, h, s, ct));
        g.MapPost("", (CreateDirectorateRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Create(Wave1GeoResource.Directorates, "GEN005.Create", r, h, s, ct));
        g.MapPut("/{id:guid}", (Guid id, UpdateDirectorateRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Update(Wave1GeoResource.Directorates, "GEN005.Edit", id, r, h, s, ct));
        g.MapPost("/{id:guid}/disable", (Guid id, DisableRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Disable(Wave1GeoResource.Directorates, "GEN005.Disable", id, r, h, s, ct));
    }

    private static void MapCities(IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/general/cities").RequireAuthorization("Authenticated");
        g.MapGet("", ([AsParameters] PagedQueryRequest q, HttpContext h, Wave1GeoService s, CancellationToken ct) => ReadList(Wave1GeoResource.Cities, "GEN006.View", q, h, s, ct));
        g.MapGet("/{id:guid}", (Guid id, HttpContext h, Wave1GeoService s, CancellationToken ct) => ReadOne(Wave1GeoResource.Cities, "GEN006.View", id, h, s, ct));
        g.MapPost("", (CreateCityRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Create(Wave1GeoResource.Cities, "GEN006.Create", r, h, s, ct));
        g.MapPut("/{id:guid}", (Guid id, UpdateCityRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Update(Wave1GeoResource.Cities, "GEN006.Edit", id, r, h, s, ct));
        g.MapPost("/{id:guid}/disable", (Guid id, DisableRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Disable(Wave1GeoResource.Cities, "GEN006.Disable", id, r, h, s, ct));
    }

    private static void MapAreas(IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/api/v1/general/areas").RequireAuthorization("Authenticated");
        g.MapGet("", ([AsParameters] PagedQueryRequest q, HttpContext h, Wave1GeoService s, CancellationToken ct) => ReadList(Wave1GeoResource.Areas, "GEN007.View", q, h, s, ct));
        g.MapGet("/{id:guid}", (Guid id, HttpContext h, Wave1GeoService s, CancellationToken ct) => ReadOne(Wave1GeoResource.Areas, "GEN007.View", id, h, s, ct));
        g.MapPost("", (CreateAreaRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Create(Wave1GeoResource.Areas, "GEN007.Create", r, h, s, ct));
        g.MapPut("/{id:guid}", (Guid id, UpdateAreaRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Update(Wave1GeoResource.Areas, "GEN007.Edit", id, r, h, s, ct));
        g.MapPost("/{id:guid}/disable", (Guid id, DisableRequest r, HttpContext h, Wave1GeoService s, CancellationToken ct) => Disable(Wave1GeoResource.Areas, "GEN007.Disable", id, r, h, s, ct));
    }

    private static async Task<IResult> ReadList(Wave1GeoResource resource, string permission, PagedQueryRequest query, HttpContext h, Wave1GeoService s, CancellationToken ct)
    {
        if (!HasPermission(h.User, permission)) return Forbidden(h);
        try { return Results.Ok(await s.ListAsync(resource, query, ct)); }
        catch (ArgumentOutOfRangeException ex) { return Bad("INVALID_FILTER", ex.Message, h); }
    }

    private static async Task<IResult> ReadOne(Wave1GeoResource resource, string permission, Guid id, HttpContext h, Wave1GeoService s, CancellationToken ct)
    {
        if (!HasPermission(h.User, permission)) return Forbidden(h);
        var row = await s.GetAsync(resource, id, ct);
        return row is null ? Results.NotFound(new { ErrorCode = "NOT_FOUND", CorrelationId = Correlation(h) }) : Results.Ok(row);
    }

    private static async Task<IResult> Create(Wave1GeoResource resource, string permission, object request, HttpContext h, Wave1GeoService s, CancellationToken ct)
    {
        if (!HasPermission(h.User, permission)) return Forbidden(h);
        try { return Results.Ok(await s.CreateAsync(resource, request, Context(h), ct)); }
        catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
    }

    private static async Task<IResult> Update(Wave1GeoResource resource, string permission, Guid id, object request, HttpContext h, Wave1GeoService s, CancellationToken ct)
    {
        if (!HasPermission(h.User, permission)) return Forbidden(h);
        try
        {
            var row = await s.UpdateAsync(resource, id, request, Context(h), ct);
            return row is null ? Results.NotFound(new { ErrorCode = "NOT_FOUND", CorrelationId = Correlation(h) }) : Results.Ok(row);
        }
        catch (DbUpdateConcurrencyException) { return Conflict(h); }
        catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
    }

    private static async Task<IResult> Disable(Wave1GeoResource resource, string permission, Guid id, DisableRequest request, HttpContext h, Wave1GeoService s, CancellationToken ct)
    {
        if (!HasPermission(h.User, permission)) return Forbidden(h);
        try
        {
            var row = await s.DisableAsync(resource, id, request, Context(h), ct);
            return row is null ? Results.NotFound(new { ErrorCode = "NOT_FOUND", CorrelationId = Correlation(h) }) : Results.Ok(row);
        }
        catch (DbUpdateConcurrencyException) { return Conflict(h); }
        catch (ArgumentException ex) { return Unprocessable(ex.Message, h); }
    }

    private static Wave1GeoOperationContext Context(HttpContext h)
        => new(
            TryGuid(h.User, ClaimTypes.NameIdentifier) ?? TryGuid(h.User, "sub"),
            TryGuid(h.User, "company_id"),
            TryGuid(h.User, "branch_id"),
            Correlation(h),
            h.User.FindFirstValue("device_id"),
            h.Connection.RemoteIpAddress?.ToString());

    private static bool HasPermission(ClaimsPrincipal p, string permission)
        => p.Claims.Any(x => x.Type is "permission" or ClaimTypes.Role && string.Equals(x.Value, permission, StringComparison.OrdinalIgnoreCase));

    private static Guid? TryGuid(ClaimsPrincipal p, string type)
        => Guid.TryParse(p.FindFirstValue(type), out var value) ? value : null;

    private static Guid Correlation(HttpContext h)
        => Guid.TryParse(h.Request.Headers["X-Correlation-Id"].FirstOrDefault(), out var value) ? value : Guid.NewGuid();

    private static IResult Forbidden(HttpContext h)
        => Results.Json(new { ErrorCode = "PERMISSION_DENIED", CorrelationId = Correlation(h) }, statusCode: StatusCodes.Status403Forbidden);

    private static IResult Conflict(HttpContext h)
        => Results.Conflict(new { ErrorCode = "CONCURRENCY_CONFLICT", CorrelationId = Correlation(h) });

    private static IResult Bad(string code, string message, HttpContext h)
        => Results.BadRequest(new { ErrorCode = code, Message = message, CorrelationId = Correlation(h) });

    private static IResult Unprocessable(string code, HttpContext h)
        => Results.UnprocessableEntity(new { ErrorCode = code, CorrelationId = Correlation(h) });
}
