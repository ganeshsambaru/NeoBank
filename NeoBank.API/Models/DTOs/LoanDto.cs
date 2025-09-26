using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

public class LoanDto
{
    public int Id { get; set; }

    [Required, Range(1, double.MaxValue)]
    [Precision(18, 2)]
    public decimal Amount { get; set; }

    [Required, Range(0, 100)]
    public double InterestRate { get; set; }

    [Required, Range(1, 600)]
    public int TermMonths { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    public int CustomerId { get; set; }
}
