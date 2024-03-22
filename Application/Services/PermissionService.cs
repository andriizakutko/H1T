using Application.Interfaces;
using Common.Results;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Services;

public class PermissionService(
    IPermissionRepository permissionRepository,
    ILogger logger) : IPermissionService
{
    public async Task<Result<HashSet<string>>> GetPermissions(Guid userId)
    {
        try
        {
            return Result<HashSet<string>>.Success(await permissionRepository.GetUserPermissions(userId));
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<HashSet<string>>
                .Failure(new Error(ErrorCodes.Permission.GetPermissions, ErrorMessages.ServiceError));
        }
    }
}