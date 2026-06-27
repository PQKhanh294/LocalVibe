using LocalVibe.API.DTOs.Comments;

namespace LocalVibe.API.Services.Interfaces;

public interface ICommentService
{
    Task<CommentResponse> AddCommentAsync(int postId, CreateCommentRequest request);
}
