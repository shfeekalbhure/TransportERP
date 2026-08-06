using Microsoft.AspNetCore.Mvc;
using TransportERP.Application.AccessControl;
using TransportERP.Contracts.AccessControl;

namespace TransportERP.Api.Controllers;

[ApiController]
[Route("api/access-control")]
public sealed class AccessControlController(IAccessControlService service) : ControllerBase
{
    [HttpGet("roles")]
    public async Task<ActionResult<PagedResult<RoleDto>>> SearchRoles([FromQuery] PagedQuery query, CancellationToken cancellationToken) =>
        Ok(await service.SearchRolesAsync(query, cancellationToken));

    [HttpGet("permissions")]
    public async Task<ActionResult<PagedResult<PermissionDto>>> SearchPermissions([FromQuery] PagedQuery query, CancellationToken cancellationToken) =>
        Ok(await service.SearchPermissionsAsync(query, cancellationToken));

    [HttpGet("data-scopes")]
    public async Task<ActionResult<PagedResult<DataScopeDto>>> SearchDataScopes([FromQuery] PagedQuery query, CancellationToken cancellationToken) =>
        Ok(await service.SearchDataScopesAsync(query, cancellationToken));

    [HttpPost("roles")]
    public async Task<ActionResult<OperationResult>> CreateRole([FromBody] CreateRoleRequest request, CancellationToken cancellationToken) =>
        ToActionResult(await service.CreateRoleAsync(request, cancellationToken));

    [HttpPost("permissions")]
    public async Task<ActionResult<OperationResult>> CreatePermission([FromBody] CreatePermissionRequest request, CancellationToken cancellationToken) =>
        ToActionResult(await service.CreatePermissionAsync(request, cancellationToken));

    [HttpPost("data-scopes")]
    public async Task<ActionResult<OperationResult>> CreateDataScope([FromBody] CreateDataScopeRequest request, CancellationToken cancellationToken) =>
        ToActionResult(await service.CreateDataScopeAsync(request, cancellationToken));

    private ActionResult<OperationResult> ToActionResult(OperationResult result) =>
        result.Succeeded ? Ok(result) : StatusCode(StatusCodes.Status503ServiceUnavailable, result);
}