using Microsoft.EntityFrameworkCore;
using NeoBank.Api.Data;
using NeoBank.Api.Models.Entities;
using NeoBank.API.Repositories.Interfaces;

namespace NeoBank.API.Repositories.Implementations
{
    public class LoanRepository : ILoanRepository
    {
        private readonly NeoBankDbContext _context;
        public LoanRepository(NeoBankDbContext context) => _context = context;

        public async Task<IEnumerable<Loan>> GetAllAsync() =>
            await _context.Loans.ToListAsync();

        public async Task<Loan> GetByIdAsync(int id) =>
            await _context.Loans.FindAsync(id);

        public async Task AddAsync(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Loan loan)
        {
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Loans.FindAsync(id);
            if (entity != null)
            {
                _context.Loans.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}
