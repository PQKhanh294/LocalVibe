using LocalVibe.API.Entities.Enums;
using Microsoft.AspNetCore.Http;

namespace LocalVibe.API.DTOs.Posts;

public class CreatePostRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public PostTag Tag { get; set; }
    public IFormFile? Image { get; set; }
}
