using Application.Interfaces;
using Common.Requests;
using Common.Results;
using Common.ServiceResults;
using Domain;
using Domain.Enums;
using Domain.Transport;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Services;

public class TransportAdvertisementService : ITransportAdvertisementService
{
    private readonly ITransportAdvertisementRepository _transportAdvertisementRepository;
    private readonly IResourceRepository _resourceRepository;
    private readonly IImageService _imageService;
    private readonly IModeratorService _moderatorService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<TransportAdvertisementService> _logger;

    public TransportAdvertisementService(
        ITransportAdvertisementRepository transportAdvertisementRepository,
        IResourceRepository resourceRepository,
        IImageService imageService,
        IModeratorService moderatorService,
        IUserRepository userRepository,
        ILogger<TransportAdvertisementService> logger)
    {
        _transportAdvertisementRepository = transportAdvertisementRepository;
        _resourceRepository = resourceRepository;
        _imageService = imageService;
        _moderatorService = moderatorService;
        _userRepository = userRepository;
        _logger = logger;
    }
    
    public async Task<Result<IEnumerable<TransportAdvertisementResult>>> GetTransportAdvertisements()
    {
        try
        {
            var list = await _transportAdvertisementRepository.GetTransportAdvertisements();
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
            _logger.LogError(ex.Message);
            return Result<IEnumerable<TransportAdvertisementResult>>.Failure(
                new Error(ErrorCodes.TransportAdvertisement.GetTransportAdvertisements, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result> CreateTransportAdvertisement(CreateTransportAdvertisementRequest request)
    {
        try
        {
            var images = request.ImageUrls.Select(url => new Image { Url = url }).ToList();

            var addImagesResult = await _imageService.AddImages(images);
            
            if(addImagesResult.IsFailure) return Result.Failure(addImagesResult.Error);

            var moderatorOverviewStatusResult =
                await _moderatorService.GetModeratorOverviewStatusByName(ModeratorOverviewStatuses.Waiting.ToString());

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

            var transportType = await _resourceRepository.GetTransportTypeById(request.TypeId);
            var transportMake = await _resourceRepository.GetTransportMakeById(request.MakeId);
            var transportModel = await _resourceRepository.GetTransportModelById(request.ModelId);
            var transportBodyType = await _resourceRepository.GetTransportBodyTypeById(request.BodyTypeId);
            
            var creator = await _userRepository.GetByEmail(request.CreatorEmail);

            if (creator is null)
                return Result.Failure(new Error(ErrorCodes.TransportAdvertisement.CreateTransportAdvertisement,
                    ErrorMessages.TransportAdvertisement.CreatorNotFound));

            transportAdvertisementModelToCreate.Type = transportType;
            transportAdvertisementModelToCreate.Make = transportMake;
            transportAdvertisementModelToCreate.Model = transportModel;
            transportAdvertisementModelToCreate.BodyType = transportBodyType;
            transportAdvertisementModelToCreate.Creator = creator;
            
            var createdAdModel = await _transportAdvertisementRepository.CreateTransportAdvertisement(transportAdvertisementModelToCreate);

            var transportAdvertisementImages = images
                .Select(x => new TransportAdvertisementImage
                    { TransportAdvertisement = createdAdModel, Image = x }).ToList();

            await _transportAdvertisementRepository.AddTransportAdvertisementImages(transportAdvertisementImages);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.TransportAdvertisement.CreateTransportAdvertisement, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<TransportAdvertisement>> GetTransportAdvertisementById(Guid id)
    {
        try
        {
            return Result<TransportAdvertisement>.Success(
                await _transportAdvertisementRepository.GetTransportAdvertisementById(id));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<TransportAdvertisement>
                .Failure(new Error(ErrorCodes.TransportAdvertisement.GetTransportAdvertisementById, ErrorMessages.ServiceError));
        }
    }
}