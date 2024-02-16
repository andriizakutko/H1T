
using Common.DTOs;
using Common.Results;
using Domain.Interfaces;

namespace Application.Services;

public class UserValidationService : IUserValidationService
{
    private readonly IUserStore _store;
    private readonly IPasswordHashingService _passwordHashingService;

    public UserValidationService(IUserStore store, IPasswordHashingService passwordHashingService)
    {
        _store = store;
        _passwordHashingService = passwordHashingService;
    }

    public async Task<Result> ValidateRegisterModel(RegisterDto registerDto)
    {
        try
        {
            if (await _store.IsEmailExist(registerDto.Email))
            {
                return Result.Failure(new Error("UserValidationService.ValidateRegisterModel", "User already exists with this email"));
            }
        
            return Result.Success();
        }
        catch
        {
            return Result.Failure(new Error("UserValidationService.ValidateRegisterModel", "Server error"));
        }
    }

    public async Task<Result> ValidateLoginModel(LoginDto loginDto)
    {
        try
        {
            if (!await _store.IsEmailExist(loginDto.Email))
            {
                return Result.Failure(new Error("UserValidationService.ValidateLoginModel", "Incorrect credentials"));
            }

            var user = await _store.GetByEmail(loginDto.Email);

            if (!_passwordHashingService.VerifyPassword(loginDto.Password, user.Password, user.Salt))
            {
                return Result.Failure(new Error("UserValidationService.ValidateLoginModel", "Incorrect credentials"));
            }

            if (!user.IsActive)
            {
                return Result.Failure(new Error("UserValidationService.ValidateLoginModel", "User is not active"));
            }

            return Result.Success();
        }
        catch
        {
            return Result.Failure(new Error("UserValidationService.ValidateLoginModel", "Server error"));
        }
    }
}