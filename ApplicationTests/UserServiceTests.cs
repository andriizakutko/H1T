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
    private Mock<ILogger<UserService>> _mockLogger;
    private LoginRequest _loginRequest;
    private RegisterRequest _registerRequest;
    private string _hashedPassword;
    private byte[]? _salt;
    private User _createdUser;
    private string _email;
    
    [SetUp]
    public void Setup()
    {
        _mockUserValidationService = new Mock<IUserValidationService>();
        _mockPasswordHashingService = new Mock<IPasswordHashingService>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockJwtService = new Mock<IJwtService>();
        _mockAdminService = new Mock<IAdminService>();
        _mockLogger = new Mock<ILogger<UserService>>();

        _userService = new UserService(
            _mockUserRepository.Object,
            _mockUserValidationService.Object,
            _mockPasswordHashingService.Object,
            _mockJwtService.Object,
            _mockAdminService.Object,
            _mockLogger.Object);

        InitEntities();
    }

    private void InitEntities()
    {
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
        
        _hashedPassword = "hashedPassword";
        
        _salt = default;
        
        _createdUser = new User
        {
            FirstName = _registerRequest.FirstName,
            LastName = _registerRequest.LastName,
            Email = _registerRequest.Email,
            Password = _hashedPassword,
            Salt = _salt,
            Country = _registerRequest.Country,
            City = _registerRequest.City,
            Address = _registerRequest.Address
        };
        
        _email = "someuseremail@test.com";
    }

    [Test]
    public async Task Register_ReturnsFailed_UserAlreadyExists()
    {
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(_registerRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateRegisterModel, ErrorMessages.User.UserAlreadyExist)));
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(_registerRequest.Password, out _salt))
            .Returns(_hashedPassword);

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(_createdUser);
        
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
    public async Task Register_ReturnsFailed_UserWasNotCreated()
    {
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(_registerRequest))
            .ReturnsAsync(Result.Success());
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(_registerRequest.Password, out _salt))
            .Returns(_hashedPassword);

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync((User)null!);
        
        //Act
        var result = await _userService.Register(_registerRequest);
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is 
            { Code: ErrorCodes.User.Register, 
                Message: ErrorMessages.User.UserNotCreated 
            });
    }
    
    [Test]
    public async Task Register_ReturnsFailed_ServiceError()
    {
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(_registerRequest)).ReturnsAsync(Result.Success());
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(_registerRequest.Password, out _salt))
            .Returns(_hashedPassword);

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(_createdUser);

        _mockAdminService.Setup(x => x.AddUserToPermission(_createdUser.Email, Permissions.User))
            .ThrowsAsync(new Exception("Some exception"));
        
        //Act
        var result = await _userService.Register(_registerRequest);
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.User.Register,
                Message: ErrorMessages.ServiceError
            });
    }
    
    [Test]
    public async Task Register_ReturnsSuccess_UserCreated()
    {
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(_registerRequest)).ReturnsAsync(Result.Success());
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(_registerRequest.Password, out _salt))
            .Returns(_hashedPassword);

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(_createdUser);

        _mockAdminService.Setup(x => x.AddUserToPermission(_createdUser.Email, Permissions.User))
            .ReturnsAsync(Result.Success());
        
        //Act
        var result = await _userService.Register(_registerRequest);
        
        //Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    [Test]
    public async Task Register_ReturnsFailed_UserValidationServiceError()
    {
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(_registerRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateRegisterModel, ErrorMessages.ServiceError)));
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(_registerRequest.Password, out _salt))
            .Returns(_hashedPassword);

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>()))
            .ReturnsAsync(_createdUser);
        
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
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(_registerRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.Admin.AddUserToPermission,
                ErrorMessages.Admin.PermissionHasAlreadyAdded)));
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(_registerRequest.Password, out _salt))
            .Returns(_hashedPassword);

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>()))
            .ReturnsAsync(_createdUser);

        _mockAdminService.Setup(x => x.AddUserToPermission(_createdUser.Email, Permissions.User))
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
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateRegisterModel(_registerRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.Admin.AddUserToPermission, ErrorMessages.User.UserNotExists)));
        
        _mockPasswordHashingService.Setup(x => x.HashPassword(_registerRequest.Password, out _salt))
            .Returns(_hashedPassword);

        _mockUserRepository.Setup(x => x.Create(It.IsAny<User>()))
            .ReturnsAsync(_createdUser);

        _mockAdminService.Setup(x => x.AddUserToPermission(_createdUser.Email, Permissions.User))
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
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateLoginModel(_loginRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel,
                ErrorMessages.UserValidation.IncorrectCredentials)));

        _mockUserRepository.Setup(x => x.IsEmailExist(_loginRequest.Email))
            .ReturnsAsync(false);
        
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
    public async Task Login_ReturnsFailed_PasswordMismatch()
    {
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateLoginModel(_loginRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel,
                ErrorMessages.UserValidation.IncorrectCredentials)));

        _mockUserRepository.Setup(x => x.IsEmailExist(_loginRequest.Email))
            .ReturnsAsync(true);

        var user = new User()
        {
            Password = _hashedPassword,
            Salt = _salt
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
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateLoginModel(_loginRequest))
            .ReturnsAsync(Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel,
                ErrorMessages.User.UserNotActive)));

        _mockUserRepository.Setup(x => x.IsEmailExist(_loginRequest.Email))
            .ReturnsAsync(true);

        var user = new User()
        {
            Password = _hashedPassword,
            Salt = _salt,
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
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateLoginModel(_loginRequest))
            .ReturnsAsync(Result.Success());

        var user = new User()
        {
            Email = _loginRequest.Email,
            Password = _hashedPassword,
            Salt = _salt
        };

        _mockUserRepository.Setup(x => x.GetByEmail(_loginRequest.Email))
            .ReturnsAsync(user);

        var token = "token";

        _mockJwtService.Setup(x => x.Generate(It.IsAny<User>()))
            .ReturnsAsync(Result<string>.Success(token));
        
        //Act
        var result = await _userService.Login(_loginRequest);

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value.Token == token);
    }
    
    [Test]
    public async Task Login_ReturnsFailed_ServiceError()
    {
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateLoginModel(_loginRequest))
            .ReturnsAsync(Result.Success());

        _mockUserRepository.Setup(x => x.IsEmailExist(_loginRequest.Email))
            .ThrowsAsync(new Exception("Some exception"));
        
        //Act
        var result = await _userService.Login(_loginRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.User.Login,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task Login_ReturnsFailed_JwtServiceFailed_PermissionServiceError()
    {
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateLoginModel(_loginRequest))
            .ReturnsAsync(Result.Success());

        var user = new User()
        {
            Email = _loginRequest.Email,
            Password = _hashedPassword,
            Salt = _salt
        };

        _mockUserRepository.Setup(x => x.GetByEmail(_loginRequest.Email))
            .ReturnsAsync(user);
        
        _mockJwtService.Setup(x => x.Generate(It.IsAny<User>()))
            .ReturnsAsync(Result<string>.Failure(
                new Error(ErrorCodes.Permission.GetPermissions, ErrorMessages.ServiceError)));
        
        //Act
        var result = await _userService.Login(_loginRequest);
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Permission.GetPermissions,
                Message: ErrorMessages.ServiceError
            });
    }
    
    [Test]
    public async Task Login_ReturnsFailed_JwtServiceFailed_ServiceError()
    {
        //Arrange
        _mockUserValidationService.Setup(x => x.ValidateLoginModel(_loginRequest))
            .ReturnsAsync(Result.Success());

        var user = new User()
        {
            Email = _loginRequest.Email,
            Password = _hashedPassword,
            Salt = _salt
        };

        _mockUserRepository.Setup(x => x.GetByEmail(_loginRequest.Email))
            .ReturnsAsync(user);
        
        _mockJwtService.Setup(x => x.Generate(It.IsAny<User>()))
            .ReturnsAsync(Result<string>.Failure(
                new Error(ErrorCodes.Jwt.Generate, ErrorMessages.ServiceError)));
        
        //Act
        var result = await _userService.Login(_loginRequest);
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Jwt.Generate,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task GetUser_ReturnsSuccess_UserInfoWasReturned()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_email))
            .ReturnsAsync(_createdUser);

        var userPermissions = new List<UserPermission>()
        {
            new() { Permission = new Permission() { Name = "User"} } 
        };

        _mockUserRepository.Setup(x => x.GetUserPermissions(_email))
            .ReturnsAsync(userPermissions);

        //Act
        var result = await _userService.GetUser(_email);
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess);
            Assert.That(result.Value, Is.Not.Null);
        });
    }
    
    [Test]
    public async Task GetUser_ReturnsFailed_UserWasNotFound()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_email))
            .ReturnsAsync((User)null!);

        //Act
        var result = await _userService.GetUser(_email);
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.User.GetUser,
                Message: ErrorMessages.User.UserNotFound
            });
    }
    
    [Test]
    public async Task GetUser_ReturnsFailed_ServiceError()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_email))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _userService.GetUser(_email);
        
        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.User.GetUser,
                Message: ErrorMessages.ServiceError
            });
    }
}