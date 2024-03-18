using Application.Interfaces;
using Common.Results;
using Common.ServiceResults;
using Persistence.Interfaces;

namespace Application.Services;

public class TransportService(ITransportRepository repository) : ITransportService
{
    public async Task<Result<IEnumerable<ValueResult>>> GetTransportTypes()
    {
        try
        {
            var list = await repository.GetTransportTypes();
            return Result<IEnumerable<ValueResult>>.Success(list.Select(x => new ValueResult()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList());
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ValueResult>>.Failure(new Error("TransportService.GetTransportTypes", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<ValueResult>>> GetTransportMakesByTransportTypeId(Guid id)
    {
        try
        {
            var transportType = await repository.GetTransportTypeById(id);

            return Result<IEnumerable<ValueResult>>.Success(transportType.TransportTypeMakes.Select(x =>
                new ValueResult()
                {
                    Id = x.TransportMake.Id,
                    Name = x.TransportMake.Name
                }));
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ValueResult>>.Failure(
                new Error("TransportService.GetTransportMakesByTransportTypeId", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<ValueResult>>> GetTransportBodyTypesByTransportTypeId(Guid id)
    {
        try
        {
            var transportType = await repository.GetTransportTypeById(id);
            
            return Result<IEnumerable<ValueResult>>.Success(transportType.TransportTypeBodyTypes.Select(x =>
                new ValueResult()
                {
                    Id = x.TransportBodyType.Id,
                    Name = x.TransportBodyType.Name
                }));
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ValueResult>>.Failure(
                new Error("TransportService.GetTransportBodyTypesByTransportTypeId", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<ValueResult>>> GetTransportModelsByTransportMakeId(Guid id)
    {
        try
        {
            var transportMake = await repository.GetTransportMakeById(id);
            
            return Result<IEnumerable<ValueResult>>.Success(transportMake.TransportMakeModels.Select(x =>
                new ValueResult()
                {
                    Id = x.TransportModel.Id,
                    Name = x.TransportModel.Name
                }));
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ValueResult>>.Failure(
                new Error("TransportService.GetTransportModelsByTransportMakeId", ex.Message));
        }
    }
}