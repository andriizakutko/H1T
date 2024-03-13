using Common.Options;
using Domain;
using Infrastructure.PasswordHashing;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class SeedData
{
    public static async Task Seed(
        ApplicationDbContext context, 
        IPasswordHashingService passwordHashingService, 
        AdminOptions options)
    {
        if (await context.Users.AnyAsync()) return;
        
        var user = new User()
        {
            FirstName = options.FirstName,
            LastName = options.LastName,
            Email = options.Email,
            Password = passwordHashingService.HashPassword(options.Password, out var salt),
            Salt = salt,
        };

        await context.Users.AddAsync(user);

        await context.SaveChangesAsync();

        var userPermission = new Permission() { Name = Authentication.Permissions.User };
        var moderatorPermission = new Permission() { Name = Authentication.Permissions.Moderator };
        var adminPermission = new Permission() { Name = Authentication.Permissions.Admin };
        var sysAdminPermission = new Permission() { Name = Authentication.Permissions.SysAdmin };

        var permissions = new List<Permission>
        {
            userPermission,
            moderatorPermission,
            adminPermission,
            sysAdminPermission
        };
        
        await context.Permissions.AddRangeAsync(permissions);

        await context.SaveChangesAsync();

        var userPermissions = new List<UserPermission>()
        {
            new() { User = user, Permission = userPermission },
            new() { User = user, Permission = moderatorPermission },
            new() { User = user, Permission = adminPermission },
            new() { User = user, Permission = sysAdminPermission },
        };

        await context.UserPermissions.AddRangeAsync(userPermissions);

        await context.SaveChangesAsync();
    }
}