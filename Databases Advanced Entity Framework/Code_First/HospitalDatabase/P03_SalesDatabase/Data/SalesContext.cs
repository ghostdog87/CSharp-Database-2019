using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Store> Stores { get; set; }
        DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateProductModel(modelBuilder);
            CreateCustomerModel(modelBuilder);
            CreateStoreModel(modelBuilder);
            CreateSaleModel(modelBuilder);
        }

        private void CreateSaleModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Sale>()
                .HasKey(p => p.SaleId);

            modelBuilder
                .Entity<Sale>()
                .Property(p => p.Date)
                .HasDefaultValueSql("GETDATE()");
        }

        private void CreateStoreModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Store>()
                .HasKey(p => p.StoreId);

            modelBuilder
                .Entity<Store>()
                .Property(p => p.Name)
                .HasMaxLength(80)
                .IsUnicode();

            modelBuilder
               .Entity<Store>()
               .HasMany(p => p.Sales)
               .WithOne(p => p.Store);
        }

        private void CreateCustomerModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Customer>()
                .HasKey(p => p.CustomerId);

            modelBuilder
                .Entity<Customer>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnicode();

            modelBuilder
                .Entity<Customer>()
                .Property(p => p.Email)
                .HasMaxLength(80);

            modelBuilder
               .Entity<Customer>()
               .HasMany(p => p.Sales)
               .WithOne(p => p.Customer);
        }

        private void CreateProductModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder
                .Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(250);

            modelBuilder
               .Entity<Product>()
               .HasMany(p => p.Sales)
               .WithOne(p => p.Product);
        }
    }
}
