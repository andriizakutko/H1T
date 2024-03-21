using Application.Services;
using Common.Requests;
using Common.Results;
using Infrastructure.PasswordHashing;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;

namespace ApplicationTests;

public class UserValidationServiceTests
{
    private UserValidationService _userValidationService;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IPasswordHashingService> _mockPasswordHashingService;
    private Mock<ILogger> _mockLogger;
    
    [SetUp]
    public void Setup()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHashingService = new Mock<IPasswordHashingService>();
        _mockLogger = new Mock<ILogger>();

        _userValidationService = new UserValidationService(
            _mockUserRepository.Object,
            _mockPasswordHashingService.Object,
            _mockLogger.Object
        );
    }

    [Test]
    public async Task ValidateRegisterModel_ReturnsSuccess_RegisterRequestIsValid()
    {
        //Assign
        var registerRequest = new RegisterRequest()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            Country = "USA",
            City = "New York",
            Address = "123 Street"
        };

        _mockUserRepository.Setup(x => x.IsEmailExist(registerRequest.Email))
            .ReturnsAsync(false);

        //Act
        var result = await _userValidationService.ValidateRegisterModel(registerRequest);

        //Assert
        Assert.That(result.IsSuccess);
    }

    [Test]
    public async Task ValidateRegisterModel_ReturnFailed_EmailAlreadyExists()
    {
        //Assign
        var registerRequest = new RegisterRequest()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            Country = "USA",
            City = "New York",
            Address = "123 Street"
        };
        
        _mockUserRepository.Setup(x => x.IsEmailExist(registerRequest.Email))
            .ReturnsAsync(true);
        
        //Act
        var result = await _userValidationService.ValidateRegisterModel(registerRequest);
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.UserValidation.ValidateRegisterModel,
                Message: ErrorMessages.User.UserAlreadyExist
            }
            );
    }
}