using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Enums.AuthErrors;

namespace Exchanger.API.Services.IServices
{
    public interface IAuthService
    {
        Task<AuthResult> Login(AuthDTO authDTO, SessionInfo sessionInfo);
        Task<AuthResult> Logout(Guid refreshToken);
    }
}
