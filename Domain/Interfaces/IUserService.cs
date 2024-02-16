using Common.DTOs;
using Common.Results;

namespace Domain.Interfaces;

public interface IUserService
{
    Task<Result> Register(RegisterDto registerDto);
    Task<Result<TokenDto>> Login(LoginDto loginDto);
    Task<Result<UserDto>> GetUser(string email);
}