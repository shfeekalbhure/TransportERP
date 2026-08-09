using Microsoft.AspNetCore.Mvc;
using TransportERP.Api.Services;

namespace TransportERP.Api.Controllers;

[ApiController]
[Route("api/operations/downstream-status")]
public sealed class DownstreamStatusController(IDownstreamStatusService downstreamStatusService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        using var response = await downstreamStatusService.ProbeAsync(cancellationToken);
        return StatusCode((int)response.StatusCode);
    }
}
