using Domain;

namespace Persistence.Interfaces;

public interface IImageRepository
{
    Task<bool> AddImage(Image img);
    Task<bool> AddImages(ICollection<Image> images);
}