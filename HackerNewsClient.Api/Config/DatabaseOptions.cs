namespace HackerNewsClient.Api.Config;

public class DatabaseOptions
{
    public const string SECTION = "Database";

    public string ConnectionString { get; init; } = string.Empty;
}
