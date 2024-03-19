using Domain;

namespace Persistence.Interfaces;

public interface IImageRepository
{
    Task AddImage(Image img);
    Task AddImages(ICollection<Image> images);
}