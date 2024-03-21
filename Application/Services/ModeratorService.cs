using Application.Interfaces;
using Common.Requests;
using Common.Results;
using Common.ServiceResults;
using Domain;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Services;

public class ModeratorService(
    IModeratorRepository moderatorRepository,
    ILogger logger) : IModeratorService
{
    public async Task<Result<ModeratorOverviewStatus>> GetModeratorOverviewStatusByName(string status)
    {
        try
        {
            return await moderatorRepository.GetModeratorOverviewStatusByName(status);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<ModeratorOverviewStatus>.Failure(new Error(ErrorCodes.Moderator.GetModeratorOverviewStatusByName,
                ErrorMessages.ServiceError));
        }
    }

    public async Task<Result> UpdateModeratorOverviewStatus(UpdateAdvertisementStatusRequest request)
    {
        try
        {
            await moderatorRepository.UpdateModeratorOverviewStatus(request.AdvertisementId, request.StatusId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.Moderator.UpdateModeratorOverviewStatus,
                ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<IEnumerable<TransportAdvertisementResult>>> GetTransportAdvertisementByStatusId(Guid statusId)
    {
        try
        {
            var list = await moderatorRepository.GetTransportAdvertisementsByStatusId(statusId);
            
            var resultList = list.Select(x => new TransportAdvertisementResult()
            {
                Id = x.Id,
                Title = x.Title,
                SubTitle = x.SubTitle,
                Description = x.Description,
                Price = x.Price,
                ModeratorOverviewStatus = x.ModeratorOverviewStatus.Name,
                Type = x.Type.Name,
                Make = x.Make.Name,
                Model = x.Model.Name,
                BodyType = x.BodyType.Name,
                EngineCapacity = x.EngineCapacity,
                SerialNumber = x.SerialNumber,
                FuelConsumption = x.FuelConsumption,
                Country = x.Country,
                City = x.City,
                Address = x.Address,
                Mileage = x.Mileage,
                ManufactureCountry = x.ManufactureCountry,
                ManufactureDate = x.ManufactureDate,
                IsElectric = x.IsElectric,
                IsNew = x.IsNew,
                IsUsed = x.IsUsed,
                IsVerified = x.IsVerified,
                Images = x.Images.Select(i => i.Image.Url).ToArray()
            });

            return Result<IEnumerable<TransportAdvertisementResult>>.Success(resultList);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<IEnumerable<TransportAdvertisementResult>>.Failure(new Error(ErrorCodes.Moderator.GetTransportAdvertisementByStatusId,
                ErrorMessages.ServiceError));
        }
    }

    public async Task<Result> UpdateTransportAdvertisementVerificationStatus(UpdateTransportAdvertisementVerificationStatusRequest request)
    {
        try
        {
            await moderatorRepository.UpdateTransportAdvertisementVerificationStatus(request.TransportAdvertisementId,
                request.IsVerified);

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result<IEnumerable<TransportAdvertisementResult>>.Failure(new Error(ErrorCodes.Moderator.UpdateTransportAdvertisementVerificationStatus,
                ErrorMessages.ServiceError));
        }
    }
}