using NeoBank.Api.Models.Entities;
using NeoBank.Api.Repositories.Interfaces;
using NeoBank.Api.Services.Interfaces;

namespace NeoBank.Api.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Account>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<Account?> GetByIdAsync(int id) =>
            await _repository.GetByIdAsync(id);

        public async Task AddAsync(Account account) =>
            await _repository.AddAsync(account);

        public async Task UpdateAsync(int id, Account account)
        {
            account.Id = id;
            await _repository.UpdateAsync(account);
        }

        public async Task DeleteAsync(int id) =>
            await _repository.DeleteAsync(id);
    }
}
