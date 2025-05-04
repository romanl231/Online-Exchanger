using Exchanger.API.Data;
using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Services.IServices;
using Exchanger.API.Repositories.IRepositories;

namespace Exchanger.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly ISessionTokenRepository _sessionTokenRepository;

        public TokenService(ISessionTokenRepository sessionTokenRepository)
        {
            _sessionTokenRepository = sessionTokenRepository;
        }

        public async Task<OnCreationSessionDTO> StartSessionAsync(Guid userId) 
        {
            return new OnCreationSessionDTO();
        }

        public async Task<OnCreationSessionDTO> RefreshSessionAsync(string refreshToken)
        {
            return new OnCreationSessionDTO();
        }

        public async Task<bool> RevokeSessionAsync(string refreshToken)
        {
            return false;
        }

        public async Task<List<DisplaySessionInfoDTO>> GetSessionsByUserIdAsync(Guid userId)
        {
            return new List<DisplaySessionInfoDTO>();
        }
    }
}
