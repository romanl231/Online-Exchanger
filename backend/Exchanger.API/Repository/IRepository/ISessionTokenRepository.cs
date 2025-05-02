using Exchanger.API.Entities;

namespace Exchanger.API.Repository.IRepository
{
    public interface ISessionTokenRepository
    {
        Task UpdateTokenAsync(SessionToken token);
        Task SaveTokenAsync(SessionToken token);
        Task<SessionToken> GetTokenByIdAsync(Guid token);
        Task<List<SessionToken>> GetUnexpiredTokensByUserIdAsync(Guid userId); 
    }
}
