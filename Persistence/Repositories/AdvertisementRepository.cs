using Domain.Transport;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class AdvertisementRepository(ApplicationDbContext context) : IAdvertisementRepository
{
    public async Task<IEnumerable<TransportAdvertisement>> GetTransportAdvertisements()
    {
        return await context.TransportAdvertisements.ToListAsync();
    }

    public async Task<TransportAdvertisement> CreateTransportAdvertisement(TransportAdvertisement model)
    {
        var result = await context.TransportAdvertisements.AddAsync(model);
        await context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task AddTransportAdvertisementImages(ICollection<TransportAdvertisementImage> advertisementImages)
    {
        await context.TransportAdvertisementImages.AddRangeAsync(advertisementImages);
        await context.SaveChangesAsync();
    }
}