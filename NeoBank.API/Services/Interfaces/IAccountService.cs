using NeoBank.Api.Models.Entities;

namespace NeoBank.Api.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(int id);
        Task AddAsync(Account account);
        Task UpdateAsync(int id, Account account);
        Task DeleteAsync(int id);
    }
}
