using NeoBank.Api.Models.Entities;
using NeoBank.API.Repositories.Interfaces;
using NeoBank.API.Services.Interfaces;

namespace NeoBank.API.Services.Implementations
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _repo;
        public LoanService(ILoanRepository repo) => _repo = repo;

        public async Task<IEnumerable<LoanDto>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            return entities.Select(x => new LoanDto
            {
                Id = x.Id,
                Amount = x.Amount,
                InterestRate = x.InterestRate,
                TermMonths = x.TermMonths,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                CustomerId = x.CustomerId
            });
        }

        public async Task<LoanDto> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            return x == null ? null : new LoanDto
            {
                Id = x.Id,
                Amount = x.Amount,
                InterestRate = x.InterestRate,
                TermMonths = x.TermMonths,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                CustomerId = x.CustomerId
            };
        }

        public async Task AddAsync(LoanDto dto)
        {
            var entity = new Loan
            {
                Amount = dto.Amount,
                InterestRate = dto.InterestRate,
                TermMonths = dto.TermMonths,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CustomerId = dto.CustomerId
            };
            await _repo.AddAsync(entity);
            dto.Id = entity.Id;
        }

        public async Task UpdateAsync(int id, LoanDto dto)
        {
            var entity = new Loan
            {
                Id = id,
                Amount = dto.Amount,
                InterestRate = dto.InterestRate,
                TermMonths = dto.TermMonths,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CustomerId = dto.CustomerId
            };
            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }

}
