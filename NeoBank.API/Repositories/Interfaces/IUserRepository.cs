using NeoBank.Api.Models;

namespace NeoBank.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByEmailAsync(string email);
        Task<User> AddAsync(User user);
    }
}
