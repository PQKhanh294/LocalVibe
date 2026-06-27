using LocalVibe.API.DTOs.Food;
using LocalVibe.API.Entities.Enums;
using LocalVibe.API.Exceptions;
using LocalVibe.API.Repositories.Interfaces;
using LocalVibe.API.Services.Interfaces;
using System.Text.Json;
using System.Web;

namespace LocalVibe.API.Services.Implementations;

/// <summary>
/// Lấy thông tin món ăn liên quan từ TheMealDB (https://www.themealdb.com/api.php).
/// API key "1" là free test key.
/// Chỉ áp dụng cho Post có Tag = Food.
/// </summary>
public class FoodInfoService : IFoodInfoService
{
    private readonly HttpClient _http;
    private readonly IPostRepository _postRepo;

    public FoodInfoService(IHttpClientFactory factory, IPostRepository postRepo)
    {
        _http     = factory.CreateClient("TheMealDB");
        _postRepo = postRepo;
    }

    public async Task<FoodInfoResponse> GetRelatedMealsAsync(int postId)
    {
        var post = await _postRepo.GetByIdAsync(postId)
            ?? throw new NotFoundException("Post", postId);

        if (post.Tag != PostTag.Food)
            throw new ArgumentException($"Post #{postId} không phải tag Food — không có thông tin món ăn.");

        // Tách từ khóa đầu tiên từ title để tìm kiếm
        var keyword = ExtractSearchKeyword(post.Title);
        var encoded = HttpUtility.UrlEncode(keyword);
        var url     = $"search.php?s={encoded}";

        try
        {
            var json = await _http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);

            var meals = new List<RelatedMeal>();
            if (doc.RootElement.TryGetProperty("meals", out var mealsEl) &&
                mealsEl.ValueKind == JsonValueKind.Array)
            {
                foreach (var m in mealsEl.EnumerateArray().Take(3)) // max 3 kết quả
                {
                    meals.Add(new RelatedMeal
                    {
                        Name         = m.TryGetProperty("strMeal",         out var name)  ? name.GetString()  ?? "" : "",
                        Category     = m.TryGetProperty("strCategory",     out var cat)   ? cat.GetString()         : null,
                        Area         = m.TryGetProperty("strArea",         out var area)  ? area.GetString()        : null,
                        ThumbnailUrl = m.TryGetProperty("strMealThumb",    out var thumb) ? thumb.GetString()       : null,
                        Instructions = m.TryGetProperty("strInstructions", out var inst)  ? TruncateInstructions(inst.GetString()) : null,
                        YoutubeUrl   = m.TryGetProperty("strYoutube",      out var yt)    ? yt.GetString()          : null,
                        SourceUrl    = m.TryGetProperty("strSource",       out var src)   ? src.GetString()         : null
                    });
                }
            }

            return new FoodInfoResponse
            {
                PostId       = postId,
                PostTitle    = post.Title,
                RelatedMeals = meals
            };
        }
        catch
        {
            return new FoodInfoResponse { PostId = postId, PostTitle = post.Title };
        }
    }

    /// <summary>Lấy 1-2 từ đầu tiên của title làm keyword tìm kiếm.</summary>
    private static string ExtractSearchKeyword(string title)
    {
        // Bỏ qua các từ mô tả thường gặp, lấy từ khóa chính
        var words = title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words.Length >= 2
            ? string.Join(" ", words.Take(2))
            : title;
    }

    private static string? TruncateInstructions(string? text) =>
        text?.Length > 300 ? text[..300] + "..." : text;
}
