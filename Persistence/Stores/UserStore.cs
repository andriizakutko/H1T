using Dapper;
using Domain;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Persistence.Stores;

public class UserStore : IUserStore
{
    private readonly DatabaseContext _context;

    public UserStore(DatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Create(User user)
    {
        var sql = @"INSERT INTO Users VALUES (@id, @firstName, @lastName, @email, @password, @country, @city, @address, @isActive, @createdAt, @updatedAt, @salt)";
        
        using var connection = _context.CreateConnection();

        return await connection.ExecuteAsync(sql, user) > 0;
    }

    public async Task<User> GetByEmail(string email)
    {
        var sql = @"SELECT * FROM users WHERE email = @email";

        using var connection = _context.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { email });
    }

    public async Task<bool> IsEmailExist(string email)
    {
        var sql = @"SELECT COUNT(*) FROM users WHERE Email = @email";

        using var connection = _context.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(sql, new { email }) > 0;
    }
}