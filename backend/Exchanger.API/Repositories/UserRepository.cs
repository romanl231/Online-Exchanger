using Exchanger.API.Data;
using Exchanger.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exchanger.API.Repositories
{
    public class UserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(User user)
        {
            if (user == null)
                return false;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {   
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            if (user == null)
                return false;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
