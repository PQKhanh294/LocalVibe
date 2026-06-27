using LocalVibe.API.DTOs.Food;

namespace LocalVibe.API.Services.Interfaces;

public interface IFoodInfoService
{
    /// <summary>
    /// Lấy thông tin món ăn liên quan từ TheMealDB cho Post có tag Food.
    /// </summary>
    Task<FoodInfoResponse> GetRelatedMealsAsync(int postId);
}
