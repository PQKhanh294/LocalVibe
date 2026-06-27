using LocalVibe.API.Entities.Enums;

namespace LocalVibe.API.Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Address { get; set; }
    public string? ImageUrl { get; set; }
    public List<string> AdditionalImages { get; set; } = new();
    public PostTag Tag { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Denormalization: Bộ đếm điểm để tối ưu câu query sort
    public int UpvotesCount { get; set; } = 0;
    public int DownvotesCount { get; set; } = 0;
    public int Score { get; set; } = 0; // Score = Upvotes - Downvotes

    // Navigation properties
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
