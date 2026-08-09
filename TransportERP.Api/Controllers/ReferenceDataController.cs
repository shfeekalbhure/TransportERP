using Microsoft.AspNetCore.Mvc;
using TransportERP.Api.Policies;
using TransportERP.Api.ReferenceData;

namespace TransportERP.Api.Controllers;

[ApiController]
[Route("api/reference-data")]
public sealed class ReferenceDataController(IReferenceLookupProvider lookupProvider) : ControllerBase
{
    [HttpGet("records")]
    public ActionResult<IReadOnlyList<int>> GetRecords([FromQuery] int? pageSize)
    {
        var effectivePageSize = RequestLimitPolicy.NormalizePageSize(pageSize);
        return Ok(Enumerable.Range(1, 1_000).Take(effectivePageSize).ToArray());
    }

    [HttpGet("lookup")]
    public ActionResult<IReadOnlyList<LookupItem>> Lookup([FromQuery] string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("A lookup query is required; full-table lookup is not permitted.");
        }

        var scope = Request.Headers["X-TransportERP-Scope"].FirstOrDefault();
        var permitted = string.Equals(Request.Headers["X-TransportERP-Permission"].FirstOrDefault(), "lookup.read", StringComparison.Ordinal);
        if (string.IsNullOrWhiteSpace(scope) || !permitted)
        {
            return Forbid();
        }

        return Ok(lookupProvider.Search(query, new LookupAccessContext(scope, permitted)));
    }
}
