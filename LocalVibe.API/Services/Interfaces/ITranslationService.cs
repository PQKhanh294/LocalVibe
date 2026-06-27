using LocalVibe.API.DTOs.Translation;

namespace LocalVibe.API.Services.Interfaces;

public interface ITranslationService
{
    /// <summary>
    /// Dịch title + description của một Post sang ngôn ngữ đích (MyMemory).
    /// </summary>
    Task<TranslationResponse> TranslatePostAsync(int postId, string targetLanguage);
}
