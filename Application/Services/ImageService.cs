using Application.Interfaces;
using Common.Results;
using Domain;
using Persistence.Interfaces;

namespace Application.Services;

public class ImageService(IImageRepository imageRepository) : IImageService
{
    public async Task<Result> AddImage(Image img)
    {
        try
        {
            await imageRepository.AddImage(img);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("ImageService.AddImage", ex.Message));
        }
    }

    public async Task<Result> AddImages(ICollection<Image> images)
    {
        try
        {
            await imageRepository.AddImages(images);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("ImageService.AddImages", ex.Message));
        }
    }
}