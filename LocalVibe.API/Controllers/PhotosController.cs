using LocalVibe.API.Entities.Enums;
using LocalVibe.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalVibe.API.Controllers;

[ApiController]
[Route("api/photos")]
public class PhotosController : ControllerBase
{
    private readonly IPhotoSuggestionService _photos;

    public PhotosController(IPhotoSuggestionService photos)
    {
        _photos = photos;
    }

    /// <summary>
    /// GET /api/photos/suggest?tag=Food&amp;query=bún+bò — Gợi ý ảnh từ Pixabay.
    /// Dùng khi Post chưa có ImageUrl để hiển thị ảnh placeholder đẹp.
    /// </summary>
    [HttpGet("suggest")]
    public async Task<IActionResult> Suggest(
        [FromQuery] PostTag? tag   = null,
        [FromQuery] string?  query = null,
        [FromQuery] int      count = 5)
    {
        // Xây dựng từ khóa tìm kiếm từ tag hoặc query trực tiếp
        var searchQuery = query ?? tag switch
        {
            PostTag.Food        => "Vietnamese food street",
            PostTag.Coffee      => "coffee shop Vietnam",
            PostTag.ScenicRoute => "Vietnam scenic road mountain",
            _                   => "Vietnam travel"
        };

        var result = await _photos.SuggestAsync(searchQuery, Math.Clamp(count, 1, 20));
        return Ok(result);
    }
}
