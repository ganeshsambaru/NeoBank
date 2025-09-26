using Microsoft.EntityFrameworkCore;

namespace NeoBank.Api.Models.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public double InterestRate { get; set; }
        public int TermMonths { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Relationships
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
