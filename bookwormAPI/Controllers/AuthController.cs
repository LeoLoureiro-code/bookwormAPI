using bookwormAPI.DTO;
using bookwormAPI.EF.DataAccess.DTO;
using bookwormAPI.EF.DataAccess.Services;
using bookwormAPI.EF.DataAccess.Services.Interfaces;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace bookwormAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO login)
        {
            try
            {
                var (accessToken, refreshToken, userId) = await _authService.LoginAsync(login.Email, login.Password);
                return Ok(new
                {
                    accessToken,
                    refreshToken,
                    userId,
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    Message = ex.Message
                });
            }
        }

        [Authorize]
        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody] UserDTO userDTO)
        {
            var (newAccessToken, newRefreshToken, userId) = await _authService.LoginAsync(userDTO.Email, userDTO.Password);
            return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LoginDTO login)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            await _authService.RevokeAndRefreshTokenAsync(login.Email, login.Password);
            return Ok(new { Message = "Logged out successfully." });
        }
    }
}
