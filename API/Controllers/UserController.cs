using Application.Interfaces;
using Common.Jwt;
using Common.Requests;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/user")]
public class UserController(IUserService service) : BaseApiController
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        return HandleResult(await service.Register(registerRequest));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        return HandleResult(await service.Login(loginRequest));
    }
    
    [HttpGet("get-user")]
    [HasPermission(Permissions.User)]
    public async Task<IActionResult> GetUser()
    {
        return HandleResult(await service.GetUser(HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimNames.Email)?.Value));
    }
}