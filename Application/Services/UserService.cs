using Application.Interfaces;
using Common.Requests;
using Common.Responses;
using Common.Results;
using Domain;
using Infrastructure.Authentication;
using Infrastructure.PasswordHashing;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IUserValidationService _validationService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IJwtService _jwtService;
    private readonly IAdminService _adminService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository repository,
        IUserValidationService validationService,
        IPasswordHashingService passwordHashingService,
        IJwtService jwtService,
        IAdminService adminService,
        ILogger<UserService> logger)
    {
        _repository = repository;
        _validationService = validationService;
        _passwordHashingService = passwordHashingService;
        _jwtService = jwtService;
        _adminService = adminService;
        _logger = logger;
    }
    
    public async Task<Result> Register(RegisterRequest registerRequest)
    {
        try
        {
            var validationResult = await _validationService.ValidateRegisterModel(registerRequest);

            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var user = new User()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                Password = _passwordHashingService.HashPassword(registerRequest.Password, out var salt),
                Salt = salt,
                Country = registerRequest.Country,
                City = registerRequest.City,
                Address = registerRequest.Address
            };
        
            var createdUser = await _repository.Create(user);

            if (createdUser is null) return Result.Failure(new Error(ErrorCodes.User.Register, ErrorMessages.User.UserNotCreated));
            
            var result = await _adminService.AddUserToPermission(createdUser.Email, Permissions.User);

            return result.IsFailure 
                ? Result.Failure(result.Error) 
                : Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.User.Register, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<TokenResponse>> Login(LoginRequest loginRequest)
    {
        try
        {
            var validationResult = await _validationService.ValidateLoginModel(loginRequest);
            
            if (validationResult.IsFailure)
            {
                return Result<TokenResponse>.Failure(validationResult.Error);
            }

            var user = await _repository.GetByEmail(loginRequest.Email);

            var tokenResult = await _jwtService.Generate(user);

            if (tokenResult.IsFailure)
            {
                return Result<TokenResponse>.Failure(tokenResult.Error);
            }

            return Result<TokenResponse>.Success(new TokenResponse()
            {
                Token = tokenResult.Value
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<TokenResponse>.Failure(new Error(ErrorCodes.User.Login, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<UserInfoResponse>> GetUser(string email)
    {
        try
        {
            var user = await _repository.GetByEmail(email);

            if (user is null) return Result<UserInfoResponse>.Failure(new Error(ErrorCodes.User.GetUser, ErrorMessages.User.UserNotFound));

            var userPermissions = await _repository.GetUserPermissions(email);

            return Result<UserInfoResponse>.Success(new UserInfoResponse()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Country = user.Country,
                City = user.City,
                Address = user.Address,
                IsActive = user.IsActive,
                Permissions = userPermissions.Select(p => p.Permission.Name).ToArray()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<UserInfoResponse>.Failure(new Error(ErrorCodes.User.GetUser, ErrorMessages.ServiceError));
        }
    }
}