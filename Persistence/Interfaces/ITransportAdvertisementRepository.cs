using Domain.Transport;

namespace Persistence.Interfaces;

public interface ITransportAdvertisementRepository
{
    Task<IEnumerable<TransportAdvertisement>> GetTransportAdvertisements();
    Task<TransportAdvertisement> CreateTransportAdvertisement(TransportAdvertisement model);
    Task<bool> AddTransportAdvertisementImages(IEnumerable<TransportAdvertisementImage> advertisementImages);
    Task<TransportAdvertisement> GetTransportAdvertisementById(Guid id);
}