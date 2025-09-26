// DTOs/TransactionDto.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace NeoBankApi.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AccountId { get; set; }
    }

    public class CreateTransactionDto
    {
        [Required]
        [StringLength(50)]
        public string TransactionType { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        [Precision(18, 2)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int AccountId { get; set; }
    }

    public class UpdateTransactionDto : CreateTransactionDto
    {
        // nothing extra, just reuse the fields
    }
}
