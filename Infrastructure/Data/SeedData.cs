using Common.Options;
using Domain;
using Infrastructure.PasswordHashing;

namespace Infrastructure.Data;

public static class SeedData
{
    public static void Seed(
        ApplicationDbContext context, 
        IPasswordHashingService passwordHashingService, 
        AdminOptions options)
    {
        if (context.Users.Any()) return;
        
        var user = new User()
        {
            FirstName = options.FirstName,
            LastName = options.LastName,
            Email = options.Email,
            Password = passwordHashingService.HashPassword(options.Password, out var salt),
            Salt = salt,
        };

        var result = context.Users.Add(user);

        result.Entity.Permissions = new List<Permission>()
        {
            new() { Name = Authentication.Permissions.User },
            new() { Name = Authentication.Permissions.Admin },
            new() { Name = Authentication.Permissions.Moderator },
            new() { Name = Authentication.Permissions.SysAdmin }
        };

        context.SaveChanges();
    }
}