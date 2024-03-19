using Domain.Transport;

namespace Persistence.Interfaces;

public interface ITransportAdvertisementRepository
{
    Task<IEnumerable<TransportAdvertisement>> GetTransportAdvertisements();
    Task<TransportAdvertisement> CreateTransportAdvertisement(TransportAdvertisement model);
    Task AddTransportAdvertisementImages(IEnumerable<TransportAdvertisementImage> advertisementImages);
    Task<TransportAdvertisement> GetTransportAdvertisementById(Guid id);
    Task UpdateTransportAdvertisementStatus(Guid id, string status);
}