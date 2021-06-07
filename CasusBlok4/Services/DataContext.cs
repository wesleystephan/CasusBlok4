using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasusBlok4.Models;
using CasusBlok4.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CasusBlok4.Services
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Categorie> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<SubCategorie> SubCategories { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionProduct> TransactionProducts { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var datetimeConverter = new DateTimeOffsetToBinaryConverter();

            builder.Entity<Categorie>(e =>
            {
                e.HasKey(q => q.Id);

                e.Property(q => q.Id)
                    .IsRequired();
                e.Property(q => q.Name)
                    .HasMaxLength(80)
                    .IsRequired();
            });

            builder.Entity<Customer>(e =>
            {
                e.HasKey(q => q.Id);

                e.Property(q => q.Id)
                    .HasMaxLength(24)
                    .IsRequired();
                e.Property(q => q.Name)
                    .HasMaxLength(80)
                    .IsRequired();
                e.Property(q => q.Saldo)
                    .IsRequired();
            });

            builder.Entity<Employee>(e =>
            {
                e.HasKey(q => q.Id);

                e.Property(q => q.Id)
                    .HasMaxLength(24)
                    .IsRequired();
                e.Property(q => q.Name)
                    .HasMaxLength(80)
                    .IsRequired();
            });

            builder.Entity<Product>(e =>
            {
                e.HasKey(q => q.Id);

                e.Property(q => q.Id)
                    .HasMaxLength(24)
                    .IsRequired();
                e.Property(q => q.Name)
                    .HasMaxLength(80)
                    .IsRequired();
                e.Property(q => q.PointsWorth)
                    .IsRequired(false);
                e.Property(q => q.CategorieId)
                    .IsRequired(false);
                e.Property(q => q.SubCategorieId)
                    .IsRequired(false);

                e.HasOne(q => q.Categorie)
                    .WithMany(q => q.Products)
                    .HasForeignKey(q=>q.CategorieId);

                e.HasOne(q => q.SubCategorie)
                    .WithMany(q => q.Products)
                    .HasForeignKey(q => q.SubCategorieId);
            });

            builder.Entity<SubCategorie>(e =>
            {
                e.HasKey(q => q.Id);

                e.Property(q => q.Id)
                    .IsRequired();
                e.Property(q => q.Name)
                    .HasMaxLength(80)
                    .IsRequired();
                e.Property(q => q.HeadCategorie)
                    .IsRequired();

                e.HasOne(q => q.Categorie)
                    .WithMany(q => q.SubCategories)
                    .HasForeignKey(q => q.HeadCategorie);
            });

            builder.Entity<Transaction>(e =>
            {
                e.HasKey(q => q.TransactionId);

                e.Property(q => q.TransactionId)
                    .IsRequired();
                e.Property(q => q.StartTime)
                    .HasConversion(datetimeConverter)
                    .IsRequired();
                e.Property(q => q.EndTime)
                    .HasConversion(datetimeConverter)
                    .IsRequired(false);
                e.Property(q => q.CustomerId)
                    .HasMaxLength(24)
                    .IsRequired();
                e.Property(q => q.EmployeeId)
                    .HasMaxLength(24)
                    .IsRequired();

                e.HasOne(q => q.Customer)
                    .WithMany(q => q.Transactions)
                    .HasForeignKey(q => q.CustomerId);
                e.HasOne(q => q.Employee)
                    .WithMany(q => q.Transactions)
                    .HasForeignKey(q => q.EmployeeId);
            });

            builder.Entity<TransactionProduct>(e =>
            {
                e.HasKey(q => q.TransactionId);

                e.Property(q => q.TransactionId)
                    .IsRequired();
                e.Property(q => q.ProductId)
                    .HasMaxLength(24)
                    .IsRequired();
                e.Property(q => q.IsForSell)
                    .HasConversion(new BoolToZeroOneConverter<byte>())
                    .IsRequired();
                e.Property(q => q.Points)
                    .IsRequired();
                e.Property(q => q.NumberOfProduct)
                    .IsRequired();

                e.HasOne(q => q.Transaction)
                    .WithMany(q => q.TransactionProducts)
                    .HasForeignKey(q => q.TransactionId);
                e.HasOne(q => q.Product)
                    .WithMany(q => q.TransactionProducts)
                    .HasForeignKey(q => q.ProductId);
            });
        }
    }
}
