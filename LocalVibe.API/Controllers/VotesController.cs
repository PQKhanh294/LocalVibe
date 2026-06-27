using LocalVibe.API.DTOs.Votes;
using LocalVibe.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalVibe.API.Controllers;

[ApiController]
[Route("api/posts/{postId:int}/vote")]
public class VotesController : ControllerBase
{
    private readonly IVoteService _voteService;

    public VotesController(IVoteService voteService)
    {
        _voteService = voteService;
    }

    /// <summary>POST /api/posts/{postId}/vote — Gửi vote Up/Down</summary>
    [HttpPost]
    public async Task<IActionResult> Vote(int postId, [FromBody] VoteRequest request)
    {
        var result = await _voteService.VoteAsync(postId, request);
        return Ok(result);
    }
}
