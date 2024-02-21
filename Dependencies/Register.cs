using Application.Services;
using Common.Options;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Stores;

namespace Dependencies;

public static class Register
{
    public static IServiceCollection RegisterStores(this IServiceCollection services)
    {
        services.AddTransient<IUserStore, UserStore>();
        services.AddTransient<IPermissionStore, PermissionStore>();
        
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