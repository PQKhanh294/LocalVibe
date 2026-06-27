using LocalVibe.API.DTOs.Translation;
using LocalVibe.API.Exceptions;
using LocalVibe.API.Repositories.Interfaces;
using LocalVibe.API.Services.Interfaces;
using System.Text.Json;
using System.Web;

namespace LocalVibe.API.Services.Implementations;

/// <summary>
/// Dịch văn bản dùng MyMemory (https://mymemory.translated.net).
/// Hoàn toàn miễn phí, không cần API key, 5000 words/ngày.
/// </summary>
public class TranslationService : ITranslationService
{
    private readonly HttpClient _http;
    private readonly IPostRepository _postRepo;

    public TranslationService(IHttpClientFactory factory, IPostRepository postRepo)
    {
        _http     = factory.CreateClient("MyMemory");
        _postRepo = postRepo;
    }

    public async Task<TranslationResponse> TranslatePostAsync(int postId, string targetLanguage)
    {
        var post = await _postRepo.GetByIdAsync(postId)
            ?? throw new NotFoundException("Post", postId);

        var translatedTitle       = await TranslateTextAsync(post.Title,       "vi", targetLanguage);
        var translatedDescription = await TranslateTextAsync(post.Description, "vi", targetLanguage);

        return new TranslationResponse
        {
            OriginalTitle         = post.Title,
            TranslatedTitle       = translatedTitle,
            OriginalDescription   = post.Description,
            TranslatedDescription = translatedDescription,
            SourceLanguage        = "vi",
            TargetLanguage        = targetLanguage
        };
    }

    private async Task<string> TranslateTextAsync(string text, string from, string to)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        var encoded = HttpUtility.UrlEncode(text);
        var url = $"get?q={encoded}&langpair={from}|{to}";

        try
        {
            var json = await _http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);

            return doc.RootElement
                      .GetProperty("responseData")
                      .GetProperty("translatedText")
                      .GetString() ?? text;
        }
        catch
        {
            return text; // Fallback: trả về text gốc nếu dịch thất bại
        }
    }
}
