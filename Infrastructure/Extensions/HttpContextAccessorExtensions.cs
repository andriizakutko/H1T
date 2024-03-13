using Common.Jwt;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Extensions;

public static class HttpContextAccessorExtensions
{
    public static string GetEmail(this IHttpContextAccessor contextAccessor)
    {
        return contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimNames.Email)?.Value;
    }
}