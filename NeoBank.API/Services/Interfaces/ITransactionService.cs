// Services/ITransactionService.cs
using NeoBankApi.DTOs;

namespace NeoBankApi.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllAsync();
        Task<TransactionDto?> GetByIdAsync(int id);
        Task<TransactionDto> CreateAsync(CreateTransactionDto dto);
        Task<TransactionDto?> UpdateAsync(int id, UpdateTransactionDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
