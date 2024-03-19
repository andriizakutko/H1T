using Domain;
using Domain.Transport;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<TransportAdvertisement> TransportAdvertisements { get; set; }
    public DbSet<TransportAdvertisementImage> TransportAdvertisementImages { get; set; }
    public DbSet<TransportModel> TransportModels { get; set; }
    public DbSet<TransportMake> TransportMakes { get; set; }
    public DbSet<TransportType> TransportTypes { get; set; }
    public DbSet<TransportBodyType> TransportBodyTypes { get; set; }
    public DbSet<TransportTypeMake> TransportTypeMakes { get; set; }
    public DbSet<TransportTypeBodyType> TransportTypeBodyTypes { get; set; }
    public DbSet<TransportMakeModel> TransportMakeModels { get; set; }
    public DbSet<ModeratorOverviewStatus> ModeratorOverviewStatuses { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
}