using LocalVibe.API.DTOs.Common;
using LocalVibe.API.DTOs.Posts;
using LocalVibe.API.Entities.Enums;

namespace LocalVibe.API.Services.Interfaces;

public interface IPostService
{
    Task<PagedResponse<PostSummaryResponse>> GetPagedAsync(PostTag? tag, string? location, string sort, int page, int pageSize);
    Task<PostDetailResponse> GetByIdAsync(int id);
    Task<PostDetailResponse> CreateAsync(CreatePostRequest request, IFormFile? image);
    Task DeleteAsync(int id);
}
