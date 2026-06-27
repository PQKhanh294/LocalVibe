using LocalVibe.API.DTOs.Comments;

namespace LocalVibe.API.DTOs.Posts;

public class PostDetailResponse : PostSummaryResponse
{
    public string Description { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Address { get; set; }
    public List<string> AdditionalImages { get; set; } = new();
    public IEnumerable<CommentResponse> Comments { get; set; } = Enumerable.Empty<CommentResponse>();
}
