using Application.Interfaces;
using Common.Requests;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/admin")]
public class AdminController(IAdminService adminService) : BaseApiController
{
    [HasPermission(Permissions.Admin)]
    [HttpGet("get-users")]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await adminService.GetUsers());
    }

    [HasPermission(Permissions.SysAdmin)]
    [HttpPost("add-user-to-permission")]
    public async Task<IActionResult> AddUserToPermission(AddUserToPermissionRequest addUserToPermissionRequest)
    {
        return HandleResult(
            await adminService.AddUserToPermission(await adminService.GetUser(addUserToPermissionRequest.Email), addUserToPermissionRequest.PermissionName));
    }

    [HasPermission(Permissions.SysAdmin)]
    [HttpGet("get-users-permissions")]
    public async Task<IActionResult> GetUsersPermissions()
    {
        return HandleResult(await adminService.GetUsersPermissions());
    }
    
    [HasPermission(Permissions.SysAdmin)]
    [HttpDelete("delete-user-from-permission")]
    public async Task<IActionResult> DeleteUserFromPermission(DeleteUserFromPermissionRequest deleteUserFromPermissionRequest)
    {
        return HandleResult(
            await adminService.DeleteUserFromPermission(deleteUserFromPermissionRequest.Email, deleteUserFromPermissionRequest.PermissionName));
    }
}