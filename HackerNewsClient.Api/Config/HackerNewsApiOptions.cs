namespace HackerNewsClient.Api.Config;

public class HackerNewsApiOptions
{
    public const string SECTION = "HackerNewsAPI";

    public string BestStoriesUrl { get; init; } = string.Empty;
    public string StoryDetailsUrlFormat { get; init; } = string.Empty;
}
