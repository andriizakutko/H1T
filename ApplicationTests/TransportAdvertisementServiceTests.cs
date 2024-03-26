using Application.Interfaces;
using Application.Services;
using Common.Requests;
using Common.Results;
using Common.ServiceResults;
using Domain;
using Domain.Enums;
using Domain.Transport;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;

namespace ApplicationTests;

public class TransportAdvertisementServiceTests
{
    private Mock<ITransportAdvertisementRepository> _mockTransportAdvertisementRepository;
    private Mock<IResourceRepository> _mockResourceRepository;
    private Mock<IImageService> _mockImageService;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IModeratorService> _mockModeratorService;
    private Mock<ILogger<TransportAdvertisementService>> _mockLogger;
    private TransportAdvertisementService _transportAdvertisementService;
    private List<TransportAdvertisement> _expectedAdvertisements;
    private CreateTransportAdvertisementRequest _request;
    private User _expectedCreator;
    private ModeratorOverviewStatus _expectedOverviewStatus;
    private Result<ModeratorOverviewStatus> _expectedResult;
    private TransportAdvertisement _transportAdvertisementModel;
    private TransportType _expectedTransportType;
    private TransportMake _expectedTransportMake;
    private TransportModel _expectedTransportModel;
    private TransportBodyType _expectedTransportBodyType;

    [SetUp]
    public void SetUp()
    {
        _mockTransportAdvertisementRepository = new Mock<ITransportAdvertisementRepository>();
        _mockResourceRepository = new Mock<IResourceRepository>();
        _mockImageService = new Mock<IImageService>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockModeratorService = new Mock<IModeratorService>();
        _mockLogger = new Mock<ILogger<TransportAdvertisementService>>();

        _transportAdvertisementService = new TransportAdvertisementService(
            _mockTransportAdvertisementRepository.Object,
            _mockResourceRepository.Object,
            _mockImageService.Object,
            _mockModeratorService.Object,
            _mockUserRepository.Object,
            _mockLogger.Object);
        
        InitEntities();
    }

    private void InitEntities()
    {
        _expectedAdvertisements = new List<TransportAdvertisement>
        {
            new()
            {
                Id = new Guid(),
                Title = "Car",
                SubTitle = "Luxury",
                Description = "Luxury car for sale",
                Price = 50000,
                ModeratorOverviewStatus = new ModeratorOverviewStatus() { Name = "Approved" },
                Type = new TransportType { Name = "Car" },
                Make = new TransportMake { Name = "BMW" },
                Model = new TransportModel { Name = "X5" },
                BodyType = new TransportBodyType { Name = "SUV" },
                EngineCapacity = 3000,
                SerialNumber = "123456",
                FuelConsumption = 10,
                Country = "USA",
                City = "New York",
                Address = "123 Main St",
                Mileage = 20000,
                ManufactureCountry = "Germany",
                ManufactureDate = new DateTime(2022, 1, 1),
                IsElectric = false,
                IsNew = false,
                IsUsed = true,
                IsVerified = true,
                Images = new List<TransportAdvertisementImage>
                {
                    new() { Image = new Image { Url = "image1.jpg" } }, 
                    new() { Image = new Image { Url = "image2.jpg" } }
                },
                Creator = new User { Email = "test@example.com", FirstName = "John", LastName = "Doe" }
            }
        };
        
        _request = new CreateTransportAdvertisementRequest
        {
            Title = "Luxury Sedan for Sale",
            SubTitle = "Low Mileage, Well-Maintained",
            Description = "This luxury sedan is in excellent condition with low mileage. Regularly serviced and well-maintained.",
            Price = 35000.00,
            TypeId = Guid.NewGuid(),
            MakeId = Guid.NewGuid(),
            ModelId = Guid.NewGuid(),
            BodyTypeId = Guid.NewGuid(),
            CreatorEmail = "test@example.com",
            EngineCapacity = 2.0,
            SerialNumber = "DEF456GHI789",
            FuelConsumption = 8.5,
            Country = "USA",
            City = "Los Angeles",
            Address = "456 Oak Street",
            Mileage = 15000,
            ManufactureCountry = "Germany",
            ManufactureDate = new DateTime(2022, 8, 10),
            IsElectric = false,
            IsNew = false,
            IsUsed = true,
            ImageUrls = new string[]
            {
                "https://example.com/image1.jpg",
                "https://example.com/image2.jpg",
                "https://example.com/image3.jpg"
            }
        };
        
        _expectedOverviewStatus = new ModeratorOverviewStatus()
        {
            Name = ModeratorOverviewStatuses.Waiting.ToString()
        };

        _expectedResult = Result<ModeratorOverviewStatus>.Success(_expectedOverviewStatus);
        
        _transportAdvertisementModel = new TransportAdvertisement
        {
            Title = _request.Title,
            SubTitle = _request.SubTitle,
            Description = _request.Description,
            Price = _request.Price,
            ModeratorOverviewStatus = _expectedResult.Value,
            EngineCapacity = _request.EngineCapacity,
            SerialNumber = _request.SerialNumber,
            FuelConsumption = _request.FuelConsumption,
            Country = _request.Country,
            City = _request.City,
            Address = _request.Address,
            Mileage = _request.Mileage,
            ManufactureCountry = _request.ManufactureCountry,
            ManufactureDate = _request.ManufactureDate,
            IsElectric = _request.IsElectric,
            IsNew = _request.IsNew,
            IsUsed = _request.IsUsed,
            IsVerified = false
        };

        _expectedTransportType = new TransportType()
        {
            Name = "Some transport type"
        };
        
        _expectedTransportMake = new TransportMake()
        {
            Name = "Some transport make"
        };
        
        _expectedTransportModel = new TransportModel()
        {
            Name = "Some transport model"
        };
        
        _expectedTransportBodyType = new TransportBodyType()
        {
            Name = "Some transport body type"
        };

        _expectedCreator = new User()
        {
            FirstName = "First name",
            LastName = "Last name",
            Email = "test@gmail.com",
            Country = "Some country",
            City = "Some city",
            IsActive = true
        };
    }

    [Test]
    public async Task GetTransportAdvertisements_ReturnsSuccess()
    {
        //Arrange
        _mockTransportAdvertisementRepository.Setup(x => x.GetTransportAdvertisements())
            .ReturnsAsync(_expectedAdvertisements);

        //Act
        var result = await _transportAdvertisementService.GetTransportAdvertisements();

        //Asserts
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Count(), Is.EqualTo(_expectedAdvertisements.Count));
        });
        
        var advertisementResult = result.Value.First();
        Assert.That(advertisementResult.Id, Is.EqualTo(_expectedAdvertisements.First().Id));
        _mockTransportAdvertisementRepository.Verify(repo => repo.GetTransportAdvertisements(), Times.Once);
    }
    
    [Test]
    public async Task GetTransportAdvertisements_ReturnsFailed_ServiceError()
    {
        //Arrange
        _mockTransportAdvertisementRepository.Setup(x => x.GetTransportAdvertisements())
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _transportAdvertisementService.GetTransportAdvertisements();

        //Asserts
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.TransportAdvertisement.GetTransportAdvertisements,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task CreateTransportAdvertisement_ReturnsSuccess_AdvertisementWasCreatedSuccessfully()
    {
        //Arrange
        _mockImageService.Setup(x => x.AddImages(It.IsAny<List<Image>>()))
            .ReturnsAsync(Result.Success());

        _mockModeratorService
            .Setup(x => x.GetModeratorOverviewStatusByName(ModeratorOverviewStatuses.Waiting.ToString()))
            .ReturnsAsync(_expectedResult);
        
        _mockResourceRepository.Setup(x => x.GetTransportTypeById(_request.TypeId))
            .ReturnsAsync(_expectedTransportType);
        
        _mockResourceRepository.Setup(x => x.GetTransportMakeById(_request.MakeId))
            .ReturnsAsync(_expectedTransportMake);

        _mockResourceRepository.Setup(x => x.GetTransportModelById(_request.ModelId))
            .ReturnsAsync(_expectedTransportModel);

        _mockResourceRepository.Setup(x => x.GetTransportBodyTypeById(_request.BodyTypeId))
            .ReturnsAsync(_expectedTransportBodyType);

        _mockUserRepository.Setup(x => x.GetByEmail(_request.CreatorEmail))
            .ReturnsAsync(_expectedCreator);

        _mockTransportAdvertisementRepository
            .Setup(x => x.CreateTransportAdvertisement(_transportAdvertisementModel))
            .ReturnsAsync(_transportAdvertisementModel);

        _mockTransportAdvertisementRepository.Setup(x =>
                x.AddTransportAdvertisementImages(It.IsAny<List<TransportAdvertisementImage>>()))
            .ReturnsAsync(true);
        
        //Act
        var result = await _transportAdvertisementService.CreateTransportAdvertisement(_request);

        //Assert
        Assert.That(result.IsSuccess);
    }
    
    [Test]
    public async Task CreateTransportAdvertisement_ReturnsFailed_AddImagesReturnsFailedResult()
    {
        //Arrange
        _mockImageService.Setup(x => x.AddImages(It.IsAny<List<Image>>()))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.Image.AddImages,
                ErrorMessages.Image.ImagesWereNotAdded)));
        
        //Act
        var result = await _transportAdvertisementService.CreateTransportAdvertisement(_request);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Image.AddImages,
                Message: ErrorMessages.Image.ImagesWereNotAdded
            });
    }

    [Test] public async Task CreateTransportAdvertisement_ReturnsFailed_GetModeratorOverviewStatusByNameReturnedFailedResult()
    {
        //Arrange
        _mockImageService.Setup(x => x.AddImages(It.IsAny<List<Image>>()))
            .ReturnsAsync(Result.Success());

        _mockModeratorService
            .Setup(x => x.GetModeratorOverviewStatusByName(ModeratorOverviewStatuses.Waiting.ToString()))
            .ReturnsAsync(Result<ModeratorOverviewStatus>.Failure(new Error(ErrorCodes.Moderator.GetModeratorOverviewStatusByName,
                ErrorMessages.ServiceError)));
        
        //Act
        var result = await _transportAdvertisementService.CreateTransportAdvertisement(_request);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Moderator.GetModeratorOverviewStatusByName,
                Message: ErrorMessages.ServiceError
            });
    }
    
    [Test]
    public async Task CreateTransportAdvertisement_ReturnsFailed_UserWasNotFoundResult()
    {
        //Arrange
        _mockImageService.Setup(x => x.AddImages(It.IsAny<List<Image>>()))
            .ReturnsAsync(Result.Success());

        _mockModeratorService
            .Setup(x => x.GetModeratorOverviewStatusByName(ModeratorOverviewStatuses.Waiting.ToString()))
            .ReturnsAsync(_expectedResult);
        
        _mockResourceRepository.Setup(x => x.GetTransportTypeById(_request.TypeId))
            .ReturnsAsync(_expectedTransportType);
        
        _mockResourceRepository.Setup(x => x.GetTransportMakeById(_request.MakeId))
            .ReturnsAsync(_expectedTransportMake);

        _mockResourceRepository.Setup(x => x.GetTransportModelById(_request.ModelId))
            .ReturnsAsync(_expectedTransportModel);

        _mockResourceRepository.Setup(x => x.GetTransportBodyTypeById(_request.BodyTypeId))
            .ReturnsAsync(_expectedTransportBodyType);

        _mockUserRepository.Setup(x => x.GetByEmail(_request.CreatorEmail))
            .ReturnsAsync((User)null!);
        
        //Act
        var result = await _transportAdvertisementService.CreateTransportAdvertisement(_request);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.TransportAdvertisement.CreateTransportAdvertisement,
                Message: ErrorMessages.TransportAdvertisement.CreatorNotFound
            });
    }
    
    [Test]
    public async Task CreateTransportAdvertisement_ReturnsFailed_AdvertisementWasNotCreatedSuccessfully()
    {
        //Arrange
        _mockImageService.Setup(x => x.AddImages(It.IsAny<List<Image>>()))
            .ReturnsAsync(Result.Success());

        _mockModeratorService
            .Setup(x => x.GetModeratorOverviewStatusByName(ModeratorOverviewStatuses.Waiting.ToString()))
            .ReturnsAsync(_expectedResult);
        
        _mockResourceRepository.Setup(x => x.GetTransportTypeById(_request.TypeId))
            .ReturnsAsync(_expectedTransportType);
        
        _mockResourceRepository.Setup(x => x.GetTransportMakeById(_request.MakeId))
            .ReturnsAsync(_expectedTransportMake);

        _mockResourceRepository.Setup(x => x.GetTransportModelById(_request.ModelId))
            .ReturnsAsync(_expectedTransportModel);

        _mockResourceRepository.Setup(x => x.GetTransportBodyTypeById(_request.BodyTypeId))
            .ReturnsAsync(_expectedTransportBodyType);

        _mockUserRepository.Setup(x => x.GetByEmail(_request.CreatorEmail))
            .ReturnsAsync(_expectedCreator);

        _mockTransportAdvertisementRepository
            .Setup(x => x.CreateTransportAdvertisement(_transportAdvertisementModel))
            .ReturnsAsync(_transportAdvertisementModel);

        _mockTransportAdvertisementRepository.Setup(x =>
                x.AddTransportAdvertisementImages(It.IsAny<List<TransportAdvertisementImage>>()))
            .ReturnsAsync(false);
        
        //Act
        var result = await _transportAdvertisementService.CreateTransportAdvertisement(_request);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.TransportAdvertisement.CreateTransportAdvertisement,
                Message: ErrorMessages.TransportAdvertisement.CreateTransportAdvertisementFailed
            });
    }
    
    [Test]
    public async Task CreateTransportAdvertisement_ReturnsFailed_ServiceError()
    {
        //Arrange
        _mockImageService.Setup(x => x.AddImages(It.IsAny<List<Image>>()))
            .ThrowsAsync(new Exception("Some exception"));
        
        //Act
        var result = await _transportAdvertisementService.CreateTransportAdvertisement(_request);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.TransportAdvertisement.CreateTransportAdvertisement,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task GetTransportAdvertisementById_ReturnsSuccess()
    {
        //Arrange
        var id = Guid.NewGuid();

        _mockTransportAdvertisementRepository.Setup(x => x.GetTransportAdvertisementById(id))
            .ReturnsAsync(_transportAdvertisementModel);

        //Act
        var result = await _transportAdvertisementService.GetTransportAdvertisementById(id);

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value is not null);
    }
    
    [Test]
    public async Task GetTransportAdvertisementById_ReturnsFailed_TransportAdvertisementWasNotFound()
    {
        //Arrange
        var id = Guid.NewGuid();

        _mockTransportAdvertisementRepository.Setup(x => x.GetTransportAdvertisementById(id))
            .ReturnsAsync((TransportAdvertisement)null!);

        //Act
        var result = await _transportAdvertisementService.GetTransportAdvertisementById(id);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.TransportAdvertisement.GetTransportAdvertisementById,
                Message: ErrorMessages.TransportAdvertisement.GetTransportAdvertisementByIdFailed
            });
    }
    
    [Test]
    public async Task GetTransportAdvertisementById_ReturnsFailed_ServiceError()
    {
        //Arrange
        var id = Guid.NewGuid();

        _mockTransportAdvertisementRepository.Setup(x => x.GetTransportAdvertisementById(id))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _transportAdvertisementService.GetTransportAdvertisementById(id);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.TransportAdvertisement.GetTransportAdvertisementById,
                Message: ErrorMessages.ServiceError
            });
    }
}