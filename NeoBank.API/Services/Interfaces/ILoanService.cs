namespace NeoBank.API.Services.Interfaces
{
    public interface ILoanService
    {
        Task<IEnumerable<LoanDto>> GetAllAsync();
        Task<LoanDto> GetByIdAsync(int id);
        Task AddAsync(LoanDto dto);
        Task UpdateAsync(int id, LoanDto dto);
        Task DeleteAsync(int id);
    }

}
