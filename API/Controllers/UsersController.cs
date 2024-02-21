using Common.DTOs;
using Common.Jwt;
using Domain.Interfaces;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/users")]
public class UsersController(IUserService service) : BaseApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        return HandleResult(await service.Register(registerDto));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        return HandleResult(await service.Login(loginDto));
    }

    [Authorize]
    [HttpGet("get-user")]
    public async Task<IActionResult> GetUser()
    {
        return HandleResult(await service.GetUser(HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimNames.Email)?.Value));
    }
}