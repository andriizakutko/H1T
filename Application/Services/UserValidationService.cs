using Application.Interfaces;
using Common.DTOs;
using Common.Results;
using Infrastructure.PasswordHashing;
using Persistence.Interfaces;

namespace Application.Services;

public class UserValidationService(IUserRepository repository, IPasswordHashingService passwordHashingService)
    : IUserValidationService
{
    public async Task<Result> ValidateRegisterModel(RegisterDto registerDto)
    {
        try
        {
            if (await repository.IsEmailExist(registerDto.Email))
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
            if (!await repository.IsEmailExist(loginDto.Email))
            {
                return Result.Failure(new Error("UserValidationService.ValidateLoginModel", "Incorrect credentials"));
            }

            var user = await repository.GetByEmail(loginDto.Email);

            if (!passwordHashingService.VerifyPassword(loginDto.Password, user.Password, user.Salt))
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