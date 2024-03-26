using Common.Requests;
using Common.Responses;
using Common.Results;
using Domain;

namespace Application.Interfaces;

public interface IAdminService
{
    Task<Result<IEnumerable<UserDetailsResponse>>> GetUsers();
    Task<Result> AddUserToPermission(AddUserToPermissionRequest request);
    Task<Result<IEnumerable<UsersPermissionsResult>>> GetUsersPermissions();
    Task<Result> DeleteUserFromPermission(DeleteUserFromPermissionRequest request);
}