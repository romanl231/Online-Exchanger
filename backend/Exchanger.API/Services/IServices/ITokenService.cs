using Exchanger.API.DTOs.AuthDTOs;

namespace Exchanger.API.Services.IServices
{
    public interface ITokenService
    {
        Task<OnCreationSessionDTO> StartSessionAsync(Guid userId);
        Task<OnCreationSessionDTO> RefreshSessionAsync(string refreshToken);
        Task<bool> RevokeSessionAsync(string refreshToken);
        Task<List<DisplaySessionInfoDTO>> GetSessionsByUserIdAsync(Guid userId);
    }
}
