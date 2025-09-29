using Microsoft.EntityFrameworkCore;
using NeoBank.Api.Models;
using NeoBank.Api.Models.Entities;
using NeoBankApi.Models;

namespace NeoBank.Api.Data
{
    public class NeoBankDbContext : DbContext
    {
        public NeoBankDbContext(DbContextOptions<NeoBankDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<User> Users { get; set; }


       
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer–Account: one Customer has many Accounts
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer–Loan: one Customer has many Loans
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Customer)
                .WithMany(c => c.Loans)           // <-- add Loans collection to Customer entity
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
