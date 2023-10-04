namespace HackerNewsClient.Api.Config;

public class CacheOptions
{
    public const string SECTION = "Cache";

    public bool Enabled { get; set; } = true;
    public TimeSpan AbsoluteExpirationRelativeToNow { get; set; }
}
