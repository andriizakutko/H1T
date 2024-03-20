using Domain;
using Domain.Transport;

namespace Persistence.Interfaces;

public interface IModeratorRepository
{
    Task<ModeratorOverviewStatus> GetModeratorOverviewStatusByName(string status);
    Task<IEnumerable<ModeratorOverviewStatus>> GetModeratorOverviewStatuses();
    Task UpdateModeratorOverviewStatus(Guid advertisementId, Guid statusId);
    Task<IEnumerable<TransportAdvertisement>> GetTransportAdvertisementsByStatusId(Guid statusId);
    Task UpdateTransportAdvertisementVerificationStatus(Guid id, bool isVerified);
}