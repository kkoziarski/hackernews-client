using HackerNewsClient.Api.Contracts;
using HackerNewsClient.Api.Database;
using HackerNewsClient.Api.Domain;

namespace HackerNewsClient.Api.Mapping;

internal static class StoryMapper
{
    public static Story ToDomain(this StoryResponse response)
    {
        return new Story
        {
            Title = response.Title,
            PostedBy = response.By,
            Uri = response.Url,
            Time = FromEpoch(response.Time),
            Score = response.Score,
            CommentCount = response.Descendants
        };
    }

    public static StoryEntity ToDbEntity(this StoryResponse response)
    {
        return new StoryEntity
        {
            Id = response.Id,
            Title = response.Title,
            PostedBy = response.By,
            Uri = response.Url,
            Time = FromEpoch(response.Time),
            Score = response.Score,
            CommentCount = response.Descendants
        };
    }

    public static Story ToDomain(this StoryEntity response)
    {
        return new Story
        {
            Title = response.Title,
            PostedBy = response.PostedBy,
            Uri = response.Uri,
            Time = response.Time,
            Score = response.Score,
            CommentCount = response.CommentCount
        };
    }

    private static DateTime FromEpoch(long epoch) => DateTimeOffset.FromUnixTimeSeconds(epoch).DateTime.ToUniversalTime();
}

