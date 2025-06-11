using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Enums.AuthErrors;
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exchanger.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger<TokenController> _logger;

        public TokenController(ITokenService tokenService, ILogger<TokenController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("refresh-jwt")]
        public async Task<IActionResult> RefreshJWT([FromBody]SessionInfo session)
        {
            try
            {
                if (Request.Cookies.TryGetValue("RefreshToken", out var refreshToken) || !string.IsNullOrEmpty(refreshToken))
                {
                    var response = await _tokenService.RefreshSessionAsync(Guid.Parse(refreshToken), session);

                    if (response == null)
                        return Unauthorized();

                    return HandleRefreshResult(response);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred during refreshing token.");
                return Unauthorized();
            }
        }

        private IActionResult HandleRefreshResult(OnCreationSessionDTO dto)
        {
            ClearAuthCookie();
            Response.Cookies.Append("AccessToken",
                dto.AccessToken,
                dto.AccessTokenCookieOptions);

            Response.Cookies.Append("RefreshToken",
                dto.RefreshToken,
                dto.RefreshTokenCookieOptions);

            return Ok("Successfully logged in");
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
