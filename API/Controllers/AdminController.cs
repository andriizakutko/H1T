using Common.Requests;
using Domain.Interfaces;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[HasPermission(Permissions.Admin)]
[Authorize]
[Route("api/admin")]
public class AdminController : BaseApiController
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("get-users")]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await _adminService.GetUsers());
    }

    [HttpPost("add-user-to-permission")]
    public async Task<IActionResult> AddUserToPermission(AddUserToPermissionRequest addUserToPermissionRequest)
    {
        return HandleResult(
            await _adminService.AddUserToPermission(await _adminService.GetUser(addUserToPermissionRequest.Email), addUserToPermissionRequest.PermissionName));
    }

    [HttpGet("get-users-permissions")]
    public async Task<IActionResult> GetUsersPermissions()
    {
        return HandleResult(await _adminService.GetUsersPermissions());
    }
    
    [HttpPost("delete-user-from-permission")]
    public async Task<IActionResult> DeleteUserFromPermission(DeleteUserFromPermissionRequest deleteUserFromPermissionRequest)
    {
        return HandleResult(
            await _adminService.DeleteUserFromPermission(deleteUserFromPermissionRequest.Email, deleteUserFromPermissionRequest.PermissionName));
    }
}