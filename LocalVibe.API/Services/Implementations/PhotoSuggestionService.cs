using LocalVibe.API.DTOs.Photos;
using LocalVibe.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Web;

namespace LocalVibe.API.Services.Implementations;

/// <summary>
/// Gợi ý ảnh từ Pixabay (https://pixabay.com/api/docs/).
/// Miễn phí với API key đăng ký free tại pixabay.com.
/// Free tier: unlimited requests.
/// </summary>
public class PhotoSuggestionService : IPhotoSuggestionService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public PhotoSuggestionService(IHttpClientFactory factory, IConfiguration config)
    {
        _http   = factory.CreateClient("Pixabay");
        _apiKey = config["ExternalApis:Pixabay:ApiKey"] ?? string.Empty;
    }

    public async Task<PhotoSuggestionResponse> SuggestAsync(string query, int count = 5)
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
            return new PhotoSuggestionResponse { Query = query };

        var encoded = HttpUtility.UrlEncode(query);
        var url = $"?key={_apiKey}&q={encoded}&image_type=photo&per_page={count}&safesearch=true&lang=en";

        try
        {
            var json = await _http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);

            var photos = new List<PhotoItem>();
            if (doc.RootElement.TryGetProperty("hits", out var hits))
            {
                foreach (var item in hits.EnumerateArray())
                {
                    photos.Add(new PhotoItem
                    {
                        ThumbnailUrl = item.TryGetProperty("previewURL",  out var thumb) ? thumb.GetString() ?? "" : "",
                        FullUrl      = item.TryGetProperty("webformatURL", out var full)  ? full.GetString()  ?? "" : "",
                        Author       = item.TryGetProperty("user",         out var user)  ? user.GetString()  ?? "" : "",
                        SourceUrl    = item.TryGetProperty("pageURL",      out var page)  ? page.GetString()  ?? "" : ""
                    });
                }
            }

            return new PhotoSuggestionResponse { Query = query, Photos = photos };
        }
        catch
        {
            return new PhotoSuggestionResponse { Query = query };
        }
    }
}
