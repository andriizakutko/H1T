using Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;

namespace ApplicationTests;

public class PermissionServiceTests
{
    private Mock<IPermissionRepository> _mockPermissionRepository;
    private Mock<ILogger<PermissionService>> _mockLogger;
    private PermissionService _permissionService;
    
    [SetUp]
    public void SetUp()
    {
        _mockPermissionRepository = new Mock<IPermissionRepository>();
        _mockLogger = new Mock<ILogger<PermissionService>>();

        _permissionService = new PermissionService(
            _mockPermissionRepository.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetPermissions_ReturnsSuccess_PermissionsWereReturnedSuccessfully()
    {
        //Arrange
        var userId = Guid.Parse("b99c7d95-f2f8-45c1-aa82-9e5fbfb0ec47");

        var permissionsReturned = new HashSet<string>()
        {
            "p1",
            "p2",
            "p3",
            "p4"
        };

        _mockPermissionRepository.Setup(x => x.GetUserPermissions(userId))
            .ReturnsAsync(permissionsReturned);

        //Act
        var result = await _permissionService.GetPermissions(userId);

        //Assert
        Assert.That(result.IsSuccess);
    }
}