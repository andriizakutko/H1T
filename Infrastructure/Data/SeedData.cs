﻿using Domain;
using Infrastructure.PasswordHashing;

namespace Infrastructure.Data;

public static class SeedData
{
    public static void Seed(ApplicationDbContext context, IPasswordHashingService passwordHashingService)
    {
        if (context.Users.Any()) return;
        
        var user = new User()
        {
            FirstName = "admin",
            LastName = "admin",
            Email = "admin@test.com",
            Password = passwordHashingService.HashPassword("Pa$$w0rd", out var salt),
            Salt = salt,
        };

        var result = context.Users.Add(user);

        result.Entity.Permissions = new List<Permission>()
        {
            new() { Name = Authentication.Permissions.User },
            new() { Name = Authentication.Permissions.Admin },
            new() { Name = Authentication.Permissions.Moderator },
        };

        context.SaveChanges();
    }
}