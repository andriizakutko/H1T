using Application.Interfaces;
using Persistence.Interfaces;

namespace Application.Services;

public class PermissionService(IPermissionRepository permissionRepository) : IPermissionService
{
    public async Task<HashSet<string>> GetPermissionsAsync(Guid userId)
    {
        return await permissionRepository.GetUserPermissions(userId);
    }
}