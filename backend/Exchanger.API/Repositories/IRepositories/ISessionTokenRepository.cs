using Exchanger.API.Entities;

namespace Exchanger.API.Repositories.IRepositories
{
    public interface ISessionTokenRepository
    {
        Task UpdateTokenAsync(SessionToken token);
        Task<bool> AddAsync(SessionToken token);
        Task<SessionToken> GetTokenByIdAsync(Guid token);
        Task<List<SessionToken>> GetUnexpiredTokensByUserIdAsync(Guid userId); 
    }
}
