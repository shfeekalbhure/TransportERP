using Microsoft.AspNetCore.Mvc;
using TransportERP.Application.Accounting;
using TransportERP.Contracts.Accounting;

namespace TransportERP.Api.Controllers;

[ApiController]
[Route("api/accounting/journal-report")]
public sealed class JournalReportController(IJournalReportQueryService service) : ControllerBase
{
    [HttpGet]
    public Task<JournalReportResponse> Get([FromQuery] JournalReportQuery query, CancellationToken cancellationToken) => service.QueryAsync(query, cancellationToken);
}
