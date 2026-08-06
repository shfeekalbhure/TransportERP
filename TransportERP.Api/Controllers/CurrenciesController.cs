using Microsoft.AspNetCore.Mvc;
using TransportERP.Application.Setup.Currencies;
using TransportERP.Contracts.Setup.Currencies;

namespace TransportERP.Api.Controllers;

[ApiController]
[Route("api/setup/currencies")]
public sealed class CurrenciesController(ICurrencyService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CurrencySearchResponse>> SearchAsync([FromQuery] string? query, [FromQuery] CurrencyStatus? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 25, CancellationToken cancellationToken = default)
    {
        var response = await service.SearchAsync(new CurrencySearchRequest(query, status, page, pageSize), cancellationToken);
        return response.StorageAvailable ? Ok(response) : StatusCode(StatusCodes.Status503ServiceUnavailable, response);
    }

    [HttpPost]
    public async Task<ActionResult<CurrencyCommandResponse>> CreateAsync(CreateCurrencyRequest request, CancellationToken cancellationToken)
        => ToResult(await service.CreateAsync(request, cancellationToken));

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CurrencyCommandResponse>> UpdateAsync(Guid id, UpdateCurrencyRequest request, CancellationToken cancellationToken)
        => ToResult(await service.UpdateAsync(id, request, cancellationToken));

    [HttpPost("{id:guid}/suspend")]
    public async Task<ActionResult<CurrencyCommandResponse>> SuspendAsync(Guid id, CancellationToken cancellationToken)
        => ToResult(await service.SuspendAsync(id, cancellationToken));

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<CurrencyCommandResponse>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        => ToResult(await service.DeleteAsync(id, cancellationToken));

    private ActionResult<CurrencyCommandResponse> ToResult(CurrencyCommandResponse result)
        => result.Succeeded ? Ok(result) : !result.StorageAvailable ? StatusCode(StatusCodes.Status503ServiceUnavailable, result) : BadRequest(result);
}
