using Application.Interfaces;
using Common.DTOs;
using Common.Options;
using Common.Responses;
using Common.Results;
using Domain;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Persistence.Interfaces;

namespace Application.Services;

public class AdminService(
        IUserRepository userRepository, 
        IPermissionRepository permissionRepository,
        IOptions<AdminOptions> options,
        IHttpContextAccessor contextAccessor)
    : IAdminService
{
    public async Task<Result<IEnumerable<UserDto>>> GetUsers()
    {
        var users = await userRepository.GetAll();
        
        return Result<IEnumerable<UserDto>>.Success(users
            .Where(u => u.Email != options.Value.Email && u.Email != contextAccessor.GetEmail())
            .Select(u => new UserDto()
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
        if (user is null)
        {
            return Result.Failure(new Error("AdminService.AddUserToPermission", "User doesn't exist"));
        }
        
        var result = await CheckPermission(user, permissionName);

        if (result.Value)
        {
            return Result.Failure(new Error("AdminService.AddUserToPermission", "Permission has already added"));
        }

        await permissionRepository.AddUserPermission(user, permissionName);

        return Result.Success();
    }

    public async Task<User> GetUser(string email)
    {
        return await userRepository.GetByEmail(email);
    }

    public async Task<Result<IEnumerable<UsersPermissionsResult>>> GetUsersPermissions()
    {
        var userPermissions = await permissionRepository.GetAll();

        var permissionsDictionary = userPermissions.GroupBy(x => x.Email).ToDictionary(x => x.Key, y => y.ToArray());

        var resultList = new List<UsersPermissionsResult>();

        foreach (var item in permissionsDictionary)
        {
            var userPermissionsResponseValue = new UsersPermissionsResult()
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

        return Result<IEnumerable<UsersPermissionsResult>>.Success(resultList);
    }

    public async Task<Result> DeleteUserFromPermission(string email, string permissionName)
    {
        var user = await userRepository.GetByEmail(email);

        if (user is null)
        {
            return Result.Failure(new Error("AdminService.DeleteUserFromPermission", "User was not found"));
        }

        var userPermissions = await userRepository.GetUserPermissions(email);

        if (userPermissions.All(p => p.Permission.Name != permissionName))
        {
            return Result.Failure(new Error("AdminService.DeleteUserFromPermission", "User doesn't have this permission to delete"));
        }

        var isDeleted = await permissionRepository.DeleteUserFromPermission(user, permissionName);

        return isDeleted ? Result.Success() : Result.Failure(new Error("AdminService.DeleteUserFromPermission", "Delete user permission is failed"));
    }

    private async Task<Result<bool>> CheckPermission(User user, string permissionName)
    {
        var isAdded = await permissionRepository.IsPermissionAdded(user, permissionName);

        return Result<bool>.Success(isAdded);
    }
}