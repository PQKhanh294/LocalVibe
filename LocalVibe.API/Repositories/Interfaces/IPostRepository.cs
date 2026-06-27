using LocalVibe.API.Entities;
using LocalVibe.API.Entities.Enums;

namespace LocalVibe.API.Repositories.Interfaces;

public interface IPostRepository : IGenericRepository<Post>
{
    /// <summary>
    /// Lấy danh sách post có filter, sort, paging.
    /// </summary>
    /// <returns>(items, totalCount)</returns>
    Task<(IEnumerable<Post> Items, int TotalCount)> GetPagedAsync(
        PostTag? tag,
        string? location,
        string sort,
        int page,
        int pageSize);

    /// <summary>
    /// Lấy post kèm Comments và Votes.
    /// </summary>
    Task<Post?> GetWithDetailsAsync(int id);
}
