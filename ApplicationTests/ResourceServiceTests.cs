using System.Collections;
using Application.Services;
using Common.Results;
using Common.ServiceResults;
using Domain;
using Domain.Transport;
using Infrastructure.Cache;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;

namespace ApplicationTests;

public class ResourceServiceTests
{
    private Mock<IResourceRepository> _mockResourceRepository;
    private Mock<ICacheProvider> _mockCache;
    private Mock<ILogger<ResourceService>> _mockLogger;
    private ResourceService _resourceService;
    private IEnumerable<TransportType> _transportTypes;
    private IEnumerable<ValueResult> _returnedTransportTypes;
    private IEnumerable<ValueResult> _returnedTransportMakes;
    private IEnumerable<ValueResult> _returnedTransportModels;
    private IEnumerable<ValueResult> _returnedTransportBodyTypes;
    private IEnumerable<ModeratorOverviewStatus> _returnedModeratorOverviewStatuses;
     
    [SetUp]
    public void SetUp()
    {
        _mockResourceRepository = new Mock<IResourceRepository>();
        _mockCache = new Mock<ICacheProvider>();
        _mockLogger = new Mock<ILogger<ResourceService>>();

        _resourceService = new ResourceService(_mockResourceRepository.Object,
            _mockCache.Object, _mockLogger.Object);

        InitEntities();
    }

    private void InitEntities()
    {
        _transportTypes = new List<TransportType>()
        {
            new() { Name = "Some type1" },
            new() { Name = "Some type2" }
        };
        
        _returnedTransportTypes = new List<ValueResult>()
        {
            new() { Name = "Some type1" },
            new() { Name = "Some type2" }
        };

        _returnedTransportMakes = new List<ValueResult>()
        {
            new() { Name = "Some make1" },
            new() { Name = "Some make2" }
        };

        _returnedTransportBodyTypes = new List<ValueResult>()
        {
            new() { Name = "Some body type1" },
            new() { Name = "Some body type2" }
        };

        _returnedTransportModels = new List<ValueResult>()
        {
            new() { Name = "Some model1 " },
            new() { Name = "Some model2 " }
        };

        _returnedModeratorOverviewStatuses = new List<ModeratorOverviewStatus>()
        {
            new() { Name = "Status1" },
            new() { Name = "Status2" },
            new() { Name = "Status3" },
        };
    }

    [Test]
    public async Task GetTransportTypes_ReturnsSuccess_TransportTypesWereReturnedFromCache()
    {
        //Arrange
        _mockCache.Setup(x => x.Get<IEnumerable<ValueResult>>(CacheKeys.Transport.Types))
            .Returns(_returnedTransportTypes);

        //Act
        var result = await _resourceService.GetTransportTypes();
        
        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value is not null);
    }
    
    [Test]
    public async Task GetTransportTypes_ReturnsSuccess_TransportTypesWereReturnedFromDb()
    {
        //Arrange
        _mockCache.Setup(x => x.Get<IEnumerable<ValueResult>>(CacheKeys.Transport.Types))
            .Returns((IEnumerable<ValueResult>)null!);

        _mockResourceRepository.Setup(x => x.GetTransportTypes())
            .ReturnsAsync(_transportTypes);

        //Act
        var result = await _resourceService.GetTransportTypes();
        
        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value is not null);
    }
    
    [Test]
    public async Task GetTransportTypes_ReturnsFailed_ServiceError()
    {
        //Arrange
        _mockCache.Setup(x => x.Get<IEnumerable<ValueResult>>(CacheKeys.Transport.Types))
            .Returns((IEnumerable<ValueResult>)null!);

        _mockResourceRepository.Setup(x => x.GetTransportTypes())
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _resourceService.GetTransportTypes();
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Resource.GetTransportTypes,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task GetTransportMakesByTransportTypeId_ReturnsSuccess_TransportMakesWereReturnedFromCache()
    {
        //Arrange
        var id = Guid.Parse("d08c8913-5b84-4fb1-b72e-3c71d0b3e9fa");
        var key = $"{CacheKeys.Transport.Makes}-{id}";

        _mockCache.Setup(x => x.Get<IEnumerable<ValueResult>>(key))
            .Returns(_returnedTransportMakes);

        //Act
        var result = await _resourceService.GetTransportMakesByTransportTypeId(id);

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value != null);
    }
    
    [Test]
    public async Task GetTransportMakesByTransportTypeId_ReturnsSuccess_TransportMakesWereReturnedFromDb()
    {
        //Arrange
        var id = Guid.Parse("d08c8913-5b84-4fb1-b72e-3c71d0b3e9fa");
        var key = $"{CacheKeys.Transport.Makes}-{id}";

        _mockCache.Setup(x => x.Get<IEnumerable<ValueResult>>(key))
            .Returns((IEnumerable<ValueResult>)null!);

        var transportType = new TransportType() { Name = "Some type", TransportTypeMakes = new List<TransportTypeMake>()
        {
            new() { TransportMake = new TransportMake() { Name = "Some make" } }
        }};

        _mockResourceRepository.Setup(x => x.GetTransportTypeById(id))
            .ReturnsAsync(transportType);

        //Act
        var result = await _resourceService.GetTransportMakesByTransportTypeId(id);

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value != null);
    }
    
    [Test]
    public async Task GetTransportMakesByTransportTypeId_ReturnsFailed_ServiceError()
    {
        //Arrange
        var id = Guid.Parse("d08c8913-5b84-4fb1-b72e-3c71d0b3e9fa");
        var key = $"{CacheKeys.Transport.Makes}-{id}";

        _mockCache.Setup(x => x.Get<IEnumerable<ValueResult>>(key))
            .Returns((IEnumerable<ValueResult>)null!);
        
        _mockResourceRepository.Setup(x => x.GetTransportTypeById(id))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _resourceService.GetTransportMakesByTransportTypeId(id);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Resource.GetTransportMakesByTransportTypeId,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task GetTransportBodyTypesByTransportTypeId_ReturnsSuccess_BodyTypesWereReturnedFromCache()
    {
        //Arrange
        var id = Guid.Parse("d08c8913-5b84-4fb1-b72e-3c71d0b3e9fa");
        var key = $"{CacheKeys.Transport.BodyTypes}-{id}";

        _mockCache.Setup(x => x.Get<IEnumerable<ValueResult>>(key))
            .Returns(_returnedTransportBodyTypes);

        //Act
        var result = await _resourceService.GetTransportBodyTypesByTransportTypeId(id);

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value is not null);
    }
    
    [Test]
    public async Task GetTransportBodyTypesByTransportTypeId_ReturnsSuccess_BodyTypesWereReturnedFromDb()
    {
        //Arrange
        var id = Guid.Parse("d08c8913-5b84-4fb1-b72e-3c71d0b3e9fa");
        var key = $"{CacheKeys.Transport.BodyTypes}-{id}";

        _mockCache.Setup(x => x.Get<IEnumerable<ValueResult>>(key))
            .Returns((IEnumerable<ValueResult>)null!);
        
        var transportType = new TransportType() { Name = "Some type", TransportTypeBodyTypes = new List<TransportTypeBodyType>()
        {
            new() { TransportBodyType = new TransportBodyType() { Name = "Some body type1" }},
            new() { TransportBodyType = new TransportBodyType() { Name = "Some body type2" }}
        }};

        _mockResourceRepository.Setup(x => x.GetTransportTypeById(id))
            .ReturnsAsync(transportType);

        //Act
        var result = await _resourceService.GetTransportBodyTypesByTransportTypeId(id);

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value is not null);
    }
    
    [Test]
    public async Task GetTransportBodyTypesByTransportTypeId_ReturnsFailed_ServiceError()
    {
        //Arrange
        var id = Guid.Parse("d08c8913-5b84-4fb1-b72e-3c71d0b3e9fa");
        var key = $"{CacheKeys.Transport.BodyTypes}-{id}";

        _mockCache.Setup(x => x.Get<IEnumerable<ValueResult>>(key))
            .Returns((IEnumerable<ValueResult>)null!);

        _mockResourceRepository.Setup(x => x.GetTransportTypeById(id))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _resourceService.GetTransportBodyTypesByTransportTypeId(id);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Resource.GetTransportBodyTypesByTransportTypeId,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task GetTransportModelsByTransportMakeId_ReturnsSuccess_TransportModelsWereReturnedFromCache()
    {
        //Arrange
        var id = Guid.Parse("d08c8913-5b84-4fb1-b72e-3c71d0b3e9fa");
        var key = $"{CacheKeys.Transport.Models}-{id}";

        _mockCache.Setup(x => x.Get<IEnumerable<ValueResult>>(key))
            .Returns(_returnedTransportModels);

        //Act
        var result = await _resourceService.GetTransportModelsByTransportMakeId(id);

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value is not null);
    }
    
    [Test]
    public async Task GetTransportModelsByTransportMakeId_ReturnsSuccess_TransportModelsWereReturnedFromDb()
    {
        //Arrange
        var id = Guid.Parse("d08c8913-5b84-4fb1-b72e-3c71d0b3e9fa");
        var key = $"{CacheKeys.Transport.Models}-{id}";

        _mockCache.Setup(x => x.Get<IEnumerable<ValueResult>>(key))
            .Returns((IEnumerable<ValueResult>)null!);

        var transportMake = new TransportMake()
        {
            TransportMakeModels = new List<TransportMakeModel>()
            {
                new() { TransportModel = new TransportModel() { Name = "Some mode1" } },
                new() { TransportModel = new TransportModel() { Name = "Some mode2" } }
            }
        };

        _mockResourceRepository.Setup(x => x.GetTransportMakeById(id))
            .ReturnsAsync(transportMake);

        //Act
        var result = await _resourceService.GetTransportModelsByTransportMakeId(id);

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value is not null);
    }
    
    [Test]
    public async Task GetTransportModelsByTransportMakeId_ReturnsFailed_ServiceError()
    {
        //Arrange
        var id = Guid.Parse("d08c8913-5b84-4fb1-b72e-3c71d0b3e9fa");
        var key = $"{CacheKeys.Transport.Models}-{id}";

        _mockCache.Setup(x => x.Get<IEnumerable<ValueResult>>(key))
            .Returns((IEnumerable<ValueResult>)null!);

        _mockResourceRepository.Setup(x => x.GetTransportMakeById(id))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _resourceService.GetTransportModelsByTransportMakeId(id);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Resource.GetTransportModelsByTransportMakeId,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task GetModeratorOverviewStatuses_ReturnsSuccess_OverviewStatusesWereReturnedFromCache()
    {
        //Arrange
        _mockCache.Setup(x => x.Get<IEnumerable<ModeratorOverviewStatus>>(CacheKeys.Moderator.OverviewStatuses))
            .Returns(_returnedModeratorOverviewStatuses);

        //Act
        var result = await _resourceService.GetModeratorOverviewStatuses();

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value is not null);
    }
    
    [Test]
    public async Task GetModeratorOverviewStatuses_ReturnsSuccess_OverviewStatusesWereReturnedFromDb()
    {
        //Arrange
        _mockCache.Setup(x => x.Get<IEnumerable<ModeratorOverviewStatus>>(CacheKeys.Moderator.OverviewStatuses))
            .Returns((IEnumerable<ModeratorOverviewStatus>)null!);

        _mockResourceRepository.Setup(x => x.GetModeratorOverviewStatuses())
            .ReturnsAsync(_returnedModeratorOverviewStatuses);

        //Act
        var result = await _resourceService.GetModeratorOverviewStatuses();

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value is not null);
    }
    
    [Test]
    public async Task GetModeratorOverviewStatuses_ReturnsFailed_ServiceError()
    {
        //Arrange
        _mockCache.Setup(x => x.Get<IEnumerable<ModeratorOverviewStatus>>(CacheKeys.Moderator.OverviewStatuses))
            .Returns((IEnumerable<ModeratorOverviewStatus>)null!);

        _mockResourceRepository.Setup(x => x.GetModeratorOverviewStatuses())
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _resourceService.GetModeratorOverviewStatuses();

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Resource.GetModeratorOverviewStatuses,
                Message: ErrorMessages.ServiceError
            });
    }
}