using Application.Interfaces;
using Common.DTOs;
using Common.Results;
using Domain;
using Persistence.Interfaces;

namespace Application.Services;

public class ProductService(
    IProductRepository repository,
    IImageRepository imageRepository) 
    : IProductService
{
    public async Task<Result> Create(ProductDto productDto)
    {
        try
        {
            var images = new List<Image>();
            
            if (productDto.ImageUrls.Length > 0)
            {
                images = productDto.ImageUrls.Select(imgUrl => new Image() { Url = imgUrl }).ToList();

                var isAdded = await imageRepository.AddImages(images);
                
                if (!isAdded) return Result.Failure(new Error("ProductService.Create", "Images were not added"));
            }

            var isCreated = await repository.Create(new Product()
            {
                Name = productDto.Name,
                SubName = productDto.SubName,
                ShortDescription = productDto.ShortDescription,
                Description = productDto.Description,
                OldPrice = productDto.OldPrice,
                Price = productDto.Price,
                Discount = productDto.Discount,
                Category = productDto.Category,
                Type = productDto.Type,
                Gender = productDto.Gender,
                Images = images
            });

            return isCreated ? Result.Success() : Result.Failure(new Error("ProductService.Create", "Product was not created"));
        }
        catch
        {
            return Result.Failure(new Error("ProductService.Create", "Product service error"));
        }
    }

    public Task<Result<ICollection<Product>>> Get(string category = null, string type = null)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Product>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Update(Guid id, ProductDto productToUpdate)
    {
        throw new NotImplementedException();
    }

    public Task<Result> Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}