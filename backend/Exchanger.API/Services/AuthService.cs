using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Enums.AuthErrors;
using Exchanger.API.Services.IServices;

namespace Exchanger.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthService(IUserService userService, ITokenService tokenService) 
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<AuthResult> LoginAsync(AuthDTO authDTO, SessionInfo sessionInfo)
        {
            var result = await _userService.CheckDoesPasswordsMatches(authDTO);
            if(result.ErrorCode != null)
                return result;

            var session = await _tokenService.StartSessionAsync(result.User.Id, sessionInfo);
            return result.WithSession(session);
        }

        public async Task<AuthResult> LogoutAsync(Guid refreshToken)
        {
            if(!await _tokenService.RevokeSessionAsync(refreshToken))
                return AuthResult.Fail(AuthErrorCode.TokenInvalid);

            return AuthResult.Success();
        }
    }
}
