using Common.Requests;
using Common.Results;
using Domain.Transport;

namespace Application.Interfaces;

public interface IAdvertisementService
{
    Task<Result<IEnumerable<TransportAdvertisement>>> GetTransportAdvertisements();
    Task<Result> CreateTransportAdvertisement(CreateTransportAdvertisementRequest request);
}