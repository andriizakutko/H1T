using Application.Services;
using Common.Results;
using Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;

namespace ApplicationTests;

public class ImageServiceTests
{
    private Mock<IImageRepository> _mockImageRepository;
    private Mock<ILogger<ImageService>> _mockLogger;
    private ImageService _imageService;
    private Image _image;
    private List<Image> _images;
    
    [SetUp]
    public void SetUp()
    {
        _mockImageRepository = new Mock<IImageRepository>();
        _mockLogger = new Mock<ILogger<ImageService>>();

        _imageService = new ImageService(_mockImageRepository.Object, _mockLogger.Object);
        
        InitEntities();
    }

    private void InitEntities()
    {
        _image = new Image()
        {
            Url = "url"
        };
        
        _images = new List<Image>
        {
            new() { Url = "url1" },
            new() { Url = "url2" }
        };
    }

    [Test]
    public async Task AddImage_ReturnsSuccess_ImageWasAddedSuccessfully()
    {
        //Arrange
        _mockImageRepository.Setup(x => x.AddImage(_image))
            .ReturnsAsync(true);

        //Act
        var result = await _imageService.AddImage(_image);

        //Assert
        Assert.That(result.IsSuccess);
    }

    [Test]
    public async Task AddImages_ReturnsSuccess_ImagesWereAddedSuccessfully()
    {
        //Arrange
        _mockImageRepository.Setup(x => x.AddImages(_images))
            .ReturnsAsync(true);

        //Act
        var result = await _imageService.AddImages(_images);

        //Assert
        Assert.That(result.IsSuccess);
    }

    [Test]
    public async Task AddImages_ReturnsFailed_ServiceError()
    {
        // Arrange
        _mockImageRepository.Setup(x => x.AddImages(It.IsAny<List<Image>>()))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _imageService.AddImages(_images);

        //Assert
        Assert.That(result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Image.AddImages,
                Message: ErrorMessages.ServiceError
            });
    }

    [Test]
    public async Task AddImage_ReturnsFailed_ImageWasNotAdded()
    {
        //Arrange
        _mockImageRepository.Setup(x => x.AddImage(_image))
            .ReturnsAsync(false);

        //Act
        var result = await _imageService.AddImage(_image);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Image.AddImage,
                Message: ErrorMessages.Image.ImageWasNotAdded
            });
    }
    
    [Test]
    public async Task AddImages_ReturnsFailed_ImagesWereNotAdded()
    {
        //Arrange
        _mockImageRepository.Setup(x => x.AddImages(_images))
            .ReturnsAsync(false);

        //Act
        var result = await _imageService.AddImages(_images);

        //Assert
        Assert.That(
            result.IsFailure
            && result.Error is
            {
                Code: ErrorCodes.Image.AddImages,
                Message: ErrorMessages.Image.ImagesWereNotAdded
            });
    }
    
    [Test]
    public async Task AddImage_ReturnsFailed_ServiceError()
    {
        // Arrange
        _mockImageRepository.Setup(x => x.AddImage(It.IsAny<Image>()))
            .ThrowsAsync(new Exception("Some exception"));

        //Act
        var result = await _imageService.AddImage(_image);

        //Assert
        Assert.That(result.IsFailure
                    && result.Error is
                    {
                        Code: ErrorCodes.Image.AddImage,
                        Message: ErrorMessages.ServiceError
                    });
    }
}