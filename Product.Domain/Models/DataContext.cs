using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnUpdate();
            modelBuilder.Entity<ProductModel>().HasKey(p => p.Id);
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // Category
            modelBuilder.Entity<CategoryModel>().HasKey(c => c.Id);

            // Product-Category relationship (many-to-one)
            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cart
            modelBuilder.Entity<CartModel>().HasKey(c => c.Id);
            modelBuilder.Entity<CartModel>()
                .HasMany(c => c.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem
            modelBuilder.Entity<CartItemModel>().HasKey(ci => ci.Id);
        }
    }
}
