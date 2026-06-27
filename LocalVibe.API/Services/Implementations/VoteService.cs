using LocalVibe.API.Data;
using LocalVibe.API.DTOs.Votes;
using LocalVibe.API.Entities;
using LocalVibe.API.Entities.Enums;
using LocalVibe.API.Exceptions;
using LocalVibe.API.Repositories.Interfaces;
using LocalVibe.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LocalVibe.API.Services.Implementations;

public class VoteService : IVoteService
{
    private readonly IGenericRepository<Vote> _voteRepo;
    private readonly IGenericRepository<Post> _postRepo;
    private readonly AppDbContext _context;

    public VoteService(
        IGenericRepository<Vote> voteRepo,
        IGenericRepository<Post> postRepo,
        AppDbContext context)
    {
        _voteRepo = voteRepo;
        _postRepo = postRepo;
        _context  = context;
    }

    public async Task<VoteResultResponse> VoteAsync(int postId, VoteRequest request)
    {
        // Verify post exists
        var post = await _postRepo.GetByIdAsync(postId)
            ?? throw new NotFoundException("Post", postId);

        // Check duplicate vote
        var existing = await _context.Votes
            .FirstOrDefaultAsync(v => v.PostId == postId && v.VoterToken == request.VoterToken);

        if (existing is not null)
        {
            if (existing.VoteType == request.VoteType)
                throw new ConflictException($"Token '{request.VoterToken}' đã vote cho bài viết này rồi.");

            // Change vote
            if (existing.VoteType == VoteType.Up)
            {
                post.UpvotesCount--;
                post.DownvotesCount++;
            }
            else
            {
                post.DownvotesCount--;
                post.UpvotesCount++;
            }
            existing.VoteType = request.VoteType;
        }
        else
        {
            // New vote
            var vote = new Vote
            {
                PostId     = postId,
                VoteType   = request.VoteType,
                VoterToken = request.VoterToken
            };
            await _voteRepo.AddAsync(vote);

            if (request.VoteType == VoteType.Up) post.UpvotesCount++;
            else post.DownvotesCount++;
        }

        post.Score = post.UpvotesCount - post.DownvotesCount;
        
        await _voteRepo.SaveChangesAsync();

        return new VoteResultResponse
        {
            UpVotes   = post.UpvotesCount,
            DownVotes = post.DownvotesCount
        };
    }
}
