using NeoBank.Api.Models.Entities;
using NeoBank.Api.Repositories.Interfaces;
using NeoBank.Api.Services.Interfaces;
using NeoBank.API.Models.DTOs;

namespace NeoBank.Api.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAsync()
        {
            var accounts = await _repository.GetAllAsync();
            return accounts.Select(MapToDto);
        }

        public async Task<AccountDto?> GetByIdAsync(int id)
        {
            var account = await _repository.GetByIdAsync(id);
            return account == null ? null : MapToDto(account);
        }

        public async Task<AccountDto> AddAsync(AccountDto dto)
        {
            var entity = MapToEntity(dto);
            await _repository.AddAsync(entity);
            dto.Id = entity.Id;
            return dto;
        }

        public async Task UpdateAsync(int id, AccountDto dto)
        {
            var entity = MapToEntity(dto);
            entity.Id = id;
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        // mapping helpers
        private static AccountDto MapToDto(Account a) =>
            new AccountDto
            {
                Id = a.Id,
                AccountNumber = a.AccountNumber,
                AccountType = a.AccountType,
                Balance = a.Balance,
                CustomerId = a.CustomerId
            };

        private static Account MapToEntity(AccountDto d) =>
            new Account
            {
                Id = d.Id,
                AccountNumber = d.AccountNumber,
                AccountType = d.AccountType,
                Balance = d.Balance,
                CustomerId = d.CustomerId
            };
    }
}
