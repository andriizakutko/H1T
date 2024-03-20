using Domain;
using Domain.Transport;

namespace Persistence.Interfaces;

public interface IResourceRepository
{
    Task<IEnumerable<TransportType>> GetTransportTypes();
    Task<TransportType> GetTransportTypeById(Guid id);
    Task<TransportMake> GetTransportMakeById(Guid id);
    Task<TransportModel> GetTransportModelById(Guid id);
    Task<TransportBodyType> GetTransportBodyTypeById(Guid id);
    Task<IEnumerable<ModeratorOverviewStatus>> GetModeratorOverviewStatuses();
}