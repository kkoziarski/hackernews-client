using HackerNewsClient.Api.Contracts;
using HackerNewsClient.Api.Database;
using HackerNewsClient.Api.Repositories;
using HackerNewsClient.Api.Services;

namespace HackerNewsClient.Api.Config;

public static class DependencyInjection
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CacheOptions>(configuration.GetSection(CacheOptions.SECTION));

        ConfigureDatabase(services, configuration);
        ConfigureExternalApi(services, configuration);
    }

    private static void ConfigureExternalApi(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient(Consts.HACKER_NEWS_HTTP_CLIENT);
        services.AddSingleton<IHackerNewsApiClient, HackerNewsApiClient>();
        services.AddScoped<IStoryService, HackerNewsService>();
        services.Configure<HackerNewsApiOptions>(configuration.GetSection(HackerNewsApiOptions.SECTION));
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SECTION));
        services.AddSingleton<IDbConnectionFactory, SqliteConnectionFactory>();
        services.AddSingleton<DatabaseInitializer>();
        services.AddScoped<IStoriesRepository, StoriesRepository>();
    }
}
