using Application.Interfaces;
using Common.Options;
using Common.Responses;
using Common.Results;
using Domain;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence.Interfaces;

namespace Application.Services;

public class AdminService(
        IUserRepository userRepository, 
        IPermissionRepository permissionRepository,
        IOptions<AdminOptions> options,
        IHttpContextAccessor contextAccessor,
        ILogger logger)
    : IAdminService
{
    public async Task<Result<IEnumerable<UserDetailsResponse>>> GetUsers()
    {
        try
        {
            var users = await userRepository.GetAll();
        
            return Result<IEnumerable<UserDetailsResponse>>.Success(users
                .Where(u => u.Email != options.Value.Email && u.Email != contextAccessor.GetEmail())
                .Select(u => new UserDetailsResponse()
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
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<IEnumerable<UserDetailsResponse>>.Failure(new Error(ErrorCodes.Admin.GetUsers,
                ErrorMessages.ServiceError));
        }
    }

    public async Task<Result> AddUserToPermission(User user, string permissionName)
    {
        try
        {
            if (user is null)
            {
                return Result.Failure(new Error(ErrorCodes.Admin.AddUserToPermission, ErrorMessages.User.UserNotExists));
            }
        
            var result = await CheckPermission(user, permissionName);

            if (result.Value)
            {
                return Result.Failure(new Error(ErrorCodes.Admin.AddUserToPermission, ErrorMessages.Admin.PermissionHasAlreadyAdded));
            }

            await permissionRepository.AddUserPermission(user, permissionName);

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.Admin.AddUserToPermission, ErrorMessages.ServiceError));
        }
    }

    public async Task<User> GetUser(string email)
    {
        return await userRepository.GetByEmail(email);
    }

    public async Task<Result<IEnumerable<UsersPermissionsResult>>> GetUsersPermissions()
    {
        try
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
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<IEnumerable<UsersPermissionsResult>>.Failure(new Error(ErrorCodes.Admin.GetUsersPermissions,
                ErrorMessages.ServiceError));
        }
    }

    public async Task<Result> DeleteUserFromPermission(string email, string permissionName)
    {
        try
        {
            var user = await userRepository.GetByEmail(email);

            if (user is null)
            {
                return Result.Failure(new Error(ErrorCodes.Admin.DeleteUserFromPermission, ErrorMessages.User.UserNotFound));
            }

            var userPermissions = await userRepository.GetUserPermissions(email);

            if (userPermissions.All(p => p.Permission.Name != permissionName))
            {
                return Result.Failure(new Error(ErrorCodes.Admin.DeleteUserFromPermission, ErrorMessages.Admin.UserNotHavePermissionToDelete));
            }

            var isDeleted = await permissionRepository.DeleteUserFromPermission(user, permissionName);

            return isDeleted ? Result.Success() : Result.Failure(new Error(ErrorCodes.Admin.DeleteUserFromPermission, 
                    ErrorMessages.Admin.DeleteUserFromPermissionFailed));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.Admin.DeleteUserFromPermission, ErrorMessages.ServiceError));
        }
    }

    private async Task<Result<bool>> CheckPermission(User user, string permissionName)
    {
        var isAdded = await permissionRepository.IsPermissionAdded(user, permissionName);

        return Result<bool>.Success(isAdded);
    }
}