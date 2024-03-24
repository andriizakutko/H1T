using Application.Interfaces;
using Common.Results;
using Common.ServiceResults;
using Domain;
using Infrastructure.Cache;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Services;

public class ResourceService : IResourceService
{
    private readonly IResourceRepository _repository;
    private readonly ICacheProvider _cache;
    private readonly ILogger<ResourceService> _logger;

    public ResourceService(
        IResourceRepository repository,
        ICacheProvider cache,
        ILogger<ResourceService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }
    
    public async Task<Result<IEnumerable<ValueResult>>> GetTransportTypes()
    {
        try
        {
            var list = _cache.Get<IEnumerable<ValueResult>>(CacheKeys.Transport.Types);

            if (list is not null)
            {
                return Result<IEnumerable<ValueResult>>.Success(list);
            }
            
            var transportTypes = await _repository.GetTransportTypes();
                
            list = transportTypes.Select(x => new ValueResult
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
                
            _cache.Set(CacheKeys.Transport.Types, list);

            return Result<IEnumerable<ValueResult>>.Success(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<IEnumerable<ValueResult>>.Failure(new Error(ErrorCodes.Resource.GetTransportTypes, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<IEnumerable<ValueResult>>> GetTransportMakesByTransportTypeId(Guid id)
    {
        try
        {
            var key = $"{CacheKeys.Transport.Makes}-{id}";
            
            var list = _cache.Get<IEnumerable<ValueResult>>(key);

            if (list is not null)
            {
                return Result<IEnumerable<ValueResult>>.Success(list);
            }
            
            var transportType = await _repository.GetTransportTypeById(id);

            list = transportType.TransportTypeMakes.Select(x =>
                new ValueResult()
                {
                    Id = x.TransportMake.Id,
                    Name = x.TransportMake.Name
                }).ToList();
            
            _cache.Set(key, list);

            return Result<IEnumerable<ValueResult>>.Success(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<IEnumerable<ValueResult>>.Failure(
                new Error(ErrorCodes.Resource.GetTransportMakesByTransportTypeId, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<IEnumerable<ValueResult>>> GetTransportBodyTypesByTransportTypeId(Guid id)
    {
        try
        {
            var key = $"{CacheKeys.Transport.BodyTypes}-{id}";

            var list = _cache.Get<IEnumerable<ValueResult>>(key);

            if (list is not null)
            {
                return Result<IEnumerable<ValueResult>>.Success(list);
            }
            
            var transportType = await _repository.GetTransportTypeById(id);

            list = transportType.TransportTypeBodyTypes.Select(x =>
                new ValueResult()
                {
                    Id = x.TransportBodyType.Id,
                    Name = x.TransportBodyType.Name
                }).ToList();
            
            _cache.Set(key, list);
            
            return Result<IEnumerable<ValueResult>>.Success(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<IEnumerable<ValueResult>>.Failure(
                new Error(ErrorCodes.Resource.GetTransportBodyTypesByTransportTypeId, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<IEnumerable<ValueResult>>> GetTransportModelsByTransportMakeId(Guid id)
    {
        try
        {
            var key = $"{CacheKeys.Transport.Models}-{id}";

            var list = _cache.Get<IEnumerable<ValueResult>>(key);

            if (list is not null)
            {
                return Result<IEnumerable<ValueResult>>.Success(list);
            }
            
            var transportMake = await _repository.GetTransportMakeById(id);

            list = transportMake.TransportMakeModels.Select(x =>
                new ValueResult()
                {
                    Id = x.TransportModel.Id,
                    Name = x.TransportModel.Name
                }).ToList();
            
            _cache.Set(key, list);
            
            return Result<IEnumerable<ValueResult>>.Success(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<IEnumerable<ValueResult>>.Failure(
                new Error(ErrorCodes.Resource.GetTransportModelsByTransportMakeId, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<IEnumerable<ModeratorOverviewStatus>>> GetModeratorOverviewStatuses()
    {
        try
        {
            var list = _cache.Get<IEnumerable<ModeratorOverviewStatus>>(CacheKeys.Moderator.OverviewStatuses);

            if (list is not null)
            {
                return Result<IEnumerable<ModeratorOverviewStatus>>.Success(list);
            }

            var overviewStatuses = await _repository.GetModeratorOverviewStatuses();

            list = overviewStatuses.ToList();

            _cache.Set(CacheKeys.Moderator.OverviewStatuses, list);
            
            return Result<IEnumerable<ModeratorOverviewStatus>>.Success(list);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<IEnumerable<ModeratorOverviewStatus>>.Failure(new Error(ErrorCodes.Resource.GetModeratorOverviewStatuses,
                ErrorMessages.ServiceError));
        }
    }
}