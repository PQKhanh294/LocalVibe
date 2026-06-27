namespace LocalVibe.API.DTOs.Comments;

public class CreateCommentRequest
{
    public string AuthorName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
