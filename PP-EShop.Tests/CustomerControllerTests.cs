using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshop.Domain.Models;
using Eshop.Domain.Repositories;
using PP_Eshop.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace EShop.Domain.Tests
{
    public class CustomerControllerTests
    {
        private EshopDataContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<EshopDataContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new EshopDataContext(options);
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