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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ProductModel>()
                .Property(p => p.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnUpdate();
            modelBuilder.Entity<ProductModel>().HasKey(p => p.Id);
            modelBuilder.Entity<CategoryModel>().HasKey(c => c.Name);
            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.Name)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
