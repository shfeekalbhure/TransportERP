using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportERP.Api.Authorization;
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
    [Authorize(Policy = LookupClaims.ReadPolicy)]
    public ActionResult<IReadOnlyList<LookupItem>> Lookup(
        [FromQuery] string? query,
        [FromQuery] string? company = null,
        [FromQuery] string? branch = null)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("A lookup query is required; full-table lookup is not permitted.");
        }

        // Header values are deliberately ignored: all authority originates in the authenticated principal.
        if (!User.TryGetTrustedScope(out var trustedCompany, out var trustedBranch) ||
            !User.HasClaim(LookupClaims.Permission, LookupClaims.ReadPermission))
        {
            return Forbid();
        }

        if ((!string.IsNullOrWhiteSpace(company) && !string.Equals(company, trustedCompany, StringComparison.Ordinal)) ||
            (!string.IsNullOrWhiteSpace(branch) && !string.Equals(branch, trustedBranch, StringComparison.Ordinal)))
        {
            return Forbid();
        }

        return Ok(lookupProvider.Search(query, new LookupAccessContext(trustedCompany, trustedBranch, true)));
    }
}
