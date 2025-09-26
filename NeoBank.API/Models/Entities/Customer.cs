﻿namespace NeoBank.Api.Models.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
