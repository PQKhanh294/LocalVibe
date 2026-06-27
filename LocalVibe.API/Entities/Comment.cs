namespace LocalVibe.API.Entities;

public class Comment
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Post Post { get; set; } = null!;
}
