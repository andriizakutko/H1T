using Domain;
using Domain.Enums;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class SeedData
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

        var productTypes = new List<ProductType>()
        {
            new() { Name = ProductTypes.Outerwear.ToString() },
            new() { Name = ProductTypes.Underwear.ToString() },
            new() { Name = ProductTypes.Gloves.ToString() },
            new() { Name = ProductTypes.Hats.ToString() },
            new() { Name = ProductTypes.Hoodie.ToString() },
            new() { Name = ProductTypes.Trousers.ToString() },
            new() { Name = ProductTypes.TShirts.ToString() },
        };
        
        context.ProductTypes.AddRange(productTypes);
        
        var productCategories = new List<ProductCategory>()
        {
            new() { Name = ProductCategories.Male.ToString() },
            new() { Name = ProductCategories.Female.ToString() }
        };
        
        context.ProductCategories.AddRange(productCategories);

        context.SaveChanges();
    }
}