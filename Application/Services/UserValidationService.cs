using Application.Interfaces;
using Common.Requests;
using Common.Results;
using Infrastructure.PasswordHashing;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Services;

public class UserValidationService : IUserValidationService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly ILogger<UserValidationService> _logger;

    public UserValidationService(
        IUserRepository repository, 
        IPasswordHashingService passwordHashingService,
        ILogger<UserValidationService> logger)
    {
        _repository = repository;
        _passwordHashingService = passwordHashingService;
        _logger = logger;
    }
    
    public async Task<Result> ValidateRegisterModel(RegisterRequest registerRequest)
    {
        try
        {
            if (await _repository.IsEmailExist(registerRequest.Email))
            {
                return Result.Failure(new Error(ErrorCodes.UserValidation.ValidateRegisterModel, ErrorMessages.User.UserAlreadyExist));
            }
        
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.UserValidation.ValidateRegisterModel, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result> ValidateLoginModel(LoginRequest loginRequest)
    {
        try
        {
            if (!await _repository.IsEmailExist(loginRequest.Email))
            {
                return Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel, ErrorMessages.UserValidation.IncorrectCredentials));
            }

            var user = await _repository.GetByEmail(loginRequest.Email);

            if (!_passwordHashingService.VerifyPassword(loginRequest.Password, user.Password, user.Salt))
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
            _logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.UserValidation.ValidateLoginModel, ErrorMessages.ServiceError));
        }
    }
}