using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Enums.AuthErrors;
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exchanger.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthorizationController : ControllerBase 
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(IAuthService authService, ILogger<AuthorizationController> logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                ClearAuthCookie();

                var result = await _authService.LoginAsync(
                    loginRequestDTO.AuthDTO,
                    loginRequestDTO.SessionInfo
                );

                return HandleLoginResult(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login");
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult CheckIsUserLogedIn()
        {
            return Ok();
        }

        private IActionResult HandleLoginResult(AuthResult result)
        {
            if (!result.IsSuccess)
            {
                var message = AuthErrorMessages.Messages.GetValueOrDefault(result.ErrorCode.Value);
                return Unauthorized(message);
            }

            Response.Cookies.Append("AccessToken",
                result.OnCreationSession.AccessToken,
                result.OnCreationSession.AccessTokenCookieOptions);

            Response.Cookies.Append("RefreshToken",
                result.OnCreationSession.RefreshToken,
                result.OnCreationSession.RefreshTokenCookieOptions);

            return Ok("Successfully logged in");
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (!Request.Cookies.TryGetValue("RefreshToken", out var refreshTokenStr) ||
                    !Guid.TryParse(refreshTokenStr, out var refreshToken))
            {
                return BadRequest("Invalid or missing refresh token.");
            }

            var result = await _authService.LogoutAsync(refreshToken);

            if (!result.IsSuccess)
            {
                var message = AuthErrorMessages.Messages
                        .GetValueOrDefault(result.ErrorCode.Value);
                return BadRequest(message);
            }
            ClearAuthCookie();
            return Ok("You successfully logged out");
        }

        private void ClearAuthCookie()
        {
            Response.Cookies.Append("AccessToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            });

            Response.Cookies.Append("RefreshToken", "", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            });
        }
    }
}
