using Application.Interfaces;
using Common.Results;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly ILogger<PermissionService> _logger;

    public PermissionService(
        IPermissionRepository permissionRepository,
        ILogger<PermissionService> logger)
    {
        _permissionRepository = permissionRepository;
        _logger = logger;
    }
    
    public async Task<Result<HashSet<string>>> GetPermissions(Guid userId)
    {
        try
        {
            return Result<HashSet<string>>.Success(await _permissionRepository.GetUserPermissions(userId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<HashSet<string>>
                .Failure(new Error(ErrorCodes.Permission.GetPermissions, ErrorMessages.ServiceError));
        }
    }
}