using Application.Interfaces;
using Common.Results;
using Domain;
using Microsoft.Extensions.Logging;
using Persistence.Interfaces;

namespace Application.Services;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly ILogger<ImageService> _logger;

    public ImageService(
        IImageRepository imageRepository,
        ILogger<ImageService> logger)
    {
        _imageRepository = imageRepository;
        _logger = logger;
    }
    
    public async Task<Result> AddImage(Image img)
    {
        try
        {
            var isAdded = await _imageRepository.AddImage(img);
            return isAdded
                ? Result.Success()
                : Result.Failure(new Error(ErrorCodes.Image.AddImage, 
                    ErrorMessages.Image.ImageWasNotAdded));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.Image.AddImage, ErrorMessages.ServiceError));
        }
    }

    public async Task<Result> AddImages(ICollection<Image> images)
    {
        try
        {
            var isAdded = await _imageRepository.AddImages(images);
            return isAdded
                ? Result.Success()
                : Result.Failure(new Error(ErrorCodes.Image.AddImages,
                    ErrorMessages.Image.ImagesWereNotAdded));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return Result.Failure(new Error(ErrorCodes.Image.AddImages, ErrorMessages.ServiceError));
        }
    }
}