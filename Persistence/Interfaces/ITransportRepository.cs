using Domain.Transport;

namespace Persistence.Interfaces;

public interface ITransportRepository
{
    Task<IEnumerable<TransportType>> GetTransportTypes();
    Task<TransportType> GetTransportTypeById(Guid id);
    Task<TransportMake> GetTransportMakeById(Guid id);
    Task<TransportModel> GetTransportModelById(Guid id);
}