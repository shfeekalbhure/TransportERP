using Microsoft.AspNetCore.Mvc;
using TransportERP.Api.Authorization;
using TransportERP.Contracts.Core;
using TransportERP.Application.Geo;
using TransportERP.Contracts.Geo;

namespace TransportERP.Api.Controllers;

public abstract class GeoControllerBase(IGeoService service) : ControllerBase
{
    protected async Task<ActionResult<PagedResponse<GeoDto>>> List(GeoResource resource, string screen, [FromQuery] PagedQueryRequest query, CancellationToken ct)
    {
        if (!TryAuthorize(screen + ".View", out var context, out var authorizationError)) return Forbidden(authorizationError!);
        try { return Ok(await service.ListAsync(resource, query with { PageSize = Math.Min(query.PageSize, 200) }, context, ct)); } catch (ArgumentOutOfRangeException) { return BadRequest(); }
    }
    protected async Task<ActionResult<GeoDto>> Get(GeoResource resource, string screen, Guid id, CancellationToken ct)
    { if (!TryAuthorize(screen + ".View", out var context, out var authorizationError)) return Forbidden(authorizationError!); var item = await service.GetAsync(resource, id, context, ct); return item is null ? NotFound(context) : Ok(item); }
    protected async Task<ActionResult<GeoDto>> Create(GeoResource resource, string screen, object request, CancellationToken ct)
    { if (!TryAuthorize(screen + ".Create", out var context, out var authorizationError)) return Forbidden(authorizationError!); try { return Created(string.Empty, await service.CreateAsync(resource, request, context, ct)); } catch (ArgumentException) { return BadRequest(); } }
    protected async Task<ActionResult<GeoDto>> Update(GeoResource resource, string screen, Guid id, object request, CancellationToken ct)
    { if (!TryAuthorize(screen + ".Edit", out var context, out var authorizationError)) return Forbidden(authorizationError!); try { var item = await service.UpdateAsync(resource, id, request, context, ct); return item is null ? NotFound(context) : Ok(item); } catch (ArgumentException) { return BadRequest(); } catch (InvalidOperationException) { return Conflict(); } }
    protected async Task<ActionResult<GeoDto>> Disable(GeoResource resource, string screen, Guid id, DisableRequest request, CancellationToken ct)
    { if (!TryAuthorize(screen + ".Disable", out var context, out var authorizationError)) return Forbidden(authorizationError!); try { var item = await service.DisableAsync(resource, id, request, context, ct); return item is null ? NotFound(context) : Ok(item); } catch (ArgumentException) { return BadRequest(); } catch (InvalidOperationException) { return Conflict(); } }
    private bool TryAuthorize(string permission, out OperationContext context, out TransportError? error)
    {
        context = default!;
        if (!User.HasPermission(permission))
        {
            error = new TransportError(TransportErrorCode.PermissionDenied, CorrelationId(), "error.permissionDenied");
            return false;
        }
        if (!User.TryGetOperationContext(Request, out context))
        {
            error = new TransportError(TransportErrorCode.ScopeDenied, CorrelationId(), "error.scopeDenied");
            return false;
        }
        error = null;
        return true;
    }
    private ObjectResult NotFound(OperationContext context) => StatusCode(StatusCodes.Status404NotFound, new TransportError(TransportErrorCode.NotFound, context.CorrelationId, "error.notFound"));
    private ObjectResult Forbidden(TransportError error) => StatusCode(StatusCodes.Status403Forbidden, error);
    private Guid CorrelationId() => Request.Headers.TryGetValue("X-Correlation-Id", out var value) && Guid.TryParse(value, out var correlation) ? correlation : Guid.CreateVersion7();
}

[ApiController, Route("api/v1/general/countries")] public sealed class CountriesController(IGeoService service) : GeoControllerBase(service)
{ [HttpGet] public Task<ActionResult<PagedResponse<GeoDto>>> List([FromQuery] PagedQueryRequest q, CancellationToken ct) => base.List(GeoResource.Countries,"GEN003",q,ct); [HttpGet("{id:guid}")] public Task<ActionResult<GeoDto>> Get(Guid id,CancellationToken ct)=>base.Get(GeoResource.Countries,"GEN003",id,ct); [HttpPost] public Task<ActionResult<GeoDto>> Create(CreateCountryRequest r,CancellationToken ct)=>base.Create(GeoResource.Countries,"GEN003",r,ct); [HttpPut("{id:guid}")] public Task<ActionResult<GeoDto>> Update(Guid id,UpdateCountryRequest r,CancellationToken ct)=>base.Update(GeoResource.Countries,"GEN003",id,r,ct); [HttpPost("{id:guid}/disable")] public Task<ActionResult<GeoDto>> Disable(Guid id,DisableRequest r,CancellationToken ct)=>base.Disable(GeoResource.Countries,"GEN003",id,r,ct); }
[ApiController, Route("api/v1/general/governorates")] public sealed class GovernoratesController(IGeoService service) : GeoControllerBase(service)
{ [HttpGet] public Task<ActionResult<PagedResponse<GeoDto>>> List([FromQuery] PagedQueryRequest q,CancellationToken ct)=>base.List(GeoResource.Governorates,"GEN004",q,ct); [HttpGet("{id:guid}")] public Task<ActionResult<GeoDto>> Get(Guid id,CancellationToken ct)=>base.Get(GeoResource.Governorates,"GEN004",id,ct); [HttpPost] public Task<ActionResult<GeoDto>> Create(CreateGovernorateRequest r,CancellationToken ct)=>base.Create(GeoResource.Governorates,"GEN004",r,ct); [HttpPut("{id:guid}")] public Task<ActionResult<GeoDto>> Update(Guid id,UpdateGovernorateRequest r,CancellationToken ct)=>base.Update(GeoResource.Governorates,"GEN004",id,r,ct); [HttpPost("{id:guid}/disable")] public Task<ActionResult<GeoDto>> Disable(Guid id,DisableRequest r,CancellationToken ct)=>base.Disable(GeoResource.Governorates,"GEN004",id,r,ct); }
[ApiController, Route("api/v1/general/directorates")] public sealed class DirectoratesController(IGeoService service) : GeoControllerBase(service)
{ [HttpGet] public Task<ActionResult<PagedResponse<GeoDto>>> List([FromQuery] PagedQueryRequest q,CancellationToken ct)=>base.List(GeoResource.Directorates,"GEN005",q,ct); [HttpGet("{id:guid}")] public Task<ActionResult<GeoDto>> Get(Guid id,CancellationToken ct)=>base.Get(GeoResource.Directorates,"GEN005",id,ct); [HttpPost] public Task<ActionResult<GeoDto>> Create(CreateDirectorateRequest r,CancellationToken ct)=>base.Create(GeoResource.Directorates,"GEN005",r,ct); [HttpPut("{id:guid}")] public Task<ActionResult<GeoDto>> Update(Guid id,UpdateDirectorateRequest r,CancellationToken ct)=>base.Update(GeoResource.Directorates,"GEN005",id,r,ct); [HttpPost("{id:guid}/disable")] public Task<ActionResult<GeoDto>> Disable(Guid id,DisableRequest r,CancellationToken ct)=>base.Disable(GeoResource.Directorates,"GEN005",id,r,ct); }
[ApiController, Route("api/v1/general/cities")] public sealed class CitiesController(IGeoService service) : GeoControllerBase(service)
{ [HttpGet] public Task<ActionResult<PagedResponse<GeoDto>>> List([FromQuery] PagedQueryRequest q,CancellationToken ct)=>base.List(GeoResource.Cities,"GEN006",q,ct); [HttpGet("{id:guid}")] public Task<ActionResult<GeoDto>> Get(Guid id,CancellationToken ct)=>base.Get(GeoResource.Cities,"GEN006",id,ct); [HttpPost] public Task<ActionResult<GeoDto>> Create(CreateCityRequest r,CancellationToken ct)=>base.Create(GeoResource.Cities,"GEN006",r,ct); [HttpPut("{id:guid}")] public Task<ActionResult<GeoDto>> Update(Guid id,UpdateCityRequest r,CancellationToken ct)=>base.Update(GeoResource.Cities,"GEN006",id,r,ct); [HttpPost("{id:guid}/disable")] public Task<ActionResult<GeoDto>> Disable(Guid id,DisableRequest r,CancellationToken ct)=>base.Disable(GeoResource.Cities,"GEN006",id,r,ct); }
[ApiController, Route("api/v1/general/areas")] public sealed class AreasController(IGeoService service) : GeoControllerBase(service)
{ [HttpGet] public Task<ActionResult<PagedResponse<GeoDto>>> List([FromQuery] PagedQueryRequest q,CancellationToken ct)=>base.List(GeoResource.Areas,"GEN007",q,ct); [HttpGet("{id:guid}")] public Task<ActionResult<GeoDto>> Get(Guid id,CancellationToken ct)=>base.Get(GeoResource.Areas,"GEN007",id,ct); [HttpPost] public Task<ActionResult<GeoDto>> Create(CreateAreaRequest r,CancellationToken ct)=>base.Create(GeoResource.Areas,"GEN007",r,ct); [HttpPut("{id:guid}")] public Task<ActionResult<GeoDto>> Update(Guid id,UpdateAreaRequest r,CancellationToken ct)=>base.Update(GeoResource.Areas,"GEN007",id,r,ct); [HttpPost("{id:guid}/disable")] public Task<ActionResult<GeoDto>> Disable(Guid id,DisableRequest r,CancellationToken ct)=>base.Disable(GeoResource.Areas,"GEN007",id,r,ct); }
