namespace LocalVibe.API.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    /// Mật khẩu đã được mã hóa (BCrypt/PBKDF2). Không lưu plaintext.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "User"; // "Admin", "User"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
