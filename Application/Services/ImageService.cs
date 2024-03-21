using Application.Interfaces;
using Common.Results;
using Domain;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Services;

public class ImageService(
    IImageRepository imageRepository,
    ILogger logger) : IImageService
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
            logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.Image.AddImage, ErrorMessages.ServiceError));
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
            logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.Image.AddImages, ErrorMessages.ServiceError));
        }
    }
}