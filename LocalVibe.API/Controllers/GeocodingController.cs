using LocalVibe.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalVibe.API.Controllers;

[ApiController]
[Route("api/geocoding")]
public class GeocodingController : ControllerBase
{
    private readonly IGeocodingService _geocoding;

    public GeocodingController(IGeocodingService geocoding)
    {
        _geocoding = geocoding;
    }

    /// <summary>
    /// GET /api/geocoding/search?q=Bún+bò+Huế — Tìm toạ độ theo tên địa điểm.
    /// Frontend dùng để auto-fill lat/lng khi tạo Post.
    /// </summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int limit = 5)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { message = "Vui lòng nhập từ khoá tìm kiếm." });

        var results = await _geocoding.SearchAsync(q, Math.Clamp(limit, 1, 10));
        return Ok(results);
    }

    /// <summary>
    /// GET /api/geocoding/reverse?lat=16.46&amp;lng=107.59 — Địa chỉ từ toạ độ.
    /// Dùng để hiển thị tên địa chỉ đầy đủ thay vì chỉ số lat/lng.
    /// </summary>
    [HttpGet("reverse")]
    public async Task<IActionResult> Reverse([FromQuery] double lat, [FromQuery] double lng)
    {
        var result = await _geocoding.ReverseAsync(lat, lng);
        if (result is null)
            return NotFound(new { message = "Không tìm thấy địa chỉ cho toạ độ này." });

        return Ok(result);
    }
}
