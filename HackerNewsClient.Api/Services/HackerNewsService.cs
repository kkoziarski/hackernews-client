using HackerNewsClient.Api.Config;
using HackerNewsClient.Api.Contracts;
using HackerNewsClient.Api.Database;
using HackerNewsClient.Api.Domain;
using HackerNewsClient.Api.Mapping;
using HackerNewsClient.Api.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace HackerNewsClient.Api.Services;

public interface IStoryService
{
    Task<IEnumerable<Story>> GetBestStoriesAsync(int count);
}

public class HackerNewsService : IStoryService
{
    private readonly IHackerNewsApiClient _apiClient;
    private readonly IOptions<CacheOptions> _cacheOptions;
    private readonly IMemoryCache _cache;
    private readonly IStoriesRepository _storiesRepository;

    public HackerNewsService(
        IHackerNewsApiClient apiClient,
        IOptions<CacheOptions> cacheOptions,
        IMemoryCache cache,
        IStoriesRepository storiesRepository)
    {
        _apiClient = apiClient;
        _cacheOptions = cacheOptions;
        _cache = cache;
        _storiesRepository = storiesRepository;
    }

    public async Task<IEnumerable<Story>> GetBestStoriesAsync(int count)
    {
        if (!CanReadFromDb())
        {
            var dbStories = await FetchStoriesAsync();

            await _storiesRepository.DeleteAllAsync();
            await _storiesRepository.InsertAsync(dbStories);
            _cache.Set(CreateKey(), true, ConfigureCacheExpiration());
        }

        var topStories = await _storiesRepository.GetTopAsync(count);
        var result = topStories.Select(x => x.ToDomain());
        return result;
    }

    private async Task<List<StoryEntity>> FetchStoriesAsync()
    {
        var bestStoryIds = await _apiClient.GetBestStoriesAsync();
        var tasks = bestStoryIds.Select(id => _apiClient.GetStoryAsync(id));
        var storiesResponse = await Task.WhenAll(tasks);
        var dbStories = storiesResponse
               .Where(s => s is not null)
               .Select(s => s!.ToDbEntity())
               .ToList();

        return dbStories;
    }

    private bool CanReadFromDb() => _cache.TryGetValue<bool>(CreateKey(), out var _);

    private MemoryCacheEntryOptions ConfigureCacheExpiration()
        => new()
        {
            AbsoluteExpirationRelativeToNow = _cacheOptions.Value.AbsoluteExpirationRelativeToNow
        };

    private static string CreateKey() => $"{CacheKeys.BestStoriesDB}";
}
