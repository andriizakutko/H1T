using Common.Requests;
using Common.Results;

namespace Application.Interfaces;

public interface IUserValidationService
{
    Task<Result> ValidateRegisterModel(RegisterRequest registerRequest);
    Task<Result> ValidateLoginModel(LoginRequest loginRequest);
}