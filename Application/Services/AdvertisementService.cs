using Application.Interfaces;
using Common.Requests;
using Common.Results;
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
    public async Task<Result<IEnumerable<TransportAdvertisement>>> GetTransportAdvertisements()
    {
        try
        {
            return Result<IEnumerable<TransportAdvertisement>>.Success(await advertisementRepository.GetTransportAdvertisements());
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<TransportAdvertisement>>.Failure(
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

            transportAdvertisementModelToCreate.Type = transportType;
            transportAdvertisementModelToCreate.Make = transportMake;
            transportAdvertisementModelToCreate.Model = transportModel;

            var transportAdvertisementImages = images
                .Select(x => new TransportAdvertisementImage
                    { TransportAdvertisement = transportAdvertisementModelToCreate, Image = x }).ToList();

            await advertisementRepository.AddTransportAdvertisementImages(transportAdvertisementImages);

            transportAdvertisementModelToCreate.Images = transportAdvertisementImages;

            await advertisementRepository.CreateTransportAdvertisement(transportAdvertisementModelToCreate);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("AdvertisementService.CreateTransportAdvertisement", ex.Message));
        }
    }
}