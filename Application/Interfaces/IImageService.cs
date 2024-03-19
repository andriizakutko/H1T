using Common.Results;
using Domain;

namespace Application.Interfaces;

public interface IImageService
{
    Task<Result> AddImage(Image img);
    Task<Result> AddImages(ICollection<Image> images);
}