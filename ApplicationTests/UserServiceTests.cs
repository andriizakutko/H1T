using Application.Interfaces;
using Application.Services;
using Common.Requests;
using Common.Results;
using Domain;
using Infrastructure.Authentication;
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

    private LoginRequest _loginRequest;
    private RegisterRequest _registerRequest;
    
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

        _loginRequest = new LoginRequest()
        {
            Email = "test@test.com",
            Password = "password"
        };
        
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
    }

    [Test]
    public async Task Register_ReturnsFailed_UserAlreadyExists()
    {
        //Assign
        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(_registerRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateRegisterModel, ErrorMessages.User.UserAlreadyExist)));

        var hashedPassword = "hashedPassword";
        byte[]? salt = default;
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(_registerRequest.Password, out salt))
            .Returns(hashedPassword);

        var createdUser = new User
        {
            FirstName = _registerRequest.FirstName,
            LastName = _registerRequest.LastName,
            Email = _registerRequest.Email,
            Password = hashedPassword,
            Salt = salt,
            Country = _registerRequest.Country,
            City = _registerRequest.City,
            Address = _registerRequest.Address
        };

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(createdUser);
        
        //Act
        var result = await _userService.Register(_registerRequest);
        
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
        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(_registerRequest)).ReturnsAsync(Result.Success());

        var hashedPassword = "hashedPassword";
        byte[]? salt = default;
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(_registerRequest.Password, out salt))
            .Returns(hashedPassword);

        var createdUser = new User
        {
            FirstName = _registerRequest.FirstName,
            LastName = _registerRequest.LastName,
            Email = _registerRequest.Email,
            Password = hashedPassword,
            Salt = salt,
            Country = _registerRequest.Country,
            City = _registerRequest.City,
            Address = _registerRequest.Address
        };

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(createdUser);

        _mockAdminService.Setup(x => x.AddUserToPermission(createdUser.Email, Permissions.User))
            .ReturnsAsync(Result.Success());
        
        //Act
        var result = await _userService.Register(_registerRequest);
        
        //Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task Register_ReturnsFailed_UserValidationServiceError()
    {
        //Assign
        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(_registerRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateRegisterModel, ErrorMessages.ServiceError)));

        var hashedPassword = "hashedPassword";
        byte[]? salt = default;
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(_registerRequest.Password, out salt))
            .Returns(hashedPassword);

        var createdUser = new User
        {
            FirstName = _registerRequest.FirstName,
            LastName = _registerRequest.LastName,
            Email = _registerRequest.Email,
            Password = hashedPassword,
            Salt = salt,
            Country = _registerRequest.Country,
            City = _registerRequest.City,
            Address = _registerRequest.Address
        };

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>()))
            .ReturnsAsync(createdUser);
        
        //Act
        var result = await _userService.Register(_registerRequest);
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is 
            { Code: ErrorCodes.UserValidation.ValidateRegisterModel, 
                Message: ErrorMessages.ServiceError }, Is.True);
    }
    
    [Test]
    public async Task Register_ReturnsFailed_FailedAddUserToPermission_PermissionHasAlreadyAdded()
    {
        //Assign
        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(_registerRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.Admin.AddUserToPermission,
                ErrorMessages.Admin.PermissionHasAlreadyAdded)));

        var hashedPassword = "hashedPassword";
        byte[]? salt = default;
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(_registerRequest.Password, out salt))
            .Returns(hashedPassword);

        var createdUser = new User
        {
            FirstName = _registerRequest.FirstName,
            LastName = _registerRequest.LastName,
            Email = _registerRequest.Email,
            Password = hashedPassword,
            Salt = salt,
            Country = _registerRequest.Country,
            City = _registerRequest.City,
            Address = _registerRequest.Address
        };

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>()))
            .ReturnsAsync(createdUser);

        _mockAdminService.Setup(x => x.AddUserToPermission(createdUser.Email, Permissions.User))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.Admin.AddUserToPermission,
                ErrorMessages.Admin.PermissionHasAlreadyAdded)));
        
        //Act
        var result = await _userService.Register(_registerRequest);
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is 
            { Code: ErrorCodes.Admin.AddUserToPermission, 
                Message: ErrorMessages.Admin.PermissionHasAlreadyAdded }, Is.True);
    }
    
    [Test]
    public async Task Register_ReturnsFailed_FailedAddUserToPermission_UserNotExists()
    {
        //Assign
        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(_registerRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.Admin.AddUserToPermission, ErrorMessages.User.UserNotExists)));

        var hashedPassword = "hashedPassword";
        byte[]? salt = default;
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(_registerRequest.Password, out salt))
            .Returns(hashedPassword);

        var createdUser = new User
        {
            FirstName = _registerRequest.FirstName,
            LastName = _registerRequest.LastName,
            Email = _registerRequest.Email,
            Password = hashedPassword,
            Salt = salt,
            Country = _registerRequest.Country,
            City = _registerRequest.City,
            Address = _registerRequest.Address
        };

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>()))
            .ReturnsAsync(createdUser);

        _mockAdminService.Setup(x => x.AddUserToPermission(createdUser.Email, Permissions.User))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.Admin.AddUserToPermission, ErrorMessages.User.UserNotExists)));
        
        //Act
        var result = await _userService.Register(_registerRequest);
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is 
            { Code: ErrorCodes.Admin.AddUserToPermission, 
                Message: ErrorMessages.User.UserNotExists }, Is.True);
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
        _mockUserValidationService.Setup(x => x.ValidateLoginModel(_loginRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel,
                ErrorMessages.UserValidation.IncorrectCredentials)));

        _mockUserRepository.Setup(x => x.IsEmailExist(_loginRequest.Email))
            .ReturnsAsync(true);
        
        var hashedPassword = "hashedPassword";
        byte[]? salt = default;

        var user = new User()
        {
            Password = hashedPassword,
            Salt = salt
        };

        _mockUserRepository.Setup(x => x.GetByEmail(_loginRequest.Email))
            .ReturnsAsync(user);

        _mockPasswordHashingService.Setup(x => x.VerifyPassword(_loginRequest.Password, user.Password, user.Salt))
            .Returns(false);
        
        //Act
        var result = await _userService.Login(_loginRequest);

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
        _mockUserValidationService.Setup(x => x.ValidateLoginModel(_loginRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel,
                ErrorMessages.User.UserNotActive)));

        _mockUserRepository.Setup(x => x.IsEmailExist(_loginRequest.Email))
            .ReturnsAsync(true);
        
        var hashedPassword = "hashedPassword";
        byte[]? salt = default;

        var user = new User()
        {
            Password = hashedPassword,
            Salt = salt,
            IsActive = false
        };

        _mockUserRepository.Setup(x => x.GetByEmail(_loginRequest.Email))
            .ReturnsAsync(user);

        _mockPasswordHashingService.Setup(x => x.VerifyPassword(_loginRequest.Password, user.Password, user.Salt))
            .Returns(true);
        
        //Act
        var result = await _userService.Login(_loginRequest);

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
        _mockUserValidationService.Setup(x => x.ValidateLoginModel(_loginRequest))
            .ReturnsAsync(Result.Success());
        
        var hashedPassword = "hashedPassword";
        byte[]? salt = default;

        var user = new User()
        {
            Email = _loginRequest.Email,
            Password = hashedPassword,
            Salt = salt
        };

        _mockUserRepository.Setup(x => x.GetByEmail(_loginRequest.Email))
            .ReturnsAsync(user);

        var token = "token";

        _mockJwtService.Setup(x => x.Generate(It.IsAny<User>()))
            .ReturnsAsync(token);
        
        //Act
        var result = await _userService.Login(_loginRequest);

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value.Token == token);
    }
}