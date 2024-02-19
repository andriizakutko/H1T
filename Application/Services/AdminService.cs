using Common.DTOs;
using Common.Responses;
using Common.Results;
using Domain;
using Domain.Interfaces;

namespace Application.Services;

public class AdminService : IAdminService
{
    private readonly IUserStore _userStore;
    private readonly IPermissionStore _permissionStore;

    public AdminService(IUserStore userStore, IPermissionStore permissionStore)
    {
        _userStore = userStore;
        _permissionStore = permissionStore;
    }

    public async Task<Result<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await _userStore.GetAll();

        return Result<IEnumerable<UserDto>>.Success(users.Select(u => new UserDto()
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            Country = u.Country,
            City = u.City,
            Address = u.Address,
            IsActive = u.IsActive
        }));
    }

    public async Task<Result> AddUserToPermission(string email, string permissionName)
    {
        var permission = await _permissionStore.GetByName(permissionName);

        if (permission is null)
        {
            return Result<bool>.Failure(new Error("AdminService.AddUserToPermission", "Permission was not found"));
        }
        
        var user = await _userStore.GetByEmail(email);

        if (user is null)
        {
            return Result.Failure(new Error("AdminService.AddUserToPermission", "User was not found"));
        }

        var result = await CheckPermission(user, permission);

        if (result.Value)
        {
            return Result<bool>.Failure(new Error("AdminService.AddUserToPermission", "Permission has already added"));
        }

        await _permissionStore.AddUserPermission(user.Id, permission.Id);

        return Result.Success();
    }

    public async Task<Result<IEnumerable<UsersPermissionsResponse>>> GetUsersPermissions()
    {
        var userPermissions = await _permissionStore.GetAll();

        var permissionsDictionary = userPermissions.GroupBy(x => x.Email).ToDictionary(x => x.Key, y => y.ToArray());

        var resultList = new List<UsersPermissionsResponse>();

        foreach (var item in permissionsDictionary)
        {
            var userPermissionsResponseValue = new UsersPermissionsResponse()
            {
                Email = item.Key,
                Permissions = new List<string>()
            };

            foreach (var permission in item.Value)
            {
                userPermissionsResponseValue.Permissions.Add(permission.PermissionName);
            }
            
            resultList.Add(userPermissionsResponseValue);
        }

        return Result<IEnumerable<UsersPermissionsResponse>>.Success(resultList);
    }

    public async Task<Result> DeleteUserFromPermission(string email, string permissionName)
    {
        var permission = await _permissionStore.GetByName(permissionName);
            
        if (permission is null)
        {
            return Result<bool>.Failure(new Error("AdminService.DeleteUserFromPermission", "Permission was not found"));
        }
        
        var user = await _userStore.GetByEmail(email);

        if (user is null)
        {
            return Result.Failure(new Error("AdminService.DeleteUserFromPermission", "User was not found"));
        }

        var isDeleted = await _permissionStore.DeleteUserFromPermission(user.Id, permission.Id);

        return isDeleted ? Result.Success() : Result.Failure(new Error("AdminService.DeleteUserFromPermission", "Delete user permission is failed"));
    }

    private async Task<Result<bool>> CheckPermission(User user, Permission permission)
    {
        var isAdded = await _permissionStore.IsPermissionAdded(user.Id, permission.Id);

        return Result<bool>.Success(isAdded);
    }
}