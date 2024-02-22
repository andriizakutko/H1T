namespace Application.Interfaces;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionsAsync(Guid userId);
}