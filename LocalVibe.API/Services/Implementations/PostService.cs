using LocalVibe.API.DTOs.Comments;
using LocalVibe.API.DTOs.Common;
using LocalVibe.API.DTOs.Posts;
using LocalVibe.API.Entities;
using LocalVibe.API.Entities.Enums;
using LocalVibe.API.Exceptions;
using LocalVibe.API.Repositories.Interfaces;
using LocalVibe.API.Services.Interfaces;

namespace LocalVibe.API.Services.Implementations;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepo;
    private readonly IFileUploadService _fileUpload;

    public PostService(IPostRepository postRepo, IFileUploadService fileUpload)
    {
        _postRepo   = postRepo;
        _fileUpload = fileUpload;
    }

    public async Task<PagedResponse<PostSummaryResponse>> GetPagedAsync(
        PostTag? tag, string? location, string sort, int page, int pageSize)
    {
        var (items, totalCount) = await _postRepo.GetPagedAsync(tag, location, sort, page, pageSize);
        var mapped = items.Select(MapToSummary);
        return PagedResponse<PostSummaryResponse>.Create(mapped, page, pageSize, totalCount);
    }

    public async Task<PostDetailResponse> GetByIdAsync(int id)
    {
        var post = await _postRepo.GetWithDetailsAsync(id)
            ?? throw new NotFoundException("Post", id);

        return MapToDetail(post);
    }

    public async Task<PostDetailResponse> CreateAsync(CreatePostRequest request, IFormFile? image)
    {
        string? imageUrl = null;
        if (image is not null)
            imageUrl = await _fileUpload.SaveAsync(image);

        var post = new Post
        {
            Title       = request.Title,
            Description = request.Description ?? string.Empty,
            Latitude    = request.Latitude,
            Longitude   = request.Longitude,
            Tag         = request.Tag,
            ImageUrl    = imageUrl,
            CreatedAt   = DateTime.UtcNow
        };

        await _postRepo.AddAsync(post);
        await _postRepo.SaveChangesAsync();

        // Reload with navigation props
        var created = await _postRepo.GetWithDetailsAsync(post.Id)
            ?? throw new InvalidOperationException("Không thể tải post sau khi tạo.");

        return MapToDetail(created);
    }

    public async Task DeleteAsync(int id)
    {
        var post = await _postRepo.GetByIdAsync(id)
            ?? throw new NotFoundException("Post", id);

        _postRepo.Delete(post);
        await _postRepo.SaveChangesAsync();
    }

    // ── Mappers ───────────────────────────────────────────────────────────────

    private static PostSummaryResponse MapToSummary(Post post) => new()
    {
        Id           = post.Id,
        Title        = post.Title,
        Tag          = post.Tag.ToString(),
        ImageUrl     = post.ImageUrl,
        CreatedAt    = post.CreatedAt,
        UpVotes      = post.Votes.Count(v => v.VoteType == VoteType.Up),
        DownVotes    = post.Votes.Count(v => v.VoteType == VoteType.Down),
        CommentCount = post.Comments.Count
    };

    private static PostDetailResponse MapToDetail(Post post) => new()
    {
        Id           = post.Id,
        Title        = post.Title,
        Description  = post.Description,
        Tag          = post.Tag.ToString(),
        ImageUrl     = post.ImageUrl,
        AdditionalImages = post.AdditionalImages,
        Latitude     = post.Latitude,
        Longitude    = post.Longitude,
        Address      = post.Address,
        CreatedAt    = post.CreatedAt,
        UpVotes      = post.Votes.Count(v => v.VoteType == VoteType.Up),
        DownVotes    = post.Votes.Count(v => v.VoteType == VoteType.Down),
        CommentCount = post.Comments.Count,
        Comments     = post.Comments.Select(c => new CommentResponse
        {
            Id         = c.Id,
            AuthorName = c.AuthorName,
            Content    = c.Content,
            CreatedAt  = c.CreatedAt
        })
    };
}
