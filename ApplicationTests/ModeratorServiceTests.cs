using Application.Services;
using Common.Requests;
using Common.Results;
using Domain;
using Domain.Enums;
using Domain.Transport;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;

namespace ApplicationTests;

public class ModeratorServiceTests
{
    private Mock<IModeratorRepository> _mockModeratorRepository;
    private Mock<ILogger<ModeratorService>> _mockLogger;
    private ModeratorService _moderatorService;
    private string _status;
    private UpdateTransportAdvertisementVerificationStatusRequest
        _updateTransportAdvertisementVerificationStatusRequest;
    
    [SetUp]
    public void SetUp()
    {
        _mockModeratorRepository = new Mock<IModeratorRepository>();
        _mockLogger = new Mock<ILogger<ModeratorService>>();

        _moderatorService = new ModeratorService(_mockModeratorRepository.Object,
            _mockLogger.Object);

        InitEntities();
    }

    private void InitEntities()
    {
        _status = ModeratorOverviewStatuses.Waiting.ToString();
        
        _updateTransportAdvertisementVerificationStatusRequest = new UpdateTransportAdvertisementVerificationStatusRequest
        {
            TransportAdvertisementId = Guid.NewGuid(),
            IsVerified = true
        };
    }

    [Test]
    public async Task GetModeratorOverviewStatusByName_ReturnsSuccess()
    {
        //Arrange
        var expectedOverviewStatus = new ModeratorOverviewStatus()
        {
            Name = "Waiting"
        };

        _mockModeratorRepository.Setup(x => x.GetModeratorOverviewStatusByName(_status))
            .ReturnsAsync(expectedOverviewStatus);

        //Act
        var result = await _moderatorService.GetModeratorOverviewStatusByName(_status);

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value is not null);
    }
    
    [Test]
    public async Task GetModeratorOverviewStatusByName_ReturnsFailed_ServiceError()
    {
        //Arrange
        _mockModeratorRepository.Setup(x => x.GetModeratorOverviewStatusByName(_status))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _moderatorService.GetModeratorOverviewStatusByName(_status);

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
    public async Task UpdateModeratorOverviewStatus_ReturnsSuccess_StatusWasUpdated()
    {
        //Arrange
        var request = new UpdateAdvertisementStatusRequest()
        {
            AdvertisementId = Guid.NewGuid(),
            StatusId = Guid.NewGuid()
        };

        _mockModeratorRepository.Setup(x => x.UpdateModeratorOverviewStatus(request.AdvertisementId, request.StatusId))
            .ReturnsAsync(true);

        //Act
        var result = await _moderatorService.UpdateModeratorOverviewStatus(request);

        //Assert
        Assert.That(result.IsSuccess);
    }
    
    [Test]
    public async Task UpdateModeratorOverviewStatus_ReturnsFailed_StatusWasNotUpdated()
    {
        //Arrange
        var request = new UpdateAdvertisementStatusRequest()
        {
            AdvertisementId = Guid.NewGuid(),
            StatusId = Guid.NewGuid()
        };

        _mockModeratorRepository.Setup(x => x.UpdateModeratorOverviewStatus(request.AdvertisementId, request.StatusId))
            .ReturnsAsync(false);

        //Act
        var result = await _moderatorService.UpdateModeratorOverviewStatus(request);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Moderator.UpdateModeratorOverviewStatus,
                Message: ErrorMessages.Moderator.UpdateModeratorOverviewStatusFailed
            });
    }
    
    [Test]
    public async Task UpdateModeratorOverviewStatus_ReturnsFailed_ServiceError()
    {
        //Arrange
        var request = new UpdateAdvertisementStatusRequest()
        {
            AdvertisementId = Guid.NewGuid(),
            StatusId = Guid.NewGuid()
        };

        _mockModeratorRepository.Setup(x => x.UpdateModeratorOverviewStatus(request.AdvertisementId, request.StatusId))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _moderatorService.UpdateModeratorOverviewStatus(request);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Moderator.UpdateModeratorOverviewStatus,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task GetTransportAdvertisementByStatusId_ReturnsSuccess_ValueWasReturned()
    {
        //Arrange
        var statusId = Guid.NewGuid();
        
        var expectedAdvertisements = new List<TransportAdvertisement>
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

        _mockModeratorRepository.Setup(x => x.GetTransportAdvertisementsByStatusId(statusId))
            .ReturnsAsync(expectedAdvertisements);

        //Act
        var result = await _moderatorService.GetTransportAdvertisementByStatusId(statusId);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Count(), Is.EqualTo(expectedAdvertisements.Count));
        });
        
        var advertisementResult = result.Value.First();
        Assert.That(advertisementResult.Id, Is.EqualTo(expectedAdvertisements.First().Id));
        _mockModeratorRepository.Verify(repo => repo.GetTransportAdvertisementsByStatusId(statusId), Times.Once);
    }
    
    [Test]
    public async Task GetTransportAdvertisementByStatusId_ReturnsFailed_ServiceError()
    {
        //Arrange
        var statusId = Guid.NewGuid();

        _mockModeratorRepository.Setup(x => x.GetTransportAdvertisementsByStatusId(statusId))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _moderatorService.GetTransportAdvertisementByStatusId(statusId);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Moderator.GetTransportAdvertisementByStatusId,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task UpdateTransportAdvertisementVerificationStatus_ReturnsSuccess()
    {
        //Arrange
        _mockModeratorRepository.Setup(x =>
                x.UpdateTransportAdvertisementVerificationStatus(_updateTransportAdvertisementVerificationStatusRequest.TransportAdvertisementId, _updateTransportAdvertisementVerificationStatusRequest.IsVerified))
            .ReturnsAsync(true);

        //Act
        var result = await _moderatorService.UpdateTransportAdvertisementVerificationStatus(_updateTransportAdvertisementVerificationStatusRequest);

        //Assert
        Assert.That(result.IsSuccess);
    }
    
    [Test]
    public async Task UpdateTransportAdvertisementVerificationStatus_ReturnsFailed_VerificationStatusWasNotUpdated()
    {
        //Arrange
        _mockModeratorRepository.Setup(x =>
                x.UpdateTransportAdvertisementVerificationStatus(_updateTransportAdvertisementVerificationStatusRequest.TransportAdvertisementId, _updateTransportAdvertisementVerificationStatusRequest.IsVerified))
            .ReturnsAsync(false);

        //Act
        var result = await _moderatorService.UpdateTransportAdvertisementVerificationStatus(_updateTransportAdvertisementVerificationStatusRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Moderator.UpdateTransportAdvertisementVerificationStatus,
                Message: ErrorMessages.Moderator.UpdateTransportAdvertisementVerificationStatusFailed
            });
    }
    
    [Test]
    public async Task UpdateTransportAdvertisementVerificationStatus_ReturnsFailed_ServiceError()
    {
        //Arrange
        _mockModeratorRepository.Setup(x =>
                x.UpdateTransportAdvertisementVerificationStatus(_updateTransportAdvertisementVerificationStatusRequest.TransportAdvertisementId, _updateTransportAdvertisementVerificationStatusRequest.IsVerified))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _moderatorService.UpdateTransportAdvertisementVerificationStatus(_updateTransportAdvertisementVerificationStatusRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Moderator.UpdateTransportAdvertisementVerificationStatus,
                Message: ErrorMessages.ServiceError
            });
    }
}