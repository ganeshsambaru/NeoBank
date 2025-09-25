using Microsoft.EntityFrameworkCore;
using NeoBank.Api.Models.Entities;
using System.Collections.Generic;

namespace NeoBank.Api.Data
{
    public class NeoBankDbContext : DbContext
    {
        public NeoBankDbContext(DbContextOptions<NeoBankDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        
    }
}
