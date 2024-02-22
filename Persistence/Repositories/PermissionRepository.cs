using Domain;
using Domain.StoreResults;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class PermissionRepository(ApplicationDbContext context) : IPermissionRepository
{
    public async Task AddUserPermission(User user, string permissionName)
    {
        user.Permissions ??= new List<Permission>();
        user.Permissions.Add(new Permission { Name = permissionName });
        await context.SaveChangesAsync();
    }

    public Task<bool> IsPermissionAdded(User user, string permissionName)
    {
        return Task.FromResult(user.Permissions is not null && user.Permissions.Any(p => p.Name == permissionName));
    }

    public async Task<IEnumerable<UserPermissionResult>> GetAll()
    {
        var users = await context.Users.Include(user => user.Permissions).ToListAsync();

        return (from user in users from permission in user.Permissions select new UserPermissionResult() { Email = user.Email, PermissionName = permission.Name }).ToList();
    }

    public async Task<bool> DeleteUserFromPermission(User user, string permissionName)
    {
        var result = user.Permissions.Remove(user.Permissions.First(p => p.Name == permissionName));
        await context.SaveChangesAsync();
        return result;
    }

    public async Task<HashSet<string>> GetUserPermissions(Guid userId)
    {
        var user = await context.Users.Include(user => user.Permissions).FirstAsync(u => u.Id == userId);

        return await Task.FromResult(user.Permissions.Select(x => x.Name).ToHashSet());
    }
}