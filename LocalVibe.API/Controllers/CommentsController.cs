using LocalVibe.API.DTOs.Comments;
using LocalVibe.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalVibe.API.Controllers;

[ApiController]
[Route("api/posts/{postId:int}/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    /// <summary>POST /api/posts/{postId}/comments — Thêm bình luận vào bài viết</summary>
    [HttpPost]
    public async Task<IActionResult> AddComment(int postId, [FromBody] CreateCommentRequest request)
    {
        var result = await _commentService.AddCommentAsync(postId, request);
        return StatusCode(201, result);
    }
}
