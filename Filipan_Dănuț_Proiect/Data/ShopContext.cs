using Filipan_Dănuț_Proiect.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filipan_Dănuț_Proiect.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProvidedDrink> ProvidedDrinks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Drink>().ToTable("Drink");
            modelBuilder.Entity<Provider>().ToTable("provider");
            modelBuilder.Entity<ProvidedDrink>().ToTable("ProvidedDrink");
            modelBuilder.Entity<ProvidedDrink>().HasKey(c => new { c.DrinkID, c.ProviderID });
        }
    }

}