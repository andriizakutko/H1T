using Application.Interfaces;
using Common.Jwt;
using Common.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/users")]
public class UsersController(IUserService service) : BaseApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        return HandleResult(await service.Register(registerRequest));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        return HandleResult(await service.Login(loginRequest));
    }

    [Authorize]
    [HttpGet("get-user")]
    public async Task<IActionResult> GetUser()
    {
        return HandleResult(await service.GetUser(HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimNames.Email)?.Value));
    }
}