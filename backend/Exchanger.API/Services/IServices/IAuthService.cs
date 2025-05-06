using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Enums.AuthErrors;

namespace Exchanger.API.Services.IServices
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(AuthDTO authDTO, SessionInfo sessionInfo);
        Task<AuthResult> LogoutAsync(Guid refreshToken);
    }
}
