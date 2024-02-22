using Common.DTOs;
using Common.Results;
using Domain;

namespace Application.Interfaces;

public interface IProductService
{
    Task<Result> Create(ProductDto productDto);
    Task<Result<ICollection<Product>>> Get(string category = null, string type = null);
    Task<Result<Product>> GetById(Guid id);
    Task<Result> Update(Guid id, ProductDto productToUpdate);
    Task<Result> Delete(Guid id);
}