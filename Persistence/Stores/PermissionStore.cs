using Dapper;
using Domain;
using Domain.Interfaces;
using Domain.StoreResults;
using Infrastructure.Data;

namespace Persistence.Stores;

public class PermissionStore : IPermissionStore
{
    private readonly DatabaseContext _context;

    public PermissionStore(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Permission> GetByName(string name)
    {
        var sql = @"SELECT * FROM Permissions WHERE Name = @name";

        using var connection = _context.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Permission>(sql, new { name });
    }

    public async Task AddUserPermission(Guid userId, Guid permissionId)
    {
        var id = Guid.NewGuid();
        
        var sql = @"INSERT INTO UserPermissions (Id, UserId, PermissionId)
                    VALUES (@id, @userId, @permissionId)";

        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(sql, new { id, userId, permissionId });
    }

    public async Task<bool> IsPermissionAdded(Guid userId, Guid permissionId)
    {
        var sql = @"SELECT COUNT(*) FROM UserPermissions WHERE UserId = @userId AND PermissionId = @permissionId";
        
        using var connection = _context.CreateConnection();

        var count = await connection.ExecuteScalarAsync<int>(sql, new { userId, permissionId });

        return count > 0;
    }

    public async Task<IEnumerable<UserPermissionResult>> GetAll()
    {
        var sql = @"SELECT email, name as permissionname
                    FROM public.userpermissions as up
                    LEFT JOIN permissions as p
                    ON up.permissionid = p.id
                    LEFT JOIN users as u
                    ON up.userid = u.id";
        
        using var connection = _context.CreateConnection();

        return await connection.QueryAsync<UserPermissionResult>(sql);
    }

    public async Task<bool> DeleteUserFromPermission(Guid userId, Guid permissionId)
    {
        var sql = @"DELETE FROM UserPermissions WHERE UserId = @userId AND PermissionId = @permissionId";
        
        using var connection = _context.CreateConnection();

        return await connection.ExecuteAsync(sql, new { userId, permissionId }) > 0;
    }
}