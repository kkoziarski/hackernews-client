using HackerNewsClient.Api.Domain;
using HackerNewsClient.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsClient.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BestStoriesController : ControllerBase
{
    private readonly ILogger<BestStoriesController> _logger;
    private readonly IStoryService _storyService;

    public BestStoriesController(ILogger<BestStoriesController> logger, IStoryService storyService)
    {
        _logger = logger;
        _storyService = storyService;
    }

    [HttpGet("{n}")]
    public async Task<IEnumerable<Story>> GetAsync(int n) => await _storyService.GetBestStoriesAsync(n);
}
