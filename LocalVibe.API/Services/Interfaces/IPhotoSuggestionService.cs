using LocalVibe.API.DTOs.Photos;

namespace LocalVibe.API.Services.Interfaces;

public interface IPhotoSuggestionService
{
    /// <summary>
    /// Gợi ý ảnh từ Pixabay theo từ khóa tìm kiếm.
    /// Dùng để hiển thị ảnh placeholder khi Post chưa có ImageUrl.
    /// </summary>
    Task<PhotoSuggestionResponse> SuggestAsync(string query, int count = 5);
}
