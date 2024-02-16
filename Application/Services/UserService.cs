using Common.DTOs;
using Common.Results;
using Domain;
using Domain.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserStore _store;
    private readonly IUserValidationService _validationService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IJwtService _jwtService;

    public UserService(
        IUserStore store, 
        IUserValidationService validationService, 
        IPasswordHashingService passwordHashingService,
        IJwtService jwtService)
    {
        _store = store;
        _validationService = validationService;
        _passwordHashingService = passwordHashingService;
        _jwtService = jwtService;
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
        
            var isCreated = await _store.Create(new User()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Password = _passwordHashingService.HashPassword(registerDto.Password, out var salt),
                Salt = salt,
                Country = registerDto.Country,
                City = registerDto.City,
                Address = registerDto.Address
            });

            return isCreated ? Result.Success() : Result.Failure(new Error("UserService.Register", "User was not created"));
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
                Token = _jwtService.Generate(user)
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
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Country = user.Country,
            City = user.City,
            Address = user.Address,
            IsActive = user.IsActive
        });
    }
}