using LocalVibe.API.DTOs.Auth;
using LocalVibe.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalVibe.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// POST /api/auth/login — Xác thực User và trả về JWT Token
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        
        if (response is null)
            return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu." });

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        
        if (response is null)
            return BadRequest(new { message = "Tài khoản đã tồn tại." });

        return Ok(response);
    }
}
