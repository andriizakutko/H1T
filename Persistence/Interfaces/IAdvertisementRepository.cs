using Domain.Transport;

namespace Persistence.Interfaces;

public interface IAdvertisementRepository
{
    Task<IEnumerable<TransportAdvertisement>> GetTransportAdvertisements();
    Task CreateTransportAdvertisement(TransportAdvertisement model);
    Task AddTransportAdvertisementImages(ICollection<TransportAdvertisementImage> advertisementImages);
}