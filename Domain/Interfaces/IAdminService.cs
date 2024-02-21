using Common.DTOs;
using Common.Responses;
using Common.Results;

namespace Domain.Interfaces;

public interface IAdminService
{
    Task<Result<IEnumerable<UserDto>>> GetUsers();
    Task<Result> AddUserToPermission(User user, string permissionName);
    Task<Result<IEnumerable<UsersPermissionsResponse>>> GetUsersPermissions();
    Task<Result> DeleteUserFromPermission(string email, string permissionName);
    Task<User> GetUser(string email);
}