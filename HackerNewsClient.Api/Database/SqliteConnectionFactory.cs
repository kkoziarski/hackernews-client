using System.Data;
using HackerNewsClient.Api.Config;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace HackerNewsClient.Api.Database;

public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync();
}

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly IOptions<DatabaseOptions> _options;

    public SqliteConnectionFactory(IOptions<DatabaseOptions> options)
    {
        _options = options;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new SqliteConnection(_options.Value.ConnectionString);
        await connection.OpenAsync();
        return connection;
    }
}
