using Domain;
using Infrastructure.Data;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class ImageRepository(ApplicationDbContext context) : IImageRepository
{
    public async Task AddImage(Image img)
    {
        await context.Images.AddAsync(img);
        await context.SaveChangesAsync();
    }

    public async Task AddImages(ICollection<Image> images)
    {
        await context.Images.AddRangeAsync(images);
        await context.SaveChangesAsync();
    }
}