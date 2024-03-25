using Domain;
using Domain.Enums;
using Domain.Transport;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class ModeratorRepository(ApplicationDbContext context) : IModeratorRepository
{
    public async Task<ModeratorOverviewStatus> GetModeratorOverviewStatusByName(string status)
    {
        return await context.ModeratorOverviewStatuses.FirstAsync(x => x.Name == status);
    }

    public async Task<bool> UpdateModeratorOverviewStatus(Guid advertisementId, Guid statusId)
    {
        var advertisement = await context.TransportAdvertisements.FindAsync(advertisementId);
        var status = await context.ModeratorOverviewStatuses.FindAsync(statusId);

        if (status!.Name == ModeratorOverviewStatuses.Accepted.ToString())
        {
            advertisement!.CreatedAt = DateTime.UtcNow;
        }

        advertisement!.ModeratorOverviewStatus = status;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<TransportAdvertisement>> GetTransportAdvertisementsByStatusId(Guid statusId)
    {
        return await context.TransportAdvertisements.Where(x => x.ModeratorOverviewStatus.Id == statusId).ToListAsync();
    }

    public async Task<bool> UpdateTransportAdvertisementVerificationStatus(Guid id, bool isVerified)
    {
        var transportAdvertisement = await context.TransportAdvertisements.FindAsync(id);

        transportAdvertisement!.IsVerified = isVerified;
        transportAdvertisement!.UpdatedAt = DateTime.UtcNow;

        return await context.SaveChangesAsync() > 0;
    }
}