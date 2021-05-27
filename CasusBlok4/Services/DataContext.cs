using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasusBlok4.Models;
using Microsoft.EntityFrameworkCore;

namespace CasusBlok4.Services
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionProduct> TransactionProducts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
    }
}
