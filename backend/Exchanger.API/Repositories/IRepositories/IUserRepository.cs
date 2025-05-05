using Exchanger.API.Entities;

namespace Exchanger.API.Repositories.IRepositories
{
    public interface IUserRepository 
    {
        Task<bool> AddAsync(User user);
        Task<User> GetByIdAsync(Guid userId);
        Task<bool> UpdateAsync(User user);
    }
}
