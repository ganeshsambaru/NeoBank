using Microsoft.EntityFrameworkCore;
using NeoBank.Api.Models.Entities;

public class Account
{
    public int Id { get; set; }

    // e.g. 1234567890
    public string AccountNumber { get; set; } = string.Empty;

    // e.g. "Savings", "Current"
    public string AccountType { get; set; } = string.Empty;
    [Precision(18, 2)]
    public decimal Balance { get; set; }

    // Foreign key to Customer
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
}
