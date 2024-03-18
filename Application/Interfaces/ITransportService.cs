using Common.Results;
using Common.ServiceResults;

namespace Application.Interfaces;

public interface ITransportService
{
    Task<Result<IEnumerable<ValueResult>>> GetTransportTypes();
    Task<Result<IEnumerable<ValueResult>>> GetTransportMakesByTransportTypeId(Guid id);
    Task<Result<IEnumerable<ValueResult>>> GetTransportBodyTypesByTransportTypeId(Guid id);
    Task<Result<IEnumerable<ValueResult>>> GetTransportModelsByTransportMakeId(Guid id);
}