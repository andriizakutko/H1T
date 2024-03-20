using Common.Results;
using Common.ServiceResults;
using Domain;

namespace Application.Interfaces;

public interface IResourceService
{
    Task<Result<IEnumerable<ValueResult>>> GetTransportTypes();
    Task<Result<IEnumerable<ValueResult>>> GetTransportMakesByTransportTypeId(Guid id);
    Task<Result<IEnumerable<ValueResult>>> GetTransportBodyTypesByTransportTypeId(Guid id);
    Task<Result<IEnumerable<ValueResult>>> GetTransportModelsByTransportMakeId(Guid id);
    Task<Result<IEnumerable<ModeratorOverviewStatus>>> GetModeratorOverviewStatuses();
}