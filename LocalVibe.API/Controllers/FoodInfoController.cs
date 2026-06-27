using LocalVibe.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalVibe.API.Controllers;

[ApiController]
[Route("api/posts/{postId:int}/food-info")]
public class FoodInfoController : ControllerBase
{
    private readonly IFoodInfoService _foodInfo;

    public FoodInfoController(IFoodInfoService foodInfo)
    {
        _foodInfo = foodInfo;
    }

    /// <summary>
    /// GET /api/posts/{postId}/food-info — Tìm món ăn liên quan từ TheMealDB.
    /// Chỉ gọi endpoint này nếu Post có Tag là "Food".
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetRelatedMeals(int postId)
    {
        try
        {
            var result = await _foodInfo.GetRelatedMealsAsync(postId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
