using LocalVibe.API.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LocalVibe.API.Controllers;

[ApiController]
[Route("api/tags")]
public class TagsController : ControllerBase
{
    /// <summary>GET /api/tags — Danh sách tag cố định</summary>
    [HttpGet]
    public IActionResult GetTags()
    {
        var tags = Enum.GetValues<PostTag>()
            .Select(t => new
            {
                value = t.ToString(),
                label = t switch
                {
                    PostTag.Food        => "Đồ ăn",
                    PostTag.ScenicRoute => "Cung đường đẹp",
                    PostTag.Coffee      => "Cà phê",
                    PostTag.Hotel       => "Nơi lưu trú",
                    PostTag.Attraction  => "Điểm tham quan",
                    _                  => t.ToString()
                }
            });

        return Ok(tags);
    }
}
