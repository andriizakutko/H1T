using Domain;
using Infrastructure.Data;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public async Task<bool> Create(Product product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return true;
    }
}