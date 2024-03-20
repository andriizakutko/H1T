using Application.Interfaces;
using Common.Requests;
using Common.Responses;
using Common.Results;
using Domain;
using Infrastructure.Authentication;
using Infrastructure.PasswordHashing;
using Persistence.Interfaces;

namespace Application.Services;

public class UserService(
        IUserRepository repository,
        IUserValidationService validationService,
        IPasswordHashingService passwordHashingService,
        IJwtService jwtService,
        IAdminService adminService)
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

            if (createdUser is null) return Result.Failure(new Error("UserService.Register", "User was not created"));
            
            await AddUserToDefaultPermission(createdUser);
            return Result.Success();
        }
        catch
        {
            return Result.Failure(new Error("UserService.Register", "Server error"));
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

            return Result<TokenResponse>.Success(new TokenResponse()
            {
                Token = await jwtService.Generate(user)
            });
        }
        catch
        {
            return Result<TokenResponse>.Failure(new Error("UserService.Login", "Server error"));
        }
    }

    public async Task<Result<UserInfoResponse>> GetUser(string email)
    {
        var user = await repository.GetByEmail(email);

        if (user is null) return Result<UserInfoResponse>.Failure(new Error("UserService.GetUser", "User was not found"));

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

    private async Task AddUserToDefaultPermission(User user)
    {
        await adminService.AddUserToPermission(user, Permissions.User);
    }
}