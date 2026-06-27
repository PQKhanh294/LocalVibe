using LocalVibe.API.Data;
using LocalVibe.API.Entities;
using LocalVibe.API.Entities.Enums;
using LocalVibe.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocalVibe.API.Repositories.Implementations;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(AppDbContext context) : base(context) { }

    public async Task<(IEnumerable<Post> Items, int TotalCount)> GetPagedAsync(
        PostTag? tag,
        string? location,
        string sort,
        int page,
        int pageSize)
    {
        var query = _context.Posts
            .AsNoTracking()
            .Include(p => p.Votes)
            .AsQueryable();

        // ── Filter ────────────────────────────────────────────────────────────
        if (tag.HasValue)
            query = query.Where(p => p.Tag == tag.Value);

        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(p => p.Address.Contains(location));
        }

        // ── Sort ──────────────────────────────────────────────────────────────
        query = sort.ToLower() switch
        {
            "top" => query.OrderByDescending(p => p.Score),
            _ => query.OrderByDescending(p => p.CreatedAt)  // default: newest
        };

        // ── Paging ────────────────────────────────────────────────────────────
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Post?> GetWithDetailsAsync(int id)
        => await _context.Posts
            .AsNoTracking()
            .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt))
            .Include(p => p.Votes)
            .FirstOrDefaultAsync(p => p.Id == id);
}
