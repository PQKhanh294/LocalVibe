using LocalVibe.API.DTOs.Comments;
using LocalVibe.API.Entities;
using LocalVibe.API.Exceptions;
using LocalVibe.API.Repositories.Interfaces;
using LocalVibe.API.Services.Interfaces;

namespace LocalVibe.API.Services.Implementations;

public class CommentService : ICommentService
{
    private readonly IGenericRepository<Comment> _commentRepo;
    private readonly IGenericRepository<Post>    _postRepo;

    public CommentService(
        IGenericRepository<Comment> commentRepo,
        IGenericRepository<Post>    postRepo)
    {
        _commentRepo = commentRepo;
        _postRepo    = postRepo;
    }

    public async Task<CommentResponse> AddCommentAsync(int postId, CreateCommentRequest request)
    {
        // Verify post exists
        var post = await _postRepo.GetByIdAsync(postId)
            ?? throw new NotFoundException("Post", postId);

        var comment = new Comment
        {
            PostId     = postId,
            AuthorName = request.AuthorName,
            Content    = request.Content,
            CreatedAt  = DateTime.UtcNow
        };

        await _commentRepo.AddAsync(comment);
        await _commentRepo.SaveChangesAsync();

        return new CommentResponse
        {
            Id         = comment.Id,
            AuthorName = comment.AuthorName,
            Content    = comment.Content,
            CreatedAt  = comment.CreatedAt
        };
    }
}
