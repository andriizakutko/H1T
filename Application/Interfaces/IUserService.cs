using Common.Requests;
using Common.Responses;
using Common.Results;

namespace Application.Interfaces;

public interface IUserService
{
    Task<Result> Register(RegisterRequest registerRequest);
    Task<Result<TokenResponse>> Login(LoginRequest loginRequest);
    Task<Result<UserInfoResponse>> GetUser(string email);
}