namespace LocalVibe.API.DTOs.Food;

public class RelatedMeal
{
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Area { get; set; }              // Cuisine origin (e.g. "Vietnamese")
    public string? ThumbnailUrl { get; set; }
    public string? Instructions { get; set; }
    public string? YoutubeUrl { get; set; }
    public string? SourceUrl { get; set; }
}

public class FoodInfoResponse
{
    public int PostId { get; set; }
    public string PostTitle { get; set; } = string.Empty;
    public IEnumerable<RelatedMeal> RelatedMeals { get; set; } = Enumerable.Empty<RelatedMeal>();
    public string Provider { get; set; } = "TheMealDB";
}
