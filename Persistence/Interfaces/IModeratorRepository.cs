using Domain;
using Domain.Transport;

namespace Persistence.Interfaces;

public interface IModeratorRepository
{
    Task<ModeratorOverviewStatus> GetModeratorOverviewStatusByName(string status);
    Task<bool> UpdateModeratorOverviewStatus(Guid advertisementId, Guid statusId);
    Task<IEnumerable<TransportAdvertisement>> GetTransportAdvertisementsByStatusId(Guid statusId);
    Task<bool> UpdateTransportAdvertisementVerificationStatus(Guid id, bool isVerified);
}