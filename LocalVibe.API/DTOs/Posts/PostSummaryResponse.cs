using LocalVibe.API.Entities.Enums;

namespace LocalVibe.API.DTOs.Posts;

public class PostSummaryResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UpVotes { get; set; }
    public int DownVotes { get; set; }
    public int NetScore => UpVotes - DownVotes;
    public int CommentCount { get; set; }
}
