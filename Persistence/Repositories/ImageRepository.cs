using Domain;
using Infrastructure.Data;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class ImageRepository(ApplicationDbContext context) : IImageRepository
{
    public async Task<bool> AddImage(Image img)
    {
        await context.Images.AddAsync(img);
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddImages(ICollection<Image> images)
    {
        await context.Images.AddRangeAsync(images);
        return await context.SaveChangesAsync() > 0;
    }
}