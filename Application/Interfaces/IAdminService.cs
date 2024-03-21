using Common.Responses;
using Common.Results;
using Domain;

namespace Application.Interfaces;

public interface IAdminService
{
    Task<Result<IEnumerable<UserDetailsResponse>>> GetUsers();
    Task<Result> AddUserToPermission(string email, string permissionName);
    Task<Result<IEnumerable<UsersPermissionsResult>>> GetUsersPermissions();
    Task<Result> DeleteUserFromPermission(string email, string permissionName);
}