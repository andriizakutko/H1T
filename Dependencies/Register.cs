using Application.Services;
using Common.Options;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Stores;

namespace Dependencies;

public static class Register
{
    public static IServiceCollection RegisterStores(this IServiceCollection services)
    {
        services.AddTransient<IUserStore, UserStore>();
        
        return services;
    }
    
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IUserValidationService, UserValidationService>();
        services.AddTransient<IPasswordHashingService, PasswordHashingService>();
        services.AddTransient<IJwtService, JwtService>();
        
        return services;
    }

    public static IServiceCollection RegisterDatabaseContext(this IServiceCollection services)
    {
        services.AddSingleton<DatabaseContext>();
        
        return services;
    }

    public static IServiceCollection RegisterOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        return services;
    }
}