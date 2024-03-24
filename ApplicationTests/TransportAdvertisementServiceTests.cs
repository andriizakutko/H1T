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
    private List<TransportAdvertisement> _advertisementReturned;
    private CreateTransportAdvertisementRequest _createTransportAdvertisementRequest;
    
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
        _advertisementReturned = new List<TransportAdvertisement>()
        {
            new()
            {
                Title = "Some title",
                SubTitle = "Some subtitle",
                Description = "Some description",
                Address = "Some address",
                Images = new List<TransportAdvertisementImage>(),
                Creator = new User()
                {
                    FirstName = "First name",
                    LastName = "Last name",
                    Email = "email@test.com"
                },
                Type = new TransportType()
                {
                    Name = "Test"
                },
                Make = new TransportMake()
                {
                    Name = "Test"
                },
                Model = new TransportModel()
                {
                    Name = "Test"
                },
                BodyType = new TransportBodyType()
                {
                    Name = "Test"
                },
                ModeratorOverviewStatus = new ModeratorOverviewStatus()
                {
                    Name = "Test"
                }
            }
        };

        _createTransportAdvertisementRequest = new CreateTransportAdvertisementRequest()
        {
            Title = "Test title",
            SubTitle = "Test sub title",
            Description = "Test description",
            Price = 14000,
            TypeId = Guid.Parse("a5e14667-3b6b-4bf4-a439-925d234593a1"),
            MakeId = Guid.Parse("e28a8881-4aa2-43d6-b8b5-0b35a9805f7f"),
            ModelId = Guid.Parse("cd5a6eab-81b2-4987-aa57-8f6820efec26"),
            BodyTypeId = Guid.Parse("7f3fb65e-df20-4715-8a5e-9981a5a9804e"),
            CreatorEmail = "email@test.com",
            EngineCapacity = 2.4,
            SerialNumber = "test000",
            FuelConsumption = 10.4,
            Country = "Some country",
            Address = "Some address",
            City = "Some city",
            Mileage = 100000,
            ManufactureCountry = "Some country",
            ManufactureDate = DateTime.UtcNow,
            IsElectric = false,
            IsNew = false,
            IsUsed = true,
            ImageUrls = new[] { "url1", "url2" }
        };
    }

    [Test]
    public async Task GetTransportAdvertisements_ReturnsSuccess()
    {
        //Arrange
        _mockTransportAdvertisementRepository.Setup(x => x.GetTransportAdvertisements())
            .ReturnsAsync(_advertisementReturned);

        //Act
        var result = await _transportAdvertisementService.GetTransportAdvertisements();

        //Asserts
        Assert.That(result.IsSuccess);
    }
}