using Application.Interfaces;
using Common.Requests;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/admin")]
public class AdminController(IAdminService adminService) : BaseApiController
{
    [HttpGet("get-users")]
    [HasPermission(Permissions.Admin)]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await adminService.GetUsers());
    }
    
    [HttpPost("add-user-to-permission")]
    [HasPermission(Permissions.SysAdmin)]
    public async Task<IActionResult> AddUserToPermission(AddUserToPermissionRequest addUserToPermissionRequest)
    {
        return HandleResult(
            await adminService.AddUserToPermission(addUserToPermissionRequest));
    }
    
    [HttpGet("get-users-permissions")]
    [HasPermission(Permissions.SysAdmin)]
    public async Task<IActionResult> GetUsersPermissions()
    {
        return HandleResult(await adminService.GetUsersPermissions());
    }
    
    [HttpDelete("delete-user-from-permission")]
    [HasPermission(Permissions.SysAdmin)]
    public async Task<IActionResult> DeleteUserFromPermission(DeleteUserFromPermissionRequest deleteUserFromPermissionRequest)
    {
        return HandleResult(
            await adminService.DeleteUserFromPermission(deleteUserFromPermissionRequest));
    }
}