using Microsoft.AspNetCore.Mvc;
using TransportERP.Application.Setup.ExchangeRates;
using TransportERP.Contracts.Setup.ExchangeRates;

namespace TransportERP.Api.Controllers;

[ApiController]
[Route("api/setup/exchange-rates")]
public sealed class ExchangeRatesController(IExchangeRateService service) : ControllerBase
{
    [HttpGet] public async Task<ActionResult<ExchangeRateSearchResponse>> SearchAsync([FromQuery] string? query,[FromQuery] ExchangeRateStatus? status,[FromQuery] int page=1,[FromQuery] int pageSize=25,CancellationToken cancellationToken=default){var r=await service.SearchAsync(new(query,status,page,pageSize),cancellationToken);return r.StorageAvailable?Ok(r):StatusCode(StatusCodes.Status503ServiceUnavailable,r);}
    [HttpPost] public async Task<ActionResult<ExchangeRateCommandResponse>> CreateAsync(CreateExchangeRateRequest request,CancellationToken cancellationToken)=>Result(await service.CreateAsync(request,cancellationToken));
    [HttpPut("{id:guid}")] public async Task<ActionResult<ExchangeRateCommandResponse>> UpdateAsync(Guid id,UpdateExchangeRateRequest request,CancellationToken cancellationToken)=>Result(await service.UpdateAsync(id,request,cancellationToken));
    [HttpPost("{id:guid}/suspend")] public async Task<ActionResult<ExchangeRateCommandResponse>> SuspendAsync(Guid id,CancellationToken cancellationToken)=>Result(await service.SuspendAsync(id,cancellationToken));
    [HttpDelete("{id:guid}")] public async Task<ActionResult<ExchangeRateCommandResponse>> DeleteAsync(Guid id,CancellationToken cancellationToken)=>Result(await service.DeleteAsync(id,cancellationToken));
    private ActionResult<ExchangeRateCommandResponse> Result(ExchangeRateCommandResponse r)=>r.Succeeded?Ok(r):!r.StorageAvailable?StatusCode(StatusCodes.Status503ServiceUnavailable,r):BadRequest(r);
}