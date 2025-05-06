using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Enums.AuthErrors;
using Exchanger.API.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exchanger.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthorizationController> _logger;

        public UserController(IUserService userService, ILogger<AuthorizationController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]AuthDTO authDTO) 
        {
            try
            {
                var result = await _userService.RegisterUserAsync(authDTO);
                return HandleRegistrationResult(result);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during user registration.");
                return StatusCode(500, "An unexpected error occured");
            }
        }

        private IActionResult HandleRegistrationResult(AuthResult result)
        {
            if (!result.IsSuccess)
            {
                if (AuthErrorMessages.Messages
                    .TryGetValue(result.ErrorCode.Value, out var message))
                    return BadRequest(message);

                return BadRequest("Unknown error");
            }

            return Ok(result);
        }
    }
}
