using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Common.Jwt;
using Common.Options;
using Common.Results;
using Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtService : IJwtService
{
    private readonly IPermissionService _permissionService;
    private readonly ILogger<JwtService> _logger;
    private readonly JwtOptions _jwtOptions;

    public JwtService(
        IOptions<JwtOptions> jwtOptions, 
        IPermissionService permissionService,
        ILogger<JwtService> logger
    )
    {
        _permissionService = permissionService;
        _logger = logger;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<Result<string>> Generate(User user)
    {
        try
        {
            var claims = new List<Claim>
            {
                new(JwtClaimNames.Id, user.Id.ToString()),
                new(JwtClaimNames.Email, user.Email),
                new(JwtClaimNames.FirstName, user.FirstName),
                new(JwtClaimNames.LastName, user.LastName)
            };

            var permissionsResult = await _permissionService.GetPermissions(user.Id);

            if (permissionsResult.IsFailure)
            {
                return Result<string>.Failure(permissionsResult.Error);
            }

            foreach (var permission in permissionsResult.Value)
            {
                claims.Add(new Claim(JwtClaimNames.Permissions, permission));
            }

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

            return Result<string>.Success(tokenValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result<string>.Failure(new Error(ErrorCodes.Jwt.Generate, ErrorMessages.ServiceError));
        }
    }
}