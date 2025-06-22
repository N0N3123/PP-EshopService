using Microsoft.EntityFrameworkCore;
using Product.Domain.Models;
using Product.Domain.Repositories;
using Xunit;

namespace Product.Domain.IntegrationTests
{
    public class ProductRepositoryIntegrationTests
    {
        private DataContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new DataContext(options);
        }

        [Fact]
        public async Task AddProduct_And_GetProductByName_Works()
        {
            // Arrange
            using var context = GetInMemoryContext("Integration_AddProduct");
            var repo = new ProductRepository(context);

            var category = new CategoryModel { Name = "Electronics", Description = "Electronics category" };
            context.Categories.Add(category);
            context.SaveChanges();

            var product = new ProductModel
            {
                Name = "Laptop",
                Description = "A test laptop",
                Price = 999.99m,
                Quantity = 10,
                Category = category,
                CategoryId = category.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Act
            await repo.AddProductAsync(product);
            var retrieved = await repo.GetProductByNameAsync("Laptop");

            // Assert
            Assert.NotNull(retrieved);
            Assert.Equal("Laptop", retrieved.Name);
            Assert.Equal("Electronics", retrieved.Category.Name);
        }
    }
}