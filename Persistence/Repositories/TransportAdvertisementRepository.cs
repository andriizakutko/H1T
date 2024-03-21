using Domain.Enums;
using Domain.Transport;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class TransportAdvertisementRepository(ApplicationDbContext context) : ITransportAdvertisementRepository
{
    public async Task<IEnumerable<TransportAdvertisement>> GetTransportAdvertisements()
    {
        return await context.TransportAdvertisements.Where(x => x.ModeratorOverviewStatus.Name == ModeratorOverviewStatuses.Accepted.ToString()).ToListAsync();
    }

    public async Task<TransportAdvertisement> CreateTransportAdvertisement(TransportAdvertisement model)
    {
        var result = await context.TransportAdvertisements.AddAsync(model);
        await context.SaveChangesAsync();

        return result.Entity;
    }

    public async Task AddTransportAdvertisementImages(IEnumerable<TransportAdvertisementImage> advertisementImages)
    {
        await context.TransportAdvertisementImages.AddRangeAsync(advertisementImages);
        await context.SaveChangesAsync();
    }

    public async Task<TransportAdvertisement> GetTransportAdvertisementById(Guid id)
    {
        return await context.TransportAdvertisements.FindAsync(id);
    }
}