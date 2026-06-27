using LocalVibe.API.Repositories.Interfaces;
using LocalVibe.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LocalVibe.API.Controllers;

[ApiController]
[Route("api")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly IPostRepository _postRepo;
    private readonly IMemoryCache _cache;

    public WeatherController(IWeatherService weatherService, IPostRepository postRepo, IMemoryCache cache)
    {
        _weatherService = weatherService;
        _postRepo       = postRepo;
        _cache          = cache;
    }

    /// <summary>GET /api/weather?lat=&amp;lng= — Thời tiết tại toạ độ bất kỳ</summary>
    [HttpGet("weather")]
    public async Task<IActionResult> GetByCoords(
        [FromQuery] double lat,
        [FromQuery] double lng)
    {
        var cacheKey = $"Weather_{Math.Round(lat, 2)}_{Math.Round(lng, 2)}";
        if (_cache.TryGetValue(cacheKey, out object? cachedResult))
        {
            return Ok(cachedResult);
        }

        var result = await _weatherService.GetWeatherAsync(lat, lng);
        
        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

        return Ok(result);
    }

    /// <summary>GET /api/posts/{id}/weather — Thời tiết tại địa điểm của bài viết</summary>
    [HttpGet("posts/{id:int}/weather")]
    public async Task<IActionResult> GetByPost(int id)
    {
        var cacheKey = $"Weather_Post_{id}";
        if (_cache.TryGetValue(cacheKey, out object? cachedResult))
        {
            return Ok(cachedResult);
        }

        var post = await _postRepo.GetByIdAsync(id);
        if (post is null) return NotFound(new { message = $"Post #{id} không tìm thấy." });

        var result = await _weatherService.GetWeatherAsync(post.Latitude, post.Longitude);
        
        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

        return Ok(result);
    }
}
