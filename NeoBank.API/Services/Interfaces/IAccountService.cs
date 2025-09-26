using NeoBank.API.Models.DTOs;

namespace NeoBank.Api.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAllAsync();
        Task<AccountDto?> GetByIdAsync(int id);
        Task<AccountDto> AddAsync(AccountDto dto);
        Task UpdateAsync(int id, AccountDto dto);
        Task DeleteAsync(int id);
    }
}
