using LocalVibe.API.DTOs.Geocoding;
using LocalVibe.API.Services.Interfaces;
using System.Text.Json;
using System.Web;

namespace LocalVibe.API.Services.Implementations;

/// <summary>
/// Geocoding dùng Nominatim / OpenStreetMap (https://nominatim.org).
/// Hoàn toàn miễn phí, không cần key. Yêu cầu User-Agent header theo policy của Nominatim.
/// Giới hạn: 1 request/giây — đủ cho production nhỏ.
/// </summary>
public class GeocodingService : IGeocodingService
{
    private readonly HttpClient _http;

    public GeocodingService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Nominatim");
    }

    public async Task<IEnumerable<GeocodingResult>> SearchAsync(string query, int limit = 5)
    {
        var encoded = HttpUtility.UrlEncode(query);
        var url = $"search?q={encoded}&format=json&limit={limit}&addressdetails=1&accept-language=vi";

        var json = await _http.GetStringAsync(url);
        using var doc = JsonDocument.Parse(json);

        var results = new List<GeocodingResult>();
        foreach (var item in doc.RootElement.EnumerateArray())
        {
            var address = item.TryGetProperty("address", out var addr) ? addr : (JsonElement?)null;
            results.Add(new GeocodingResult
            {
                DisplayName = item.GetProperty("display_name").GetString() ?? string.Empty,
                Latitude    = double.Parse(item.GetProperty("lat").GetString() ?? "0",
                                           System.Globalization.CultureInfo.InvariantCulture),
                Longitude   = double.Parse(item.GetProperty("lon").GetString() ?? "0",
                                           System.Globalization.CultureInfo.InvariantCulture),
                Type        = item.TryGetProperty("type", out var t) ? t.GetString() ?? "" : "",
                City        = address?.TryGetProperty("city", out var city) == true ? city.GetString()
                            : address?.TryGetProperty("town", out var town) == true ? town.GetString()
                            : null,
                Country     = address?.TryGetProperty("country", out var country) == true
                            ? country.GetString() : null
            });
        }

        return results;
    }

    public async Task<ReverseGeocodingResponse?> ReverseAsync(double latitude, double longitude)
    {
        var lat = latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
        var lng = longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
        var url = $"reverse?lat={lat}&lon={lng}&format=json&addressdetails=1&accept-language=vi";

        var json = await _http.GetStringAsync(url);
        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("display_name", out _))
            return null;

        var addr = doc.RootElement.TryGetProperty("address", out var a) ? a : (JsonElement?)null;

        return new ReverseGeocodingResponse
        {
            DisplayName = doc.RootElement.GetProperty("display_name").GetString() ?? string.Empty,
            Latitude    = latitude,
            Longitude   = longitude,
            Road        = addr?.TryGetProperty("road", out var road) == true ? road.GetString() : null,
            Suburb      = addr?.TryGetProperty("suburb", out var suburb) == true ? suburb.GetString() : null,
            City        = addr?.TryGetProperty("city", out var city) == true ? city.GetString()
                        : addr?.TryGetProperty("town", out var town) == true ? town.GetString() : null,
            State       = addr?.TryGetProperty("state", out var state) == true ? state.GetString() : null,
            Country     = addr?.TryGetProperty("country", out var country) == true ? country.GetString() : null
        };
    }
}
