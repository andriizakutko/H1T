using Common.DTOs;
using Common.Results;

namespace Application.Interfaces;

public interface IUserService
{
    Task<Result> Register(RegisterDto registerDto);
    Task<Result<TokenDto>> Login(LoginDto loginDto);
    Task<Result<UserInfoDto>> GetUser(string email);
}