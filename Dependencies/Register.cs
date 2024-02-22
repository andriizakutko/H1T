using Application.Interfaces;
using Application.Services;
using Common.Options;
using Infrastructure.Cache;
using Infrastructure.Data;
using Infrastructure.PasswordHashing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Interfaces;
using Persistence.Repositories;

namespace Dependencies;

public static class Register
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IPermissionRepository, PermissionRepository>();
        
        return services;
    }
    
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserValidationService, UserValidationService>();
        services.AddTransient<IPasswordHashingService, PasswordHashingService>();
        services.AddTransient<IJwtService, JwtService>();
        services.AddTransient<IAdminService, AdminService>();
        services.AddTransient<IPermissionService, PermissionService>();

        services.AddSingleton<ICacheProvider, CacheProvider>();
        
        return services;
    }

    public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("PosterDb")).UseLazyLoadingProxies();
        });
        
        return services;
    }

    public static IServiceCollection RegisterOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        return services;
    }
}