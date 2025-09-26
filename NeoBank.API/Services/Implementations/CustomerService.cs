using NeoBank.Api.Models.DTOs;
using NeoBank.Api.Models.Entities;
using NeoBank.Api.Repositories.Interfaces;
using NeoBank.Api.Services.Interfaces;

namespace NeoBank.Api.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo) => _repo = repo;

        public async Task<IEnumerable<CustomerDto>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(x => new CustomerDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Phone = x.Phone,
                Address = x.Address
            });
        }

        public async Task<CustomerDto> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : new CustomerDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Phone = x.Phone,
                Address = x.Address
            };
        }

        public async Task AddAsync(CustomerDto dto)
        {
            var entity = new Customer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address
            };
            await _repo.AddAsync(entity);
            dto.Id = entity.Id;
        }

        public async Task UpdateAsync(int id, CustomerDto dto)
        {
            var entity = new Customer
            {
                Id = id,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address
            };
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}
