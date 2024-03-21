using Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Interfaces;

namespace ApplicationTests;

public class ImageServiceTests
{
    private Mock<IImageRepository> _mockImageRepository;
    private Mock<ILogger> _mockLogger;
    private ImageService _imageService;
    
    [SetUp]
    public void SetUp()
    {
        _mockImageRepository = new Mock<IImageRepository>();
        _mockLogger = new Mock<ILogger>();

        _imageService = new ImageService(_mockImageRepository.Object, _mockLogger.Object);
    }
}