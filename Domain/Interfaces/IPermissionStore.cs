using Domain.StoreResults;

namespace Domain.Interfaces;

public interface IPermissionStore
{
    Task<Permission> GetByName(string name);
    Task AddUserPermission(Guid userId, Guid permissionId);
    Task<bool> IsPermissionAdded(Guid userId, Guid permissionId);
    Task<IEnumerable<UserPermissionResult>> GetAll();
    Task<bool> DeleteUserFromPermission(Guid userId, Guid permissionId);
    Task<HashSet<string>> GetUserPermissions(Guid userId);
}