using Dapper;
using HackerNewsClient.Api.Database;

namespace HackerNewsClient.Api.Repositories;

public interface IStoriesRepository
{
    Task<IEnumerable<StoryEntity>> GetTopAsync(int topN);
    Task<bool> InsertAsync(StoryEntity story);
    Task<bool> InsertAsync(List<StoryEntity> stories);
    Task DeleteAllAsync();
}

internal class StoriesRepository : IStoriesRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private const string TABLE_NAME = "STORIES";

    public StoriesRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<StoryEntity>> GetTopAsync(int topN)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<StoryEntity>($"SELECT * FROM {TABLE_NAME} ORDER BY Score DESC LIMIT @topN", new { topN });
    }

    public async Task<bool> InsertAsync(StoryEntity story)
    {
        return await InsertAsync(new List<StoryEntity> { story });
    }

    public async Task<bool> InsertAsync(List<StoryEntity> stories)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @$"INSERT INTO {TABLE_NAME} (Id, Title, Uri, PostedBy, Time, Score, CommentCount)
               VALUES (@Id, @Title, @Uri, @PostedBy, @Time, @Score, @CommentCount)",
            stories);
        return result > 0;
    }

    public async Task DeleteAllAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync($"DELETE FROM {TABLE_NAME}");
    }
}
