using Microsoft.EntityFrameworkCore;
using NeoBank.Api.Data;
using NeoBank.Api.Models.Entities;
using NeoBank.Api.Repositories.Interfaces;

namespace NeoBank.Api.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly NeoBankDbContext _context;
        public CustomerRepository(NeoBankDbContext context) => _context = context;

        public async Task<IEnumerable<Customer>> GetAllAsync() =>
            await _context.Customers.ToListAsync();

        public async Task<Customer> GetByIdAsync(int id) =>
            await _context.Customers.FindAsync(id);

        public async Task AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Customers.FindAsync(id);
            if (entity != null)
            {
                _context.Customers.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
