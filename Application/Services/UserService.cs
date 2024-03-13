using Application.Interfaces;
using Common.DTOs;
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
    public async Task<Result> Register(RegisterDto registerDto)
    {
        try
        {
            var validationResult = await validationService.ValidateRegisterModel(registerDto);

            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var user = new User()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Password = passwordHashingService.HashPassword(registerDto.Password, out var salt),
                Salt = salt,
                Country = registerDto.Country,
                City = registerDto.City,
                Address = registerDto.Address
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

    public async Task<Result<TokenDto>> Login(LoginDto loginDto)
    {
        try
        {
            var validationResult = await validationService.ValidateLoginModel(loginDto);
            
            if (validationResult.IsFailure)
            {
                return Result<TokenDto>.Failure(validationResult.Error);
            }

            var user = await repository.GetByEmail(loginDto.Email);

            return Result<TokenDto>.Success(new TokenDto()
            {
                Token = await jwtService.Generate(user)
            });
        }
        catch
        {
            return Result<TokenDto>.Failure(new Error("UserService.Login", "Server error"));
        }
    }

    public async Task<Result<UserInfoDto>> GetUser(string email)
    {
        var user = await repository.GetByEmail(email);

        if (user is null) return Result<UserInfoDto>.Failure(new Error("UserService.GetUser", "User was not found"));

        return Result<UserInfoDto>.Success(new UserInfoDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Country = user.Country,
            City = user.City,
            Address = user.Address,
            IsActive = user.IsActive,
            Permissions = user.Permissions.Select(p => p.Name).ToArray()
        });
    }

    private async Task AddUserToDefaultPermission(User user)
    {
        await adminService.AddUserToPermission(user, Permissions.User);
    }
}