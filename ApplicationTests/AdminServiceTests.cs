using System.Security.Claims;
using Application.Services;
using Common.Options;
using Common.Requests;
using Common.Results;
using Domain;
using Domain.StoreResults;
using Infrastructure.Authentication;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Persistence.Interfaces;

namespace ApplicationTests;

public class AdminServiceTests
{
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IPermissionRepository> _mockPermissionRepository;
    private Mock<IOptions<AdminOptions>> _mockOptions;
    private Mock<IHttpContextAccessor> _mockContextAccessor;
    private Mock<ILogger<AdminService>> _mockLogger;
    private AdminService _adminService;
    private AddUserToPermissionRequest _addUserToPermissionRequest;
    private DeleteUserFromPermissionRequest _deleteUserFromPermissionRequest;
    private User _expectedUser;
    
    [SetUp]
    public void SetUp()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPermissionRepository = new Mock<IPermissionRepository>();
        _mockOptions = new Mock<IOptions<AdminOptions>>();
        _mockContextAccessor = new Mock<IHttpContextAccessor>();
        _mockLogger = new Mock<ILogger<AdminService>>();

        _adminService = new AdminService(
            _mockUserRepository.Object,
            _mockPermissionRepository.Object,
            _mockOptions.Object,
            _mockContextAccessor.Object,
            _mockLogger.Object);

        InitEntities();
    }

    private void InitEntities()
    {
        _addUserToPermissionRequest = new AddUserToPermissionRequest()
        {
            Email = "john@gmail.com",
            PermissionName = Permissions.Moderator
        };

        _deleteUserFromPermissionRequest = new DeleteUserFromPermissionRequest()
        {
            Email = "john@gmail.com",
            PermissionName = Permissions.Moderator
        };

        _expectedUser = new User()
        {
            FirstName = "Some first name",
            LastName = "Some last name",
            Email = "john@gmail.com",
            Address = "Some address",
            Country = "Some country",
            City = "Some city",
            IsActive = true
        };
    }

    [Test]
    public async Task GetUsers_ReturnsSuccess()
    {
        //Arrange
        var options = new AdminOptions() { Email = "admin@example.com" };
        var contextEmail = "user@example.com";
        var user1Id = Guid.Parse("e4d2e3f7-4d87-4d22-af5b-97e2e499b596");

        var expectedUsers = new List<User>
        {
            new User
            {
                Id = user1Id,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Country = "USA",
                City = "New York",
                Address = "123 Main St",
                IsActive = true
            }
        };

        _mockUserRepository.Setup(repo => repo.GetAll()).ReturnsAsync(expectedUsers);
        _mockOptions.Setup(opt => opt.Value).Returns(options);
        _mockContextAccessor.Setup(accessor => accessor.HttpContext).Returns(new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, contextEmail)
            }, "mock"))
        });

        //Act
        var result = await _adminService.GetUsers();

        //Assert
        Assert.That(result.IsSuccess, Is.True);

        var filteredUsers = result.Value.ToList();
        Assert.That(filteredUsers, Has.Count.EqualTo(1));
        var userDetailsResponse = filteredUsers.First();
        Assert.Multiple(() =>
        {
            Assert.That(userDetailsResponse.Id, Is.EqualTo(user1Id)); // The second user should remain
            Assert.That(userDetailsResponse.FirstName, Is.EqualTo("John"));
            Assert.That(userDetailsResponse.LastName, Is.EqualTo("Doe"));
            Assert.That(userDetailsResponse.Email, Is.EqualTo("john@example.com"));
            Assert.That(userDetailsResponse.Country, Is.EqualTo("USA"));
            Assert.That(userDetailsResponse.City, Is.EqualTo("New York"));
            Assert.That(userDetailsResponse.Address, Is.EqualTo("123 Main St"));
            Assert.That(userDetailsResponse.IsActive, Is.True);
        });

        // Verify that repository method was called
        _mockUserRepository.Verify(repo => repo.GetAll(), Times.Once);
    }
    
    [Test]
    public async Task GetUsers_ReturnsFailed_ServiceError()
    {
        //Arrange
        var options = new AdminOptions() { Email = "admin@example.com" };
        var contextEmail = "user@example.com";
        var user1Id = Guid.Parse("e4d2e3f7-4d87-4d22-af5b-97e2e499b596");

        var expectedUsers = new List<User>
        {
            new User
            {
                Id = user1Id,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Country = "USA",
                City = "New York",
                Address = "123 Main St",
                IsActive = true
            }
        };

        _mockUserRepository.Setup(repo => repo.GetAll())
            .ThrowsAsync(new Exception("Some exception"));
       

        //Act
        var result = await _adminService.GetUsers();

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Admin.GetUsers,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task AddUserToPermission_ReturnsSuccess()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_addUserToPermissionRequest.Email))
            .ReturnsAsync(_expectedUser);

        _mockPermissionRepository.Setup(x => x.IsPermissionAdded(_expectedUser, _addUserToPermissionRequest.PermissionName))
            .ReturnsAsync(false);

        _mockPermissionRepository.Setup(x => x.AddUserPermission(_expectedUser, _addUserToPermissionRequest.PermissionName))
            .ReturnsAsync(true);

        //Act
        var result = await _adminService.AddUserToPermission(_addUserToPermissionRequest);

        //Assert
        Assert.That(result.IsSuccess);
    }
    
    [Test]
    public async Task AddUserToPermission_ReturnsFailed_UserNotExists()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_addUserToPermissionRequest.Email))
            .ReturnsAsync((User)null!);
        
        //Act
        var result = await _adminService.AddUserToPermission(_addUserToPermissionRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Admin.AddUserToPermission,
                Message: ErrorMessages.User.UserNotExists
            });
    }
    
    [Test]
    public async Task AddUserToPermission_ReturnsFailed_PermissionHasAlreadyAdded()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_addUserToPermissionRequest.Email))
            .ReturnsAsync(_expectedUser);

        _mockPermissionRepository.Setup(x => x.IsPermissionAdded(_expectedUser, _addUserToPermissionRequest.PermissionName))
            .ReturnsAsync(true);

        //Act
        var result = await _adminService.AddUserToPermission(_addUserToPermissionRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Admin.AddUserToPermission,
                Message: ErrorMessages.Admin.PermissionHasAlreadyAdded
            });
    }
    
    [Test]
    public async Task AddUserToPermission_ReturnsFailed_AddUserToPermissionFailed()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_addUserToPermissionRequest.Email))
            .ReturnsAsync(_expectedUser);

        _mockPermissionRepository.Setup(x => x.IsPermissionAdded(_expectedUser, _addUserToPermissionRequest.PermissionName))
            .ReturnsAsync(false);

        _mockPermissionRepository.Setup(x => x.AddUserPermission(_expectedUser, _addUserToPermissionRequest.PermissionName))
            .ReturnsAsync(false);

        //Act
        var result = await _adminService.AddUserToPermission(_addUserToPermissionRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Admin.AddUserToPermission,
                Message: ErrorMessages.Admin.AddUserToPermissionFailed
            });
    }
    
    [Test]
    public async Task AddUserToPermission_ReturnsFailed_ServiceError()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_addUserToPermissionRequest.Email))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _adminService.AddUserToPermission(_addUserToPermissionRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Admin.AddUserToPermission,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task GetUsersPermissions_ReturnsSuccess()
    {
        //Arrange
        var expectedUserPermissions = new List<UserPermissionResult>()
        {
            new() { Email = _addUserToPermissionRequest.Email, PermissionName = _addUserToPermissionRequest.PermissionName },
            new() { Email = _addUserToPermissionRequest.Email, PermissionName = Permissions.User }
        };

        _mockPermissionRepository.Setup(x => x.GetAll())
            .ReturnsAsync(expectedUserPermissions);

        //Act
        var result = await _adminService.GetUsersPermissions();

        //Assert
        Assert.That(
            result.IsSuccess
            && result.Value is not null);
    }
    
    [Test]
    public async Task GetUsersPermissions_ReturnsFailed_ServiceError()
    {
        //Arrange
        _mockPermissionRepository.Setup(x => x.GetAll())
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _adminService.GetUsersPermissions();

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Admin.GetUsersPermissions,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task DeleteUserFromPermission_ReturnsSuccess()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_deleteUserFromPermissionRequest.Email))
            .ReturnsAsync(_expectedUser);

        var expectedUserPermissions = new List<UserPermission>()
        {
            new() { Permission = new Permission() { Name = Permissions.User } },
            new() { Permission = new Permission() { Name = Permissions.Moderator } }
        };

        _mockUserRepository.Setup(x => x.GetUserPermissions(_deleteUserFromPermissionRequest.Email))
            .ReturnsAsync(expectedUserPermissions);

        _mockPermissionRepository.Setup(x => x.DeleteUserFromPermission(_expectedUser, _deleteUserFromPermissionRequest.PermissionName))
            .ReturnsAsync(true);

        //Act
        var result = await _adminService.DeleteUserFromPermission(_deleteUserFromPermissionRequest);

        //Assert
        Assert.That(result.IsSuccess);
    }
    
    [Test]
    public async Task DeleteUserFromPermission_ReturnsFailed_UserNotFound()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_deleteUserFromPermissionRequest.Email))
            .ReturnsAsync((User)null!);

        //Act
        var result = await _adminService.DeleteUserFromPermission(_deleteUserFromPermissionRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Admin.DeleteUserFromPermission,
                Message: ErrorMessages.User.UserNotFound
            });
    }
    
    [Test]
    public async Task DeleteUserFromPermission_ReturnsFailed_UserNotHavePermissionToDelete()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_deleteUserFromPermissionRequest.Email))
            .ReturnsAsync(_expectedUser);

        var expectedUserPermissions = new List<UserPermission>()
        {
            new() { Permission = new Permission() { Name = Permissions.User } }
        };

        _mockUserRepository.Setup(x => x.GetUserPermissions(_deleteUserFromPermissionRequest.Email))
            .ReturnsAsync(expectedUserPermissions);

        //Act
        var result = await _adminService.DeleteUserFromPermission(_deleteUserFromPermissionRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Admin.DeleteUserFromPermission,
                Message: ErrorMessages.Admin.UserNotHavePermissionToDelete
            });
    }
    
    [Test]
    public async Task DeleteUserFromPermission_ReturnsFailed_DeleteUserPermissionFailed()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_deleteUserFromPermissionRequest.Email))
            .ReturnsAsync(_expectedUser);

        var expectedUserPermissions = new List<UserPermission>()
        {
            new() { Permission = new Permission() { Name = Permissions.User } },
            new() { Permission = new Permission() { Name = Permissions.Moderator } }
        };

        _mockUserRepository.Setup(x => x.GetUserPermissions(_deleteUserFromPermissionRequest.Email))
            .ReturnsAsync(expectedUserPermissions);

        _mockPermissionRepository.Setup(x => x.DeleteUserFromPermission(_expectedUser, _deleteUserFromPermissionRequest.PermissionName))
            .ReturnsAsync(false);

        //Act
        var result = await _adminService.DeleteUserFromPermission(_deleteUserFromPermissionRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Admin.DeleteUserFromPermission,
                Message: ErrorMessages.Admin.DeleteUserFromPermissionFailed
            });
    }
    
    [Test]
    public async Task DeleteUserFromPermission_ReturnsFailed_ServiceError()
    {
        //Arrange
        _mockUserRepository.Setup(x => x.GetByEmail(_deleteUserFromPermissionRequest.Email))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _adminService.DeleteUserFromPermission(_deleteUserFromPermissionRequest);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Admin.DeleteUserFromPermission,
                Message: ErrorMessages.ServiceError
            });
    }
}