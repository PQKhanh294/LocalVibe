using LocalVibe.API.DTOs.Votes;

namespace LocalVibe.API.Services.Interfaces;

public interface IVoteService
{
    Task<VoteResultResponse> VoteAsync(int postId, VoteRequest request);
}
