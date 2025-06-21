using Microsoft.EntityFrameworkCore;
using Product.Domain.Models;
using Product.Domain.Repositories;

namespace Product.Domain.Tests
{
    public class ProductRepositoryTests
    {
        private DataContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new DataContext(options);
        }

        [Fact]
        public async Task AddProductAsync_AddsProduct()
        {
            using var context = GetInMemoryContext("AddProductAsync_AddsProduct");
            var repo = new ProductRepository(context);
            var category = new CategoryModel { Name = "TestCat", Description = "desc" };
            context.Categories.Add(category);
            context.SaveChanges();

            var product = new ProductModel
            {
                Name = "Produkt1",
                Description = "Opis",
                Price = 10,
                Quantity = 5,
                Category = category,
                CategoryId = category.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await repo.AddProductAsync(product);

            Assert.Equal("Produkt1", result.Name);
            Assert.Single(context.Products);
        }

        [Fact]
        public async Task GetProductByNameAsync_ReturnsProduct_WhenExists()
        {
            using var context = GetInMemoryContext("GetProductByNameAsync_ReturnsProduct");
            var category = new CategoryModel { Name = "TestCat", Description = "desc" };
            context.Categories.Add(category);
            var product = new ProductModel
            {
                Name = "Produkt2",
                Description = "Opis2",
                Price = 20,
                Quantity = 2,
                Category = category,
                CategoryId = category.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Products.Add(product);
            context.SaveChanges();
            var repo = new ProductRepository(context);

            var result = await repo.GetProductByNameAsync("Produkt2");

            Assert.NotNull(result);
            Assert.Equal("Produkt2", result.Name);
        }

        [Fact]
        public async Task UpdateProductAsync_UpdatesProduct()
        {
            using var context = GetInMemoryContext("UpdateProductAsync_UpdatesProduct");
            var category = new CategoryModel { Name = "TestCat", Description = "desc" };
            context.Categories.Add(category);
            var product = new ProductModel
            {
                Name = "Produkt3",
                Description = "Opis3",
                Price = 30,
                Quantity = 3,
                Category = category,
                CategoryId = category.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Products.Add(product);
            context.SaveChanges();
            var repo = new ProductRepository(context);

            product.Description = "Nowy opis";
            await repo.UpdateProductAsync(product);

            var updated = context.Products.First(p => p.Id == product.Id);
            Assert.Equal("Nowy opis", updated.Description);
        }

        [Fact]
        public async Task DeleteProductAsync_RemovesProduct()
        {
            using var context = GetInMemoryContext("DeleteProductAsync_RemovesProduct");
            var category = new CategoryModel { Name = "TestCat", Description = "desc" };
            context.Categories.Add(category);
            var product = new ProductModel
            {
                Name = "Produkt4",
                Description = "Opis4",
                Price = 40,
                Quantity = 4,
                Category = category,
                CategoryId = category.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Products.Add(product);
            context.SaveChanges();
            var repo = new ProductRepository(context);

            await repo.DeleteProductAsync(product.Id);

            Assert.Empty(context.Products);
        }
    }
}