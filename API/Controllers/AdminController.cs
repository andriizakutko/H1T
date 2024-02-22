using Application.Interfaces;
using Common.Requests;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[HasPermission(Permissions.Admin)]
[Authorize]
[Route("api/admin")]
public class AdminController(IAdminService adminService) : BaseApiController
{
    [HttpGet("get-users")]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await adminService.GetUsers());
    }

    [HttpPost("add-user-to-permission")]
    public async Task<IActionResult> AddUserToPermission(AddUserToPermissionRequest addUserToPermissionRequest)
    {
        return HandleResult(
            await adminService.AddUserToPermission(await adminService.GetUser(addUserToPermissionRequest.Email), addUserToPermissionRequest.PermissionName));
    }

    [HttpGet("get-users-permissions")]
    public async Task<IActionResult> GetUsersPermissions()
    {
        return HandleResult(await adminService.GetUsersPermissions());
    }
    
    [HttpPost("delete-user-from-permission")]
    public async Task<IActionResult> DeleteUserFromPermission(DeleteUserFromPermissionRequest deleteUserFromPermissionRequest)
    {
        return HandleResult(
            await adminService.DeleteUserFromPermission(deleteUserFromPermissionRequest.Email, deleteUserFromPermissionRequest.PermissionName));
    }
}