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

    public async Task<Result> AddUserToPermission(User user, string permissionName)
    {
        var result = await CheckPermission(user, permissionName);

        if (result.Value)
        {
            return Result<bool>.Failure(new Error("AdminService.AddUserToPermission", "Permission has already added"));
        }

        await _permissionStore.AddUserPermission(user, permissionName);

        return Result.Success();
    }

    public async Task<User> GetUser(string email)
    {
        return await _userStore.GetByEmail(email);
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
        var user = await _userStore.GetByEmail(email);

        if (user is null)
        {
            return Result.Failure(new Error("AdminService.DeleteUserFromPermission", "User was not found"));
        }

        if (user.Permissions.All(p => p.Name != permissionName))
        {
            return Result.Failure(new Error("AdminService.DeleteUserFromPermission", "User doesn't have this permission to delete"));
        }

        var isDeleted = await _permissionStore.DeleteUserFromPermission(user, permissionName);

        return isDeleted ? Result.Success() : Result.Failure(new Error("AdminService.DeleteUserFromPermission", "Delete user permission is failed"));
    }

    private async Task<Result<bool>> CheckPermission(User user, string permissionName)
    {
        var isAdded = await _permissionStore.IsPermissionAdded(user, permissionName);

        return Result<bool>.Success(isAdded);
    }
}