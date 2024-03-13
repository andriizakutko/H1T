using Domain;
using Domain.StoreResults;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class PermissionRepository(ApplicationDbContext context) : IPermissionRepository
{
    public async Task<Permission> GetByName(string permissionName)
    {
        return await context.Permissions.FirstOrDefaultAsync(x => x.Name == permissionName);
    }

    public async Task AddUserPermission(User user, string permissionName)
    {
        await context.UserPermissions.AddAsync(new UserPermission()
        {
            User = user, Permission = await GetByName(permissionName)
        });
        await context.SaveChangesAsync();
    }

    public async Task<bool> IsPermissionAdded(User user, string permissionName)
    {
        var userPermissions = await context.UserPermissions.Where(x => x.User.Email == user.Email)
            .Include(userPermission => userPermission.Permission).ToListAsync();
        return userPermissions.Any(p => p.Permission.Name == permissionName);
    }

    public async Task<IEnumerable<UserPermissionResult>> GetAll()
    {
        var userPermissions = await context.UserPermissions.ToListAsync();

        return (from userPermission in userPermissions select new UserPermissionResult() { Email = userPermission.User.Email, PermissionName = userPermission.Permission.Name }).ToList();
    }

    public async Task<bool> DeleteUserFromPermission(User user, string permissionName)
    {
        var userPermissionToDelete =
            await context.UserPermissions.Where(x => x.User.Email == user.Email && x.Permission.Name == permissionName).FirstOrDefaultAsync();
        if (userPermissionToDelete is null)
        {
            return false;
        }
        context.UserPermissions.Remove(userPermissionToDelete);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<HashSet<string>> GetUserPermissions(Guid userId)
    {
        var userPermissions = await context.UserPermissions.Where(u => u.User.Id == userId)
            .Include(userPermission => userPermission.Permission).ToListAsync();

        return await Task.FromResult(userPermissions.Select(x => x.Permission.Name).ToHashSet());
    }
}