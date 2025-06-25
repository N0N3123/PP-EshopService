using Microsoft.EntityFrameworkCore;

namespace Eshop.Domain.Models
{
    public class EshopDataContext : DbContext
    {
        public EshopDataContext(DbContextOptions<EshopDataContext> options) : base(options) { }
        public DbSet<CustomerModel> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerModel>().HasKey(c => c.Id);

        }
    }
}