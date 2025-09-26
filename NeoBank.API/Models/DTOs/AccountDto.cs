using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace NeoBank.API.Models.DTOs
{
    public class AccountDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Account number is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Account number must be between 4 and 50 characters")]
        [RegularExpression(@"^[A-Za-z0-9\-]+$", ErrorMessage = "Account number can only contain letters, digits and hyphens")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account type is required")]
        [StringLength(50, ErrorMessage = "Account type must not exceed 50 characters")]
        public string AccountType { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Balance must be zero or positive")]
        [Precision(18, 2)]
        public decimal Balance { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be a positive integer")]
        public int CustomerId { get; set; }
    }
}
