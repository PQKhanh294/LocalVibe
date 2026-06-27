namespace LocalVibe.API.Services.Interfaces;

public interface IFileUploadService
{
    /// <summary>
    /// Lưu file ảnh vào wwwroot/uploads và trả về relative URL.
    /// </summary>
    /// <returns>Relative URL, e.g. /uploads/abc123.jpg</returns>
    Task<string> SaveAsync(IFormFile file);
}
