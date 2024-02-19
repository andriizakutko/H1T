using Common.DTOs;
using Common.Results;
using Domain;
using Domain.Interfaces;
using Infrastructure.Authentication;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserStore _store;
    private readonly IUserValidationService _validationService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IJwtService _jwtService;
    private readonly IAdminService _adminService;

    public UserService(
        IUserStore store, 
        IUserValidationService validationService, 
        IPasswordHashingService passwordHashingService,
        IJwtService jwtService,
        IAdminService adminService)
    {
        _store = store;
        _validationService = validationService;
        _passwordHashingService = passwordHashingService;
        _jwtService = jwtService;
        _adminService = adminService;
    }

    public async Task<Result> Register(RegisterDto registerDto)
    {
        try
        {
            var validationResult = await _validationService.ValidateRegisterModel(registerDto);

            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var user = new User()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Password = _passwordHashingService.HashPassword(registerDto.Password, out var salt),
                Salt = salt,
                Country = registerDto.Country,
                City = registerDto.City,
                Address = registerDto.Address
            };
        
            var isCreated = await _store.Create(user);

            if (!isCreated) return Result.Failure(new Error("UserService.Register", "User was not created"));
            
            await AddUserToDefaultPermission(user.Email);
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
            var validationResult = await _validationService.ValidateLoginModel(loginDto);
            
            if (validationResult.IsFailure)
            {
                return Result<TokenDto>.Failure(validationResult.Error);
            }

            var user = await _store.GetByEmail(loginDto.Email);

            return Result<TokenDto>.Success(new TokenDto()
            {
                Token = await _jwtService.Generate(user)
            });
        }
        catch
        {
            return Result<TokenDto>.Failure(new Error("UserService.Login", "Server error"));
        }
    }

    public async Task<Result<UserDto>> GetUser(string email)
    {
        var user = await _store.GetByEmail(email);

        if (user is null) return Result<UserDto>.Failure(new Error("UserService.GetUser", "User was not found"));

        return Result<UserDto>.Success(new UserDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Country = user.Country,
            City = user.City,
            Address = user.Address,
            IsActive = user.IsActive
        });
    }

    private async Task AddUserToDefaultPermission(string email)
    {
        await _adminService.AddUserToPermission(email, Permissions.User);
    }
}