using Domain.Interfaces;
using Infrastructure.Data;

namespace Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionStore _permissionStore;

    public PermissionService(DatabaseContext context, IPermissionStore permissionStore)
    {
        _permissionStore = permissionStore;
    }

    public async Task<HashSet<string>> GetPermissionsAsync(Guid userId)
    {
        return await _permissionStore.GetUserPermissions(userId);
    }
}