using Exchanger.API.DTOs.AuthDTOs;

namespace Exchanger.API.Services.IServices
{
    public interface ITokenService
    {
        Task<OnCreationSessionDTO> StartSessionAsync(Guid userId, SessionInfo sessionInfo);
        Task<OnCreationSessionDTO>? RefreshSessionAsync(Guid refreshToken, SessionInfo sessionInfo);
        Task<bool> RevokeSessionAsync(Guid refreshToken);
        Task<List<DisplaySessionInfoDTO>> GetSessionsByUserIdAsync(Guid userId);
    }
}
