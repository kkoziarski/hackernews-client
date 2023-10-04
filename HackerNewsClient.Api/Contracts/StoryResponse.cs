namespace HackerNewsClient.Api.Contracts;

public record StoryResponse
{
    public int Id { get; init; }
    public string By { get; init; } = string.Empty;
    public int Descendants { get; init; }
    public int Score { get; init; }
    public long Time { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;

    //public string Type { get; init; } = string.Empty; //ignored becaus not needed

    //public int[] Kids { get; init; } = Array.Empty<int>(); //ignored becaus not needed
}

