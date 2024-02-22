using Common.DTOs;
using Common.Results;

namespace Application.Interfaces;

public interface IProductService
{
    Task<Result> Create(ProductDto productDto);
}