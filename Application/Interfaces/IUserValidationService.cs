using Common.DTOs;
using Common.Results;

namespace Application.Interfaces;

public interface IUserValidationService
{
    Task<Result> ValidateRegisterModel(RegisterDto registerDto);
    Task<Result> ValidateLoginModel(LoginDto loginDto);
}