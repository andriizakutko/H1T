using Application.Interfaces;
using Common.Requests;
using Common.Results;
using Infrastructure.PasswordHashing;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Services;

public class UserValidationService(
        IUserRepository repository, 
        IPasswordHashingService passwordHashingService,
        ILogger logger) : IUserValidationService
{
    public async Task<Result> ValidateRegisterModel(RegisterRequest registerRequest)
    {
        try
        {
            if (await repository.IsEmailExist(registerRequest.Email))
            {
                return Result.Failure(new Error(ErrorCodes.UserValidation.ValidateRegisterModel, ErrorMessages.User.UserAlreadyExist));
            }
        
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.UserValidation.ValidateRegisterModel, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result> ValidateLoginModel(LoginRequest loginRequest)
    {
        try
        {
            if (!await repository.IsEmailExist(loginRequest.Email))
            {
                return Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel, ErrorMessages.UserValidation.IncorrectCredentials));
            }

            var user = await repository.GetByEmail(loginRequest.Email);

            if (!passwordHashingService.VerifyPassword(loginRequest.Password, user.Password, user.Salt))
            {
                return Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel, ErrorMessages.UserValidation.IncorrectCredentials));
            }

            if (!user.IsActive)
            {
                return Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel, ErrorMessages.User.UserNotActive));
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel, ErrorMessages.ServiceError));
        }
    }
}