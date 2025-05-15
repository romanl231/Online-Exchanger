using Exchanger.API.Data;
using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Enums.AuthErrors;
using Exchanger.API.Enums.UploadToCloudErrors;
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exchanger.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public Task<IActionResult> Register([FromBody] AuthDTO authDTO) =>
            SafeExecuteAsync(async () =>
            {
                var result = await _userService.RegisterUserAsync(authDTO);
                return HandleAuthResult(result);
            }, "user registration");

        [Authorize]
        [HttpGet("me")]
        public Task<IActionResult> GetCurrentUserProfile() =>
          SafeExecuteAsync(async () =>
          {
              var userId = UserHelper.GetCurrentUserId(HttpContext);
              var result = await _userService.GetUserInfoAsync(userId);
              return Ok(result);
          }, "getting user profile");


        [Authorize]
        [HttpPatch("update")]
        public Task<IActionResult> UpdateUserInfo([FromBody] UpdateProfileDTO updateProfileDTO) =>
            SafeExecuteAsync(async () =>
            {
                var userId = UserHelper.GetCurrentUserId(HttpContext);
                var result = await _userService.UpdateUserInfoAsync(updateProfileDTO, userId);
                return HandleAuthResult(result);
            }, "updating user profile");

        [Authorize]
        [HttpPatch("change-avatar")]
        public Task<IActionResult> UpdateAvatar(IFormFile image) =>
            SafeExecuteAsync(async () => {
                var userId = UserHelper.GetCurrentUserId(HttpContext);
                var result = await _userService.UploadAvatarAsync(image, userId);
                return HandleCloudResult(result);
            }, "changing avatar");

        private IActionResult HandleAuthResult(AuthResult result)
        {
            if (!result.IsSuccess)
            {
                if (AuthErrorMessages.Messages
                    .TryGetValue(result.ErrorCode.Value, out var message))
                    return BadRequest(message);

                return BadRequest("Unknown error");
            }

            return Ok(result.IsSuccess);
        }

        private IActionResult HandleCloudResult(CloudResult result)
        {
            if (!result.IsSuccess)
            {
                if (CloudErrorMessages.Messages
                    .TryGetValue(result.ErrorCode.Value, out var message))
                    return BadRequest(message);

                return BadRequest("Unknown error");
            }

            return Ok(result.IsSuccess);
        }

        private async Task<IActionResult> SafeExecuteAsync(Func<Task<IActionResult>> action, string logContext)
        {
            try
            {
                return await action();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred during {logContext}.");
                return StatusCode(500, "An unexpected error occurred");
            }
        }
    }
}
