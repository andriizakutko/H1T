using Domain;

namespace Persistence.Interfaces;

public interface IImageRepository
{
    Task<bool> AddImages(ICollection<Image> images);
}