// Repositories/ITransactionRepository.cs
using NeoBankApi.Models;

namespace NeoBankApi.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(int id);
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
