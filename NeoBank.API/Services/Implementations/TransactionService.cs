using Microsoft.EntityFrameworkCore;
using NeoBankApi.DTOs;
using NeoBankApi.Models;
using NeoBankApi.Repositories;
using NeoBank.Api.Data; // NeoBankDbContext

namespace NeoBankApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repo;
        private readonly NeoBankDbContext _context;

        public TransactionService(ITransactionRepository repo, NeoBankDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<IEnumerable<TransactionDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(t => new TransactionDto
            {
                Id = t.Id,
                TransactionType = t.TransactionType,
                Amount = t.Amount,
                TransactionDate = t.TransactionDate,
                AccountId = t.AccountId
            });
        }

        public async Task<TransactionDto?> GetByIdAsync(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null) return null;

            return new TransactionDto
            {
                Id = t.Id,
                TransactionType = t.TransactionType,
                Amount = t.Amount,
                TransactionDate = t.TransactionDate,
                AccountId = t.AccountId
            };
        }

        public async Task<TransactionDto> CreateAsync(CreateTransactionDto dto)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Id == dto.AccountId);

            if (account == null)
                return null!; // let controller/global handler respond 404

            // Adjust balance
            if (dto.TransactionType.Equals("Credit", StringComparison.OrdinalIgnoreCase))
            {
                account.Balance += dto.Amount;
            }
            else if (dto.TransactionType.Equals("Debit", StringComparison.OrdinalIgnoreCase))
            {
                if (account.Balance < dto.Amount)
                    return null!; // insufficient funds
                account.Balance -= dto.Amount;
            }

            var transaction = new Transaction
            {
                TransactionType = dto.TransactionType,
                Amount = dto.Amount,
                TransactionDate = dto.TransactionDate,
                AccountId = dto.AccountId
            };

            await _repo.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return new TransactionDto
            {
                Id = transaction.Id,
                TransactionType = transaction.TransactionType,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                AccountId = transaction.AccountId
            };
        }



        public async Task<TransactionDto?> UpdateAsync(int id, UpdateTransactionDto dto)
        {
            // Load existing transaction (tracked)
            var existingTx = await _context.Transactions
                .Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingTx == null)
                return null;

            // Step 1: reverse effect of old transaction using OLD values
            var oldAccount = existingTx.Account;

            if (existingTx.TransactionType.Equals("Credit", StringComparison.OrdinalIgnoreCase))
                oldAccount.Balance -= existingTx.Amount;
            else if (existingTx.TransactionType.Equals("Debit", StringComparison.OrdinalIgnoreCase))
                oldAccount.Balance += existingTx.Amount;

            // Step 2: determine target account
            Account targetAccount = oldAccount;
            if (oldAccount.Id != dto.AccountId)
            {
                targetAccount = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.Id == dto.AccountId);

                if (targetAccount == null)
                    return null; // no such account
            }

            // Step 3: apply NEW effect to target account
            if (dto.TransactionType.Equals("Credit", StringComparison.OrdinalIgnoreCase))
            {
                targetAccount.Balance += dto.Amount;
            }
            else if (dto.TransactionType.Equals("Debit", StringComparison.OrdinalIgnoreCase))
            {
                if (targetAccount.Balance < dto.Amount)
                    return null; // insufficient funds
                targetAccount.Balance -= dto.Amount;
            }

            // Step 4: now update the transaction fields AFTER applying balances
            existingTx.TransactionType = dto.TransactionType;
            existingTx.Amount = dto.Amount;
            existingTx.TransactionDate = dto.TransactionDate;
            existingTx.AccountId = dto.AccountId;

            // EF is already tracking everything
            await _context.SaveChangesAsync();

            return new TransactionDto
            {
                Id = existingTx.Id,
                TransactionType = existingTx.TransactionType,
                Amount = existingTx.Amount,
                TransactionDate = existingTx.TransactionDate,
                AccountId = existingTx.AccountId
            };
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            if (t == null) return false;

            await _repo.DeleteAsync(id);
            await _repo.SaveChangesAsync();
            return true;
        }
    }
}
