using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Models;
using Product.Domain.Repositories;
using ProductService.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace EShop.Domain.Tests
{
    public class CustomerControllerTests
    {
        private DataContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new DataContext(options);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsOk_WhenCustomerExists()
        {
            // Arrange
            using var context = GetInMemoryContext("GetCustomerById_ReturnsOk");
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
            context.Customers.Add(customer);
            context.SaveChanges();

            var repo = new CustomerRepository(context);
            var controller = new CustomerController(repo);

            // Act
            var result = await controller.GetCustomerById(customer.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCustomer = Assert.IsType<CustomerModel>(okResult.Value);
            Assert.Equal("Jan", returnedCustomer.Name);
            Assert.Equal("Kowalski", returnedCustomer.LastName);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            using var context = GetInMemoryContext("GetCustomerById_ReturnsNotFound");
            var repo = new CustomerRepository(context);
            var controller = new CustomerController(repo);

            // Act
            var result = await controller.GetCustomerById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}