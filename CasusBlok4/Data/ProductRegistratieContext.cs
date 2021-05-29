using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CasusBlok4.Models;

    public class ProductRegistratieContext : DbContext
    {
        public ProductRegistratieContext (DbContextOptions<ProductRegistratieContext> options)
            : base(options)
        {
        }

        public DbSet<CasusBlok4.Models.Product> Product { get; set; }
    }
