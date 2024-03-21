using Application.Interfaces;
using Common.Results;
using Common.ServiceResults;
using Domain;
using Infrastructure.Cache;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Services;

public class ResourceService(
    IResourceRepository repository,
    ICacheProvider cache,
    ILogger logger) : IResourceService
{
    public async Task<Result<IEnumerable<ValueResult>>> GetTransportTypes()
    {
        try
        {
            var list = cache.Get<IEnumerable<ValueResult>>(CacheKeys.Transport.Types);

            if (list is not null)
            {
                return Result<IEnumerable<ValueResult>>.Success(list);
            }
            
            var transportTypes = await repository.GetTransportTypes();
                
            list = transportTypes.Select(x => new ValueResult
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
                
            cache.Set(CacheKeys.Transport.Types, list);

            return Result<IEnumerable<ValueResult>>.Success(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<IEnumerable<ValueResult>>.Failure(new Error(ErrorCodes.Resource.GetTransportTypes, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<IEnumerable<ValueResult>>> GetTransportMakesByTransportTypeId(Guid id)
    {
        try
        {
            var key = $"{CacheKeys.Transport.Makes}-{id}";
            
            var list = cache.Get<IEnumerable<ValueResult>>(key);

            if (list is not null)
            {
                return Result<IEnumerable<ValueResult>>.Success(list);
            }
            
            var transportType = await repository.GetTransportTypeById(id);

            list = transportType.TransportTypeMakes.Select(x =>
                new ValueResult()
                {
                    Id = x.TransportMake.Id,
                    Name = x.TransportMake.Name
                }).ToList();
            
            cache.Set(key, list);

            return Result<IEnumerable<ValueResult>>.Success(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<IEnumerable<ValueResult>>.Failure(
                new Error(ErrorCodes.Resource.GetTransportMakesByTransportTypeId, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<IEnumerable<ValueResult>>> GetTransportBodyTypesByTransportTypeId(Guid id)
    {
        try
        {
            var key = $"{CacheKeys.Transport.BodyTypes}-{id}";

            var list = cache.Get<IEnumerable<ValueResult>>(key);

            if (list is not null)
            {
                return Result<IEnumerable<ValueResult>>.Success(list);
            }
            
            var transportType = await repository.GetTransportTypeById(id);

            list = transportType.TransportTypeBodyTypes.Select(x =>
                new ValueResult()
                {
                    Id = x.TransportBodyType.Id,
                    Name = x.TransportBodyType.Name
                }).ToList();
            
            cache.Set(key, list);
            
            return Result<IEnumerable<ValueResult>>.Success(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<IEnumerable<ValueResult>>.Failure(
                new Error(ErrorCodes.Resource.GetTransportBodyTypesByTransportTypeId, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<IEnumerable<ValueResult>>> GetTransportModelsByTransportMakeId(Guid id)
    {
        try
        {
            var key = $"{CacheKeys.Transport.Models}-{id}";

            var list = cache.Get<IEnumerable<ValueResult>>(key);

            if (list is not null)
            {
                return Result<IEnumerable<ValueResult>>.Success(list);
            }
            
            var transportMake = await repository.GetTransportMakeById(id);

            list = transportMake.TransportMakeModels.Select(x =>
                new ValueResult()
                {
                    Id = x.TransportModel.Id,
                    Name = x.TransportModel.Name
                }).ToList();
            
            cache.Set(key, list);
            
            return Result<IEnumerable<ValueResult>>.Success(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<IEnumerable<ValueResult>>.Failure(
                new Error(ErrorCodes.Resource.GetTransportModelsByTransportMakeId, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<IEnumerable<ModeratorOverviewStatus>>> GetModeratorOverviewStatuses()
    {
        try
        {
            var list = cache.Get<IEnumerable<ModeratorOverviewStatus>>(CacheKeys.Moderator.OverviewStatuses);

            if (list is not null)
            {
                return Result<IEnumerable<ModeratorOverviewStatus>>.Success(list);
            }

            var overviewStatuses = await repository.GetModeratorOverviewStatuses();

            list = overviewStatuses.ToList();

            cache.Set(CacheKeys.Moderator.OverviewStatuses, list);
            
            return Result<IEnumerable<ModeratorOverviewStatus>>.Success(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<IEnumerable<ModeratorOverviewStatus>>.Failure(new Error(ErrorCodes.Resource.GetModeratorOverviewStatuses,
                ErrorMessages.ServiceError));
        }
    }
}