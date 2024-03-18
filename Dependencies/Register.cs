using System.Text;
using Application.Interfaces;
using Application.Services;
using Common.Options;
using Infrastructure.Authentication;
using Infrastructure.Cache;
using Infrastructure.Data;
using Infrastructure.PasswordHashing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence.Interfaces;
using Persistence.Repositories;

namespace Dependencies;

public static class Register
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IPermissionRepository, PermissionRepository>();
        services.AddTransient<ITransportRepository, TransportRepository>();
    }
    
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserValidationService, UserValidationService>();
        services.AddTransient<IPasswordHashingService, PasswordHashingService>();
        services.AddTransient<IJwtService, JwtService>();
        services.AddTransient<IAdminService, AdminService>();
        services.AddTransient<IPermissionService, PermissionService>();
        services.AddTransient<ITransportService, TransportService>();
    }

    public static void RegisterProviders(this IServiceCollection services)
    {
        services.AddSingleton<ICacheProvider, CacheProvider>();
    }

    public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("PosterDb")).UseLazyLoadingProxies();
        });
    }

    public static void RegisterOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<AdminOptions>(configuration.GetSection("Admin"));
    }

    public static void RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = new JwtOptions();
        configuration.GetSection("Jwt").Bind(jwtOptions);
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = jwtOptions.Audience,
                ValidIssuer = jwtOptions.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
            };
        });
    }
    
    public static void RegisterAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }
}