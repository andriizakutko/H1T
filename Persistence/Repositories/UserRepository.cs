﻿using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAll()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User> Create(User user)
    {
        var result = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<User> GetByEmail(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> IsEmailExist(string email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<UserPermission>> GetUserPermissions(string email)
    {
        return await context.UserPermissions.Where(x => x.User.Email == email).ToListAsync();
    }
}