using AuthLearn.Constants;
using AuthLearn.Models;
using AuthLearn.Policies;
using AuthLearn.Policies.Attributes;
using AuthLearn.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthLearn.Controllers;

[ApiController]
[Route("/permissions")]
[Authorize("IsAuthenticatedWithLocalCookieScheme")]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionsRepository _repository;

    public PermissionsController(IPermissionsRepository repository)
    {
        _repository = repository;
    }


    [HttpGet("")]
    [RequirePermission(AccessScopeConstants.Read)]
    public async Task<IActionResult> GetPermissions()
    {
        var permissions = await _repository.GetPermissionsAsync();
        return Ok(permissions);
    }

    [HttpGet("{uid}")]
    [RequirePermission(AccessScopeConstants.Read)]
    public async Task<IActionResult> GetUserPermissions([FromRoute] int uid)
    {
        var permissions = await _repository.GetUserPermissionsAsync(uid);
        return Ok(permissions);
    }

    [HttpPost]
    [RequirePermission(AccessScopeConstants.Write)]
    public async Task<IActionResult> AddUserPermissions([FromBody] UserPermissionModel model)
    {
        await _repository.AddUserPermissionAsync(model.UserId, model.Permissions);
        return Ok();
    }

    [HttpPatch]
    [RequirePermission(AccessScopeConstants.Write, AccessScopeConstants.Edit)]
    public async Task<IActionResult> UpdateUserPermissions([FromBody] UserPermissionModel model)
    {
        await _repository.UpdateUserPermissionAsync(model);
        return Ok();
    }

    [HttpDelete("{uid}")]
    [RequirePermission(AccessScopeConstants.Delete)]
    public async Task<IActionResult> ClearUserPermissions([FromRoute] int uid)
    {
        await _repository.ClearUserPermissionsAsync(uid);
        return Ok();
    }
}