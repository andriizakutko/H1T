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

public class UserService(
        IUserRepository repository,
        IUserValidationService validationService,
        IPasswordHashingService passwordHashingService,
        IJwtService jwtService,
        IAdminService adminService,
        ILogger logger)
    : IUserService
{
    public async Task<Result> Register(RegisterRequest registerRequest)
    {
        try
        {
            var validationResult = await validationService.ValidateRegisterModel(registerRequest);

            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var user = new User()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                Password = passwordHashingService.HashPassword(registerRequest.Password, out var salt),
                Salt = salt,
                Country = registerRequest.Country,
                City = registerRequest.City,
                Address = registerRequest.Address
            };
        
            var createdUser = await repository.Create(user);

            if (createdUser is null) return Result.Failure(new Error(ErrorCodes.User.Register, ErrorMessages.User.UserNotCreated));
            
            var result = await adminService.AddUserToPermission(createdUser.Email, Permissions.User);

            return result.IsFailure 
                ? Result.Failure(result.Error) 
                : Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.User.Register, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<TokenResponse>> Login(LoginRequest loginRequest)
    {
        try
        {
            var validationResult = await validationService.ValidateLoginModel(loginRequest);
            
            if (validationResult.IsFailure)
            {
                return Result<TokenResponse>.Failure(validationResult.Error);
            }

            var user = await repository.GetByEmail(loginRequest.Email);

            var tokenResult = await jwtService.Generate(user);

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
            logger.LogError(ex.Message);
            return Result<TokenResponse>.Failure(new Error(ErrorCodes.User.Login, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result<UserInfoResponse>> GetUser(string email)
    {
        try
        {
            var user = await repository.GetByEmail(email);

            if (user is null) return Result<UserInfoResponse>.Failure(new Error(ErrorCodes.User.GetUser, ErrorMessages.User.UserNotFound));

            var userPermissions = await repository.GetUserPermissions(email);

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
            logger.LogError(ex.Message);
            return Result<UserInfoResponse>.Failure(new Error(ErrorCodes.User.GetUser, ErrorMessages.ServiceError));
        }
    }
}