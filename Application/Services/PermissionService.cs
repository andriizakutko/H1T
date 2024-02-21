using Domain.Interfaces;

namespace Application.Services;

public class PermissionService(IPermissionStore permissionStore) : IPermissionService
{
    public async Task<HashSet<string>> GetPermissionsAsync(Guid userId)
    {
        return await permissionStore.GetUserPermissions(userId);
    }
}