using Microsoft.AspNetCore.Mvc;
using TransportERP.Application.Setup.VehicleTypes;
using TransportERP.Contracts.Setup.VehicleTypes;

namespace TransportERP.Api.Controllers;

[ApiController]
[Route("api/setup/vehicle-types")]
public sealed class VehicleTypesController : ControllerBase
{
    private readonly IVehicleTypeService _service;

    public VehicleTypesController(IVehicleTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(VehicleTypeSearchResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VehicleTypeSearchResponse), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<VehicleTypeSearchResponse>> SearchAsync(
        [FromQuery] string? query,
        [FromQuery] VehicleTypeStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        CancellationToken cancellationToken = default)
    {
        var response = await _service.SearchAsync(new VehicleTypeSearchRequest(query, status, page, pageSize), cancellationToken);
        return response.StorageAvailable ? Ok(response) : StatusCode(StatusCodes.Status503ServiceUnavailable, response);
    }

    [HttpPost]
    public async Task<ActionResult<VehicleTypeCommandResponse>> CreateAsync(
        [FromBody] CreateVehicleTypeRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.CreateAsync(request, cancellationToken);
        return ToActionResult(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<VehicleTypeCommandResponse>> UpdateAsync(
        Guid id,
        [FromBody] UpdateVehicleTypeRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.UpdateAsync(id, request, cancellationToken);
        return ToActionResult(response);
    }

    [HttpPost("{id:guid}/suspend")]
    public async Task<ActionResult<VehicleTypeCommandResponse>> SuspendAsync(Guid id, CancellationToken cancellationToken)
        => ToActionResult(await _service.SuspendAsync(id, cancellationToken));

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<VehicleTypeCommandResponse>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        => ToActionResult(await _service.DeleteAsync(id, cancellationToken));

    private ActionResult<VehicleTypeCommandResponse> ToActionResult(VehicleTypeCommandResponse response)
    {
        if (response.Succeeded) return Ok(response);
        if (!response.StorageAvailable) return StatusCode(StatusCodes.Status503ServiceUnavailable, response);
        return BadRequest(response);
    }
}
