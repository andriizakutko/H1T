using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Jwt;
using Common.Options;
using Domain;
using Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions;
    
    public JwtService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    public string Generate(User user)
    {
        var claims = new Claim[]
        {
            new Claim(JwtClaimNames.Id, user.Id.ToString()),
            new Claim(JwtClaimNames.Email, user.Email),
            new Claim(JwtClaimNames.FirstName, user.FirstName),
            new Claim(JwtClaimNames.LastName, user.LastName)
        };

        var sighingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)),
            SecurityAlgorithms.HmacSha256);
    
        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(_jwtOptions.Expiration),
            sighingCredentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}