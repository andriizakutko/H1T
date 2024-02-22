using Domain;

namespace Persistence.Interfaces;

public interface IProductRepository
{
    Task<bool> Create(Product product);
}