using Exchanger.API.Data;
using Exchanger.API.DTOs.AuthDTOs;
using Exchanger.API.Entities;
using Exchanger.API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Exchanger.API.Repositories
{
    public class SessionTokenRepository : ISessionTokenRepository
    {
        private readonly AppDbContext _context;

        public SessionTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(SessionToken token)
        {
            if (token == null)
            {
                return false;
            }

            _context.SessionTokens.Add(token);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateTokenAsync(SessionToken token)
        {
            _context.SessionTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task<SessionToken> GetTokenByIdAsync(Guid tokenId)
        {
            var token = await _context.SessionTokens
                .FirstOrDefaultAsync(t => t.Id == tokenId);
            return token;
        }

        public async Task<List<SessionToken>> GetUnexpiredTokensByUserIdAsync(Guid userId)
        {
            var tokens = await _context.SessionTokens
                .Where(t => t.UserId == userId && !t.IsRevoked).ToListAsync();
            return tokens;
        }
    }
}
