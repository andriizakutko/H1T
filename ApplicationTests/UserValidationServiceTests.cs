using Application.Services;
using Common.Requests;
using Common.Results;
using Domain;
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
    private RegisterRequest _registerRequest;
    private LoginRequest _loginRequest;
    
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

        _registerRequest = new RegisterRequest()
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "password123",
            Country = "USA",
            City = "New York",
            Address = "123 Street"
        };

        _loginRequest = new LoginRequest()
        {
            Email = "test@test.com",
            Password = "password"
        };
    }

    [Test]
    public async Task ValidateRegisterModel_ReturnsSuccess_RegisterRequestIsValid()
    {
        //Assign
        _mockUserRepository.Setup(x => x.IsEmailExist(_registerRequest.Email))
            .ReturnsAsync(false);

        //Act
        var result = await _userValidationService.ValidateRegisterModel(_registerRequest);

        //Assert
        Assert.That(result.IsSuccess);
    }

    [Test]
    public async Task ValidateRegisterModel_ReturnFailed_EmailAlreadyExists()
    {
        //Assign
        _mockUserRepository.Setup(x => x.IsEmailExist(_registerRequest.Email))
            .ReturnsAsync(true);
        
        //Act
        var result = await _userValidationService.ValidateRegisterModel(_registerRequest);
        
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

    [Test]
    public async Task ValidateLoginModel_ReturnsSuccess_LoginRequestIsValid()
    {
        //Assign
        _mockUserRepository.Setup(x => x.IsEmailExist(_loginRequest.Email))
            .ReturnsAsync(true);

        var passwordHashed = "passwordHashed";
        byte[]? salt = default;
        
        var user = new User()
        {
            Email = _loginRequest.Email,
            Password = passwordHashed,
            Salt = salt,
            IsActive = true
        };

        _mockUserRepository.Setup(x => x.GetByEmail(_loginRequest.Email))
            .ReturnsAsync(user);

        _mockPasswordHashingService.Setup(x => x.VerifyPassword(_loginRequest.Password, user.Password, user.Salt))
            .Returns(true);

        //Act
        var result = await _userValidationService.ValidateLoginModel(_loginRequest);

        //Assert
        Assert.That(result.IsSuccess);
    }

    [Test]
    public async Task ValidateLoginModel_ReturnsFailed_IncorrectCredentials_EmailIsNotExists()
    {
        //Assign
        _mockUserRepository.Setup(x => x.IsEmailExist(_loginRequest.Email))
            .ReturnsAsync(false);

        //Act
        var result = await _userValidationService.ValidateLoginModel(_loginRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.UserValidation.ValidateLoginModel,
                Message: ErrorMessages.UserValidation.IncorrectCredentials
            });
    }
    
    [Test]
    public async Task ValidateLoginModel_ReturnsFailed_IncorrectCredentials_PasswordMismatch()
    {
        //Assign
        _mockUserRepository.Setup(x => x.IsEmailExist(_loginRequest.Email))
            .ReturnsAsync(true);
        
        var passwordHashed = "passwordHashed";
        byte[]? salt = default;
        
        var user = new User()
        {
            Email = _loginRequest.Email,
            Password = passwordHashed,
            Salt = salt,
            IsActive = true
        };

        _mockUserRepository.Setup(x => x.GetByEmail(_loginRequest.Email))
            .ReturnsAsync(user);

        _mockPasswordHashingService.Setup(x => x.VerifyPassword(_loginRequest.Password, user.Password, user.Salt))
            .Returns(false);

        //Act
        var result = await _userValidationService.ValidateLoginModel(_loginRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.UserValidation.ValidateLoginModel,
                Message: ErrorMessages.UserValidation.IncorrectCredentials
            });
    }
    
    [Test]
    public async Task ValidateLoginModel_ReturnsFailed_UserIsNotActive()
    {
        //Assign
        _mockUserRepository.Setup(x => x.IsEmailExist(_loginRequest.Email))
            .ReturnsAsync(true);
        
        var passwordHashed = "passwordHashed";
        byte[]? salt = default;
        
        var user = new User()
        {
            Email = _loginRequest.Email,
            Password = passwordHashed,
            Salt = salt,
            IsActive = false
        };

        _mockUserRepository.Setup(x => x.GetByEmail(_loginRequest.Email))
            .ReturnsAsync(user);

        _mockPasswordHashingService.Setup(x => x.VerifyPassword(_loginRequest.Password, user.Password, user.Salt))
            .Returns(true);

        //Act
        var result = await _userValidationService.ValidateLoginModel(_loginRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.UserValidation.ValidateLoginModel,
                Message: ErrorMessages.User.UserNotActive
            });
    }
}