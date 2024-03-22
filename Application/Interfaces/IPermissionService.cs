using Common.Results;

namespace Application.Interfaces;

public interface IPermissionService
{
    Task<Result<HashSet<string>>> GetPermissions(Guid userId);
}