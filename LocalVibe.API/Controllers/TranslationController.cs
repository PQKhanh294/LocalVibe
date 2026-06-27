using LocalVibe.API.DTOs.Translation;
using LocalVibe.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalVibe.API.Controllers;

[ApiController]
[Route("api/posts/{postId:int}/translate")]
public class TranslationController : ControllerBase
{
    private readonly ITranslationService _translation;

    public TranslationController(ITranslationService translation)
    {
        _translation = translation;
    }

    /// <summary>
    /// POST /api/posts/{postId}/translate — Dịch title + description của bài viết.
    /// Body: { "targetLanguage": "en" }  (en, fr, ja, ko, zh...)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Translate(int postId, [FromBody] TranslationRequest request)
    {
        var result = await _translation.TranslatePostAsync(postId, request.TargetLanguage);
        return Ok(result);
    }
}
