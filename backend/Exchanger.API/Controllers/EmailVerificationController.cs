using Exchanger.API.Data;
using Exchanger.API.Enums.AuthErrors;
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Exchanger.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class EmailVerificationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<EmailVerificationController> _logger;

        public EmailVerificationController(
            IUserService userService, 
            ILogger<EmailVerificationController> logger) 
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Sends a verification email with confirmation link to the current user.
        /// </summary>
        [Authorize]
        [HttpPost("send-verification-email")]
        public Task<IActionResult> SendVerificationEmail() =>
            SafeExecuteAsync(async () =>
            {
                var userId = UserHelper.GetCurrentUserId(HttpContext);
                var result = await _userService.InitiateEmailConfirmationAsync(userId);
                return HandleAuthResult(result);

            }, "Initiating email verification");

        /// <summary>
        /// Verifying email with confirmation token
        /// </summary>
        [HttpPost("verify-email/{token}")]
        public Task<IActionResult> VerifyEmail(string token) =>
            SafeExecuteAsync(async () =>
            {
                var result = await _userService.ConfirmUserEmailAsync(token);
                return HandleAuthResult(result);
            }, "Verifying email with token");

        private IActionResult HandleAuthResult(AuthResult result)
        {
            if (!result.IsSuccess)
            {
                if (result.ErrorCode.HasValue)
                {
                    if (AuthErrorMessages.Messages
                        .TryGetValue(result.ErrorCode.Value, out var message))
                        return BadRequest(message);
                }

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
