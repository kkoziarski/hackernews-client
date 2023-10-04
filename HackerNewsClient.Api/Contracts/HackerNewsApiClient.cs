using System.Net;
using HackerNewsClient.Api.Config;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace HackerNewsClient.Api.Contracts;

public interface IHackerNewsApiClient
{
    Task<long[]> GetBestStoriesAsync();
    Task<StoryResponse?> GetStoryAsync(long id);
}

internal class HackerNewsApiClient : IHackerNewsApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<HackerNewsApiOptions> _options;
    private readonly IOptions<CacheOptions> _cacheOptions;
    private readonly IMemoryCache _cache;

    public HackerNewsApiClient(IHttpClientFactory httpClientFactory,
        IOptions<HackerNewsApiOptions> options,
        IOptions<CacheOptions> cacheOptions,
        IMemoryCache cache)
    {
        _httpClientFactory = httpClientFactory;
        _options = options;
        _cacheOptions = cacheOptions;
        _cache = cache;
    }

    public async Task<long[]> GetBestStoriesAsync()
    {
        var client = _httpClientFactory.CreateClient(Consts.HACKER_NEWS_HTTP_CLIENT);
        var url = _options.Value.BestStoriesUrl;

        var response = await client.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return Array.Empty<long>();
        }

        var ids = await response.Content.ReadFromJsonAsync<long[]>();
        return ids ?? Array.Empty<long>();
    }

    public async Task<StoryResponse?> GetStoryAsync(long id)
    {
        if (!_cacheOptions.Value.Enabled)
        {
            return await FetchAsync(id);
        }

        var cacheKey = CreateKey(id);

        var story = await _cache.GetOrCreateAsync(
            CreateKey(id),
            async cacheEntry =>
            {
                ConfigureCacheExpiration(cacheEntry);
                return await FetchAsync(id);
            });
        return story;
    }

    private async Task<StoryResponse?> FetchAsync(long id)
    {
        var client = _httpClientFactory.CreateClient(Consts.HACKER_NEWS_HTTP_CLIENT);
        var url = string.Format(_options.Value.StoryDetailsUrlFormat, id);

        var response = await client.GetAsync(url);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        var story = await response.Content.ReadFromJsonAsync<StoryResponse?>();
        return story;
    }

    private void ConfigureCacheExpiration(ICacheEntry cacheEntry)
    {
        //cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(1);
        cacheEntry.AbsoluteExpirationRelativeToNow = _cacheOptions.Value.AbsoluteExpirationRelativeToNow;
    }
    private static string CreateKey(long id) => $"{CacheKeys.Story}_{id}";
}
