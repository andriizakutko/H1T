using Common.Requests;
using Common.Results;
using Common.ServiceResults;
using Domain;

namespace Application.Interfaces;

public interface IModeratorService
{
    Task<Result<ModeratorOverviewStatus>> GetModeratorOverviewStatusByName(string status);
    Task<Result<IEnumerable<ModeratorOverviewStatus>>> GetModeratorOverviewStatuses();
    Task<Result> UpdateModeratorOverviewStatus(UpdateAdvertisementStatusRequest request);
    Task<Result<IEnumerable<TransportAdvertisementResult>>> GetTransportAdvertisementByStatusId(Guid statusId);
}