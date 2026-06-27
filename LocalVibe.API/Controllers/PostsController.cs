using LocalVibe.API.DTOs.Posts;
using LocalVibe.API.Entities.Enums;
using LocalVibe.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

namespace LocalVibe.API.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IMemoryCache _cache;

    public PostsController(IPostService postService, IMemoryCache cache)
    {
        _postService = postService;
        _cache = cache;
    }

    /// <summary>GET /api/posts — Danh sách bài viết với filter, sort, paging</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PostTag? tag      = null,
        [FromQuery] string?  location = null,
        [FromQuery] string   sort     = "newest",
        [FromQuery] int      page     = 1,
        [FromQuery] int      pageSize = 10)
    {
        if (page < 1)     page     = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50;

        var cacheKey = $"Posts_{tag}_{location}_{sort}_{page}_{pageSize}";
        if (_cache.TryGetValue(cacheKey, out object? cachedResult))
        {
            return Ok(cachedResult);
        }

        var result = await _postService.GetPagedAsync(tag, location, sort, page, pageSize);
        
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(2)); // Cache for 2 minutes
        _cache.Set(cacheKey, result, cacheEntryOptions);

        return Ok(result);
    }

    /// <summary>GET /api/posts/{id} — Chi tiết bài viết kèm comments</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _postService.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>POST /api/posts — Tạo bài viết mới (multipart/form-data)</summary>
    [HttpPost]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreatePostRequest request)
    {
        var result = await _postService.CreateAsync(request, request.Image);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>DELETE /api/posts/{id} — Xóa bài viết</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")] // Only Admin can delete
    public async Task<IActionResult> Delete(int id)
    {
        await _postService.DeleteAsync(id);
        return NoContent();
    }
}
