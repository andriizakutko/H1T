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
}