namespace HackerNewsClient.Api.Domain;

public record Story
{
    public string Title { get; init; } = string.Empty;
    public string Uri { get; init; } = string.Empty;
    public string PostedBy { get; init; } = string.Empty;
    public DateTime Time { get; init; }
    public int Score { get; init; }
    public int CommentCount { get; init; }
}
