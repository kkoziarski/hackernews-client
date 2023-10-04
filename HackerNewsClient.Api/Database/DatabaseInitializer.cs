using Dapper;

namespace HackerNewsClient.Api.Database;

public class DatabaseInitializer
{
    private const string TABLE_NAME = "STORIES";
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var table = await connection.QueryAsync<string>($"SELECT name FROM sqlite_master WHERE type='table' AND name='{TABLE_NAME}';");
        var tableName = table.FirstOrDefault();
        if (!string.IsNullOrEmpty(tableName) && tableName == TABLE_NAME)
        {
            await connection.ExecuteAsync($"DROP TABLE {TABLE_NAME};");
        }

        await connection.ExecuteAsync(@$"
            BEGIN;
            CREATE TABLE {TABLE_NAME}
            (
                Id INTEGER PRIMARY KEY,
                Title TEXT NOT NULL,
                PostedBy TEXT NOT NULL,
                Uri TEXT NOT NULL,
                Time DATETIME NOT NULL,
                CommentCount INTEGER NOT NULL,
                Score INTEGER NOT NULL
            );
            CREATE INDEX {TABLE_NAME}_Id_IDX ON {TABLE_NAME} (Id);
            COMMIT;
        ");
    }
}
