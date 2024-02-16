using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Data;

public class DatabaseContext
{
    private readonly string _connectionString;

    public DatabaseContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PosterDb");
    }

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}