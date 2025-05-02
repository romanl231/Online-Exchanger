using Exchanger.API.Data;
using Exchanger.API.Entities;
using Exchanger.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Exchanger.API.Repository
{
    public class SessionTokenRepository : ISessionTokenRepository
    {
        private readonly AppDbContext _context;

        public SessionTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveTokenAsync(SessionToken token)
        {
            _context.SessionTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTokenAsync(SessionToken token)
        {
            _context.SessionTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task<SessionToken> GetTokenByIdAsync(Guid tokenId)
        {
            return 
                await _context.SessionTokens.FirstOrDefaultAsync(t => t.Id == tokenId);
        }

        public async Task<List<SessionToken>> GetUnexpiredTokensByUserIdAsync(Guid userId)
        {
            var tokens = await _context.SessionTokens
                .Where(t => t.UserId == userId && !t.IsRevoked).ToListAsync();
            return tokens;
        }
    }
}
