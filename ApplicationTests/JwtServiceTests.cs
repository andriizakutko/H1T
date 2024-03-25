using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Application.Interfaces;
using Application.Services;
using Common.Options;
using Common.Results;
using Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace ApplicationTests;

public class JwtServiceTests
{
    private Mock<IPermissionService> _mockPermissionService;
    private Mock<IOptions<JwtOptions>> _mockJwtOptions;
    private Mock<ILogger<JwtService>> _mockLogger;
    private Mock<JwtSecurityTokenHandler> _mockJwtSecurityTokenHandler;
    private JwtService _jwtService;

    [SetUp]
    public void SetUp()
    {
        _mockPermissionService = new Mock<IPermissionService>();
        _mockJwtOptions = new Mock<IOptions<JwtOptions>>();
        var jwtOptions = new JwtOptions
        {
            Key = "Yx0P2xXXfLxWPDY4V4m/4CAHTk1zJ365vzm8UspNeNS/VWqgDcy8kXQ4JpXCcD2lyv4LQq03s24YBvIZqBG4Ue7L+IltkJiinV8ZvJ6URdTtFIgDGF1lm3CCcq6QeQrkWdZWOCYT3ROykk5640du5ndLcrzCjbytD/gWeh0t1CKXrQTl+wyocA9jDNWg+wq8r6Jeou7heeA4JDMSBs61PwIWmwb3LmS/x1OvDJLgkT9Cbe4xtZPF3mmror5XRUdr+3iTEIqcYrbh+XheyCEK6Uqhtg6Few6n7eyahKQeRFlL83hcNHI56b1VVPLkhxGI1rqQmKO0c3uCSD0B/19FxT1N0ZgAWCQX2k3P+2yI2Rc=test",
            Issuer = "your-issuer",
            Audience = "your-audience",
            Expiration = 1
        };
        _mockJwtOptions.Setup(x => x.Value).Returns(jwtOptions);
        _mockLogger = new Mock<ILogger<JwtService>>();
        _mockJwtSecurityTokenHandler = new Mock<JwtSecurityTokenHandler>();

        _jwtService = new JwtService(
            _mockJwtOptions.Object,
            _mockPermissionService.Object,
            _mockLogger.Object);
    }
    
    [Test]
    public async Task Generate_ReturnsSuccess_WhenPermissionsRetrievedSuccessfully()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe"
        };

        var permissions = new HashSet<string>() { "Permission1", "Permission2" }; 

        _mockPermissionService.Setup(x => x.GetPermissions(user.Id))
            .ReturnsAsync(Result<HashSet<string>>.Success(permissions));

        var expectedToken = "fakeTokenValue";
        
        _mockJwtSecurityTokenHandler
            .Setup(m => m.WriteToken(It.IsAny<JwtSecurityToken>()))
            .Returns(expectedToken);

        // Act
        var result = await _jwtService.Generate(user);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Not.Null);
        });
    }
    
    [Test]
    public async Task Generate_ReturnsFailed_GetPermissionsFailed()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe"
        };

        _mockPermissionService.Setup(x => x.GetPermissions(user.Id))
            .ReturnsAsync(Result<HashSet<string>>.Failure(new Error(ErrorCodes.Permission.GetPermissions, ErrorMessages.ServiceError)));

        // Act
        var result = await _jwtService.Generate(user);
        
        // Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Permission.GetPermissions,
                Message: ErrorMessages.ServiceError
            });
    }
    
    [Test]
    public async Task Generate_ReturnsFailed_ServiceError()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe"
        };

        _mockPermissionService.Setup(x => x.GetPermissions(user.Id))
            .ThrowsAsync(new Exception("Some exception"));

        // Act
        var result = await _jwtService.Generate(user);
        
        // Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Jwt.Generate,
                Message: ErrorMessages.ServiceError
            });
    }
}