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
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ProfileData> ProfileData { get; set; }
        public virtual DbSet<ProfileData> Employees { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionProduct> TransactionProducts { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DataContext() {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var datetimeConverter = new DateTimeOffsetToBinaryConverter();

            builder.Entity<Category>(e =>
            {
                e.ToTable("ArtikelSoorten");

                e.HasKey(q => q.Id);

                e.Property(q => q.Id)
                    .HasColumnName("ArtikelID")
                    .IsRequired();
                e.Property(q => q.Name)
                    .HasColumnName("ArtikelNaam")
                    .HasMaxLength(80)
                    .IsRequired();
            });

            builder.Entity<ProfileData>(e =>
            {
                e.ToTable("ProfileData");

                e.HasKey(q => q.Id);

                e.Property(q => q.Id)
                    .HasColumnName("Id")
                    .IsRequired();
                e.Property(q => q.Email)
                    .HasColumnName("Email")
                    .HasMaxLength(50)
                    .IsRequired(false);
                e.Property(q => q.FirstName)
                    .HasColumnName("Voornaam")
                    .HasMaxLength(50)
                    .IsRequired(false);
                e.Property(q => q.LastName)
                    .HasColumnName("Achternaam")
                    .HasMaxLength(50)
                    .IsRequired(false);
                e.Property(q => q.Balans)
                    .HasDefaultValue(0)
                    .HasColumnName("Balans")
                    .IsRequired();
                e.Property(q => q.AccountTypeId)
                    .HasColumnName("AccountType")
                    .IsRequired(false);
                e.Property(q => q.MemberCardId)
                    .HasColumnName("Ledenpas")
                    .IsRequired(false);
                e.Property(q => q.Street)
                    .HasColumnName("Straat")
                    .HasMaxLength(50)
                    .IsRequired(false);
                e.Property(q => q.HouseNumber)
                    .HasColumnName("Huisnummer")
                    .HasMaxLength(10)
                    .IsRequired(false);
                e.Property(q => q.City)
                    .HasColumnName("Woonplaats")
                    .HasMaxLength(50)
                    .IsRequired(false);
                e.Property(q => q.PostalCode)
                    .HasColumnName("Postcode")
                    .HasMaxLength(10)
                    .IsRequired(false);
                e.Property(q => q.AccountCreated)
                    .HasColumnName("DateCreated")
                    .HasConversion(datetimeConverter)
                    .IsRequired(false);
                e.Property(q => q.DateOfBirth)
                    .HasColumnName("Geboortedatum")
                    .HasConversion(datetimeConverter)
                    .IsRequired(false);

            });

            builder.Entity<Product>(e =>
            {
                e.ToTable("Artikelen");

                e.HasKey(q => q.Id);

                e.Property(q => q.Id)
                    .HasColumnName("ArtikelID")
                    .IsRequired();
                e.Property(q => q.Name)
                    .HasColumnName("ArtikelNaam")
                    .HasMaxLength(50)
                    .IsRequired(false);
                e.Property(q => q.PointsWorth)
                    .HasColumnName("ArtikelPunten")
                    .IsRequired();
                e.Property(q => q.CategorieId)
                    .HasColumnName("ArtikelSoortID")
                    .IsRequired();
                e.Property(q => q.SerialNumber)
                    .HasColumnName("Serienummer")
                    .HasMaxLength(50)
                    .IsRequired(false);

                e.HasOne(q => q.Categorie)
                    .WithMany(q => q.Products)
                    .HasForeignKey(q=>q.CategorieId);

            });

            builder.Entity<Transaction>(e =>
            {
                e.ToTable("Transacties");

                e.HasKey(q => q.TransactionId);

                e.Property(q => q.TransactionId)
                    .HasColumnName("TransactieID")
                    .IsRequired();
                e.Property(q => q.IsLoan)
                    .HasColumnName("Lening")
                    .IsRequired();
                e.Property(q => q.Date)
                    .HasColumnName("Datum")
                    .HasConversion(datetimeConverter)
                    .HasDefaultValue(DateTimeOffset.MinValue)
                    .IsRequired();
                e.Property(q => q.ProfileId)
                    .HasColumnName("ProfileId")
                    .IsRequired();
                e.Property(q => q.IsDonation)
                    .HasColumnName("Donatie")
                    .IsRequired();
                e.Property(q => q.SerialNumber)
                    .HasColumnName("Serienummer")
                    .HasDefaultValue(null)
                    .HasMaxLength(50)
                    .IsRequired(false);

                e.HasOne(q => q.Customer)
                    .WithMany(q => q.Transactions)
                    .HasForeignKey(q => q.ProfileId);
            });

            builder.Entity<TransactionProduct>(e =>
            {
                e.ToTable("TransactieArtikelen");
                e.HasKey(q => q.TransactionProductId);

                e.Property(q => q.TransactionId)
                    .HasColumnName("TransactieID")
                    .IsRequired();
                e.Property(q => q.ProductId)
                    .HasColumnName("ArtikelID")
                    .HasMaxLength(24)
                    .IsRequired();
                e.Property(q => q.IsForSell)
                    .HasColumnName("IsVerkoop")
                    .HasConversion(new BoolToZeroOneConverter<byte>())
                    .IsRequired();
                e.Property(q => q.Points)
                    .HasColumnName("Punten")
                    .IsRequired();
                e.Property(q => q.NumberOfProduct)
                    .HasColumnName("Aantal")
                    .IsRequired();
                e.Property(q => q.TransactionProductId)
                    .HasColumnName("TransactieArtikelId")
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
