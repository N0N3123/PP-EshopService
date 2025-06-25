using Microsoft.EntityFrameworkCore;
using Product.Domain.Models;
using Product.Domain.Repositories;
using Xunit;

namespace EShop.Domain.Tests
{
    public class CustomerRepositoryTests
    {
        private DataContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new DataContext(options);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ReturnsCustomer_WhenExists()
        {
            using var context = GetInMemoryContext("GetCustomerByIdAsync_ReturnsCustomer_WhenExists");
            var customer = new CustomerModel
            {
                Email = "test@example.com",
                Name = "Jan",
                LastName = "Kowalski",
                Phone = "123456789",
                StreetName = "Testowa",
                StreetNumber = "1",
                PostalCode = "00-000",
                City = "Warszawa"
            };
            context.Add(customer);
            context.SaveChanges();

            var repo = new CustomerRepository(context);

            var result = await repo.GetCustomerByIdAsync(customer.Id);

            Assert.NotNull(result);
            Assert.Equal("Jan", result.Name);
            Assert.Equal("Kowalski", result.LastName);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ThrowsIfNotFound()
        {
            using var context = GetInMemoryContext("GetCustomerByIdAsync_ThrowsIfNotFound");
            var repo = new CustomerRepository(context);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.GetCustomerByIdAsync(999));
        }
    }
}