using LocalVibe.API.DTOs.Geocoding;

namespace LocalVibe.API.Services.Interfaces;

public interface IGeocodingService
{
    /// <summary>Tìm kiếm địa điểm theo tên (Nominatim/OpenStreetMap).</summary>
    Task<IEnumerable<GeocodingResult>> SearchAsync(string query, int limit = 5);

    /// <summary>Chuyển toạ độ thành địa chỉ (reverse geocoding).</summary>
    Task<ReverseGeocodingResponse?> ReverseAsync(double latitude, double longitude);
}
