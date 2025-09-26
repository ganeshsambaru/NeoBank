using Microsoft.EntityFrameworkCore;
using NeoBank.Api.Data;     // your DbContext
using NeoBankApi.Models;    // your Transaction entity

namespace NeoBankApi.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly NeoBankDbContext _context;

        public TransactionRepository(NeoBankDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync() =>
            await _context.Transactions.AsNoTracking().ToListAsync();

        public async Task<Transaction?> GetByIdAsync(int id) =>
            await _context.Transactions.FindAsync(id);

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }

        public Task UpdateAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var tx = await _context.Transactions.FindAsync(id);
            if (tx != null)
            {
                _context.Transactions.Remove(tx);
            }
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
