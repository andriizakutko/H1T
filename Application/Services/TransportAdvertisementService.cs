using Application.Interfaces;
using Common.Requests;
using Common.Results;
using Common.ServiceResults;
using Domain;
using Domain.Enums;
using Domain.Transport;
using Persistence.Interfaces;

namespace Application.Services;

public class TransportAdvertisementService(
    ITransportAdvertisementRepository transportAdvertisementRepository,
    IResourceRepository resourceRepository,
    IImageService imageService,
    IModeratorService moderatorService,
    IUserRepository userRepository) : ITransportAdvertisementService
{
    public async Task<Result<IEnumerable<TransportAdvertisementResult>>> GetTransportAdvertisements()
    {
        try
        {
            var list = await transportAdvertisementRepository.GetTransportAdvertisements();
            var advertisements = list.Select(x => new TransportAdvertisementResult()
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
                Images = x.Images.Select(i => i.Image.Url).ToArray(),
                CreatorEmail = x.Creator.Email,
                CreatorFirstName = x.Creator.FirstName,
                CreatorLastName = x.Creator.LastName
            });
            
            return Result<IEnumerable<TransportAdvertisementResult>>.Success(advertisements);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<TransportAdvertisementResult>>.Failure(
                new Error("AdvertisementService.GetTransportAdvertisements", ex.Message));
        }
    }

    public async Task<Result> CreateTransportAdvertisement(CreateTransportAdvertisementRequest request)
    {
        try
        {
            var images = request.ImageUrls.Select(url => new Image { Url = url }).ToList();

            var addImagesResult = await imageService.AddImages(images);
            
            if(addImagesResult.IsFailure) return Result.Failure(addImagesResult.Error);

            var moderatorOverviewStatusResult =
                await moderatorService.GetModeratorOverviewStatusByName(ModeratorOverviewStatuses.Waiting.ToString());

            if (moderatorOverviewStatusResult.IsFailure) return Result.Failure(moderatorOverviewStatusResult.Error);

            var transportAdvertisementModelToCreate = new TransportAdvertisement
            {
                Title = request.Title,
                SubTitle = request.SubTitle,
                Description = request.Description,
                Price = request.Price,
                ModeratorOverviewStatus = moderatorOverviewStatusResult.Value,
                EngineCapacity = request.EngineCapacity,
                SerialNumber = request.SerialNumber,
                FuelConsumption = request.FuelConsumption,
                Country = request.Country,
                City = request.City,
                Address = request.Address,
                Mileage = request.Mileage,
                ManufactureCountry = request.ManufactureCountry,
                ManufactureDate = request.ManufactureDate,
                IsElectric = request.IsElectric,
                IsNew = request.IsNew,
                IsUsed = request.IsUsed,
                IsVerified = false
            };

            var transportType = await resourceRepository.GetTransportTypeById(request.TypeId);
            var transportMake = await resourceRepository.GetTransportMakeById(request.MakeId);
            var transportModel = await resourceRepository.GetTransportModelById(request.ModelId);
            var transportBodyType = await resourceRepository.GetTransportBodyTypeById(request.BodyTypeId);
            
            var creator = await userRepository.GetByEmail(request.CreatorEmail);

            if (creator is null)
                return Result.Failure(new Error("TransportAdvertisementService.CreateTransportAdvertisement",
                    "Creator was not found"));

            transportAdvertisementModelToCreate.Type = transportType;
            transportAdvertisementModelToCreate.Make = transportMake;
            transportAdvertisementModelToCreate.Model = transportModel;
            transportAdvertisementModelToCreate.BodyType = transportBodyType;
            transportAdvertisementModelToCreate.Creator = creator;
            
            var createdAdModel = await transportAdvertisementRepository.CreateTransportAdvertisement(transportAdvertisementModelToCreate);

            var transportAdvertisementImages = images
                .Select(x => new TransportAdvertisementImage
                    { TransportAdvertisement = createdAdModel, Image = x }).ToList();

            await transportAdvertisementRepository.AddTransportAdvertisementImages(transportAdvertisementImages);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("AdvertisementService.CreateTransportAdvertisement", ex.Message));
        }
    }

    public async Task<Result<TransportAdvertisement>> GetTransportAdvertisementById(Guid id)
    {
        try
        {
            return Result<TransportAdvertisement>.Success(
                await transportAdvertisementRepository.GetTransportAdvertisementById(id));
        }
        catch (Exception ex)
        {
            return Result<TransportAdvertisement>.Failure(new Error("AdvertisementService.GetTransportAdvertisementById", ex.Message));
        }
    }
}