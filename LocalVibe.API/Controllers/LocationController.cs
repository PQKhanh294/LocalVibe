using LocalVibe.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalVibe.API.Controllers;

[ApiController]
[Route("api/location")]
public class LocationController : ControllerBase
{
    private readonly IIpLocationService _ipLocation;

    public LocationController(IIpLocationService ipLocation)
    {
        _ipLocation = ipLocation;
    }

    /// <summary>
    /// GET /api/location/detect — Phát hiện vị trí người dùng theo IP.
    /// Frontend dùng để ưu tiên hiển thị bài viết gần vị trí user nhất.
    /// </summary>
    [HttpGet("detect")]
    public async Task<IActionResult> Detect()
    {
        // Lấy IP thực của client (hỗ trợ cả proxy/reverse proxy)
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString()
              ?? HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
              ?? "unknown";

        var result = await _ipLocation.DetectAsync(ip);

        if (result is null)
            return Ok(new
            {
                message = "Đang chạy local hoặc không xác định được vị trí.",
                ip
            });

        return Ok(result);
    }
}
