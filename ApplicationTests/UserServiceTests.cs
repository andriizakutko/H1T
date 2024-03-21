using Application.Interfaces;
using Application.Services;
using Common.Requests;
using Common.Results;
using Domain;
using Infrastructure.PasswordHashing;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;

namespace ApplicationTests;

public class UserServiceTests
{
    private UserService _userService;
    private Mock<IUserValidationService> _mockUserValidationService;
    private Mock<IPasswordHashingService> _mockPasswordHashingService;
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IJwtService> _mockJwtService;
    private Mock<IAdminService> _mockAdminService;
    private Mock<ILogger> _mockLogger;
    
    [SetUp]
    public void Setup()
    {
        _mockUserValidationService = new Mock<IUserValidationService>();
        _mockPasswordHashingService = new Mock<IPasswordHashingService>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockJwtService = new Mock<IJwtService>();
        _mockAdminService = new Mock<IAdminService>();
        _mockLogger = new Mock<ILogger>();

        _userService = new UserService(
            _mockUserRepository.Object,
            _mockUserValidationService.Object,
            _mockPasswordHashingService.Object,
            _mockJwtService.Object,
            _mockAdminService.Object,
            _mockLogger.Object);
    }

    [Test]
    public async Task Register_ReturnsFailed_UserAlreadyExists()
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

        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(registerRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateRegisterModel, ErrorMessages.User.UserAlreadyExist)));

        var hashedPassword = "hashedPassword";
        byte[]? salt = default;
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(registerRequest.Password, out salt))
            .Returns(hashedPassword);

        var createdUser = new User
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Email = registerRequest.Email,
            Password = hashedPassword,
            Salt = salt,
            Country = registerRequest.Country,
            City = registerRequest.City,
            Address = registerRequest.Address
        };

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(createdUser);
        
        //Act
        var result = await _userService.Register(registerRequest);
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is 
                { Code: ErrorCodes.UserValidation.ValidateRegisterModel, 
                    Message: ErrorMessages.User.UserAlreadyExist }, Is.True);
    }
    
    [Test]
    public async Task Register_ReturnsSuccess_UserCreated()
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

        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(registerRequest)).ReturnsAsync(Result.Success());

        var hashedPassword = "hashedPassword";
        byte[]? salt = default;
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(registerRequest.Password, out salt))
            .Returns(hashedPassword);

        var createdUser = new User
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Email = registerRequest.Email,
            Password = hashedPassword,
            Salt = salt,
            Country = registerRequest.Country,
            City = registerRequest.City,
            Address = registerRequest.Address
        };

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(createdUser);
        
        //Act
        var result = await _userService.Register(registerRequest);
        
        //Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task Register_ReturnsFailed_UserValidationServiceError()
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

        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(registerRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateRegisterModel, ErrorMessages.ServiceError)));

        var hashedPassword = "hashedPassword";
        byte[]? salt = default;
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(registerRequest.Password, out salt))
            .Returns(hashedPassword);

        var createdUser = new User
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Email = registerRequest.Email,
            Password = hashedPassword,
            Salt = salt,
            Country = registerRequest.Country,
            City = registerRequest.City,
            Address = registerRequest.Address
        };

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>()))
            .ReturnsAsync(createdUser);
        
        //Act
        var result = await _userService.Register(registerRequest);
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is 
            { Code: ErrorCodes.UserValidation.ValidateRegisterModel, 
                Message: ErrorMessages.ServiceError }, Is.True);
    }

    [Test]
    public async Task Login_ReturnsFailed_UserEmailIsNotExists()
    {
        //Assign
        var loginRequest = new LoginRequest()
        {
            Email = "test@test.com",
            Password = "password"
        };

        _mockUserValidationService.Setup(x => x.ValidateLoginModel(loginRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel,
                ErrorMessages.UserValidation.IncorrectCredentials)));

        _mockUserRepository.Setup(x => x.IsEmailExist(loginRequest.Email))
            .ReturnsAsync(false);
        
        //Act
        var result = await _userService.Login(loginRequest);

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
    public async Task Login_ReturnsFailed_PasswordMismatch()
    {
        //Assign
        var loginRequest = new LoginRequest()
        {
            Email = "test@test.com",
            Password = "password"
        };

        _mockUserValidationService.Setup(x => x.ValidateLoginModel(loginRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel,
                ErrorMessages.UserValidation.IncorrectCredentials)));

        _mockUserRepository.Setup(x => x.IsEmailExist(loginRequest.Email))
            .ReturnsAsync(true);
        
        var hashedPassword = "hashedPassword";
        byte[]? salt = default;

        var user = new User()
        {
            Password = hashedPassword,
            Salt = salt
        };

        _mockUserRepository.Setup(x => x.GetByEmail(loginRequest.Email))
            .ReturnsAsync(user);

        _mockPasswordHashingService.Setup(x => x.VerifyPassword(loginRequest.Password, user.Password, user.Salt))
            .Returns(false);
        
        //Act
        var result = await _userService.Login(loginRequest);

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
    public async Task Login_ReturnsFailed_UserIsNotActive()
    {
        //Assign
        var loginRequest = new LoginRequest()
        {
            Email = "test@test.com",
            Password = "password"
        };

        _mockUserValidationService.Setup(x => x.ValidateLoginModel(loginRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel,
                ErrorMessages.User.UserNotActive)));

        _mockUserRepository.Setup(x => x.IsEmailExist(loginRequest.Email))
            .ReturnsAsync(true);
        
        var hashedPassword = "hashedPassword";
        byte[]? salt = default;

        var user = new User()
        {
            Password = hashedPassword,
            Salt = salt,
            IsActive = false
        };

        _mockUserRepository.Setup(x => x.GetByEmail(loginRequest.Email))
            .ReturnsAsync(user);

        _mockPasswordHashingService.Setup(x => x.VerifyPassword(loginRequest.Password, user.Password, user.Salt))
            .Returns(true);
        
        //Act
        var result = await _userService.Login(loginRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.UserValidation.ValidateLoginModel,
                Message: ErrorMessages.User.UserNotActive
            });
    }
    
    [Test]
    public async Task Login_ReturnsSuccess_UserWasCreatedAndTokenWasGenerated()
    {
        //Assign
        var loginRequest = new LoginRequest()
        {
            Email = "test@test.com",
            Password = "password"
        };

        _mockUserValidationService.Setup(x => x.ValidateLoginModel(loginRequest))
            .ReturnsAsync(Result.Success());
        
        var hashedPassword = "hashedPassword";
        byte[]? salt = default;

        var user = new User()
        {
            Email = loginRequest.Email,
            Password = hashedPassword,
            Salt = salt
        };

        _mockUserRepository.Setup(x => x.GetByEmail(loginRequest.Email))
            .ReturnsAsync(user);

        var token = "token";

        _mockJwtService.Setup(x => x.Generate(It.IsAny<User>()))
            .ReturnsAsync(token);
        
        //Act
        var result = await _userService.Login(loginRequest);

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value.Token == token);
    }
}