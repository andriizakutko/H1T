using Common.Requests;
using Common.Results;
using Common.ServiceResults;

namespace Application.Interfaces;

public interface ITransportAdvertisementService
{
    Task<Result<IEnumerable<TransportAdvertisementResult>>> GetTransportAdvertisements();
    Task<Result> CreateTransportAdvertisement(CreateTransportAdvertisementRequest request);
}