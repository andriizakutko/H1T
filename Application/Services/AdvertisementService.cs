using Application.Interfaces;
using Common.Requests;
using Common.Results;
using Common.ServiceResults;
using Domain;
using Domain.Enums;
using Domain.Transport;
using Persistence.Interfaces;

namespace Application.Services;

public class AdvertisementService(
    IAdvertisementRepository advertisementRepository,
    ITransportRepository transportRepository,
    IImageService imageService) : IAdvertisementService
{
    public async Task<Result<IEnumerable<TransportAdvertisementResult>>> GetTransportAdvertisements()
    {
        try
        {
            var list = await advertisementRepository.GetTransportAdvertisements();
            var advertisements = list.Select(x => new TransportAdvertisementResult()
            {
                Title = x.Title,
                SubTitle = x.SubTitle,
                Description = x.Description,
                Price = x.Price,
                ModeratorOverviewStatus = x.ModeratorOverviewStatus,
                Type = x.Type,
                Make = x.Make,
                Model = x.Model,
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

            var transportAdvertisementModelToCreate = new TransportAdvertisement
            {
                Title = request.Title,
                SubTitle = request.SubTitle,
                Description = request.Description,
                Price = request.Price,
                ModeratorOverviewStatus = ModeratorOverviewStatus.Waiting.ToString(),
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

            var transportType = await transportRepository.GetTransportTypeById(request.TypeId);
            var transportMake = await transportRepository.GetTransportMakeById(request.MakeId);
            var transportModel = await transportRepository.GetTransportModelById(request.ModelId);

            transportAdvertisementModelToCreate.Type = transportType.Name;
            transportAdvertisementModelToCreate.Make = transportMake.Name;
            transportAdvertisementModelToCreate.Model = transportModel.Name;
            
            var createdAdModel = await advertisementRepository.CreateTransportAdvertisement(transportAdvertisementModelToCreate);

            var transportAdvertisementImages = images
                .Select(x => new TransportAdvertisementImage
                    { TransportAdvertisement = createdAdModel, Image = x }).ToList();

            await advertisementRepository.AddTransportAdvertisementImages(transportAdvertisementImages);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("AdvertisementService.CreateTransportAdvertisement", ex.Message));
        }
    }
}