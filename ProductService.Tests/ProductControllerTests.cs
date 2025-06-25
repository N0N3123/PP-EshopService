using Microsoft.AspNetCore.Mvc;
using Product.Domain.Models;
using Product.Domain.Repositories;
using ProductService.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.Tests
{
    public class FakeProductRepository : IProductRepository
    {
        public List<ProductModel> Products { get; set; } = new();

        public Task<ProductModel> AddProductAsync(ProductModel product)
        {
            product.Id = Products.Count + 1;
            Products.Add(product);
            return Task.FromResult(product);
        }

        public Task DeleteProductAsync(int id)
        {
            Products.RemoveAll(p => p.Id == id);
            return Task.CompletedTask;
        }

        public Task<ProductModel> GetProductByNameAsync(string name)
        {
            var product = Products.Find(p => p.Name == name);
            if (product == null)
                throw new KeyNotFoundException();
            return Task.FromResult(product);
        }

        public Task<IEnumerable<ProductModel>> GetProductsByCategoryAsync(string categoryName)
        {
            var result = Products.FindAll(p => p.Category.Name == categoryName);
            return Task.FromResult<IEnumerable<ProductModel>>(result);
        }

        public Task UpdateProductAsync(ProductModel product)
        {
            var idx = Products.FindIndex(p => p.Id == product.Id);
            if (idx >= 0)
                Products[idx] = product;
            return Task.CompletedTask;
        }
    }

    public class ProductControllerTests
    {
        private ProductController GetControllerWithFakeRepo()
        {
            var repo = new FakeProductRepository();
            var httpClient = new System.Net.Http.HttpClient();
            return new ProductController(repo, httpClient);
        }

        [Fact]
        public async Task AddProduct_ReturnsOk()
        {
            var controller = GetControllerWithFakeRepo();
            var product = new ProductModel
            {
                Name = "Test",
                Description = "Opis",
                Price = 10,
                Quantity = 1,
                Category = new CategoryModel { Name = "Cat", Description = "Desc" },
                CategoryId = 1,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            };

            var result = await controller.AddProduct(product);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<ProductModel>(okResult.Value);
            Assert.Equal("Test", returnedProduct.Name);
        }

        [Fact]
        public async Task GetProductByName_ReturnsOk_WhenProductExists()
        {
            var repo = new FakeProductRepository();
            repo.Products.Add(new ProductModel
            {
                Id = 1,
                Name = "Test",
                Description = "Opis",
                Price = 10,
                Quantity = 1,
                Category = new CategoryModel { Name = "Cat", Description = "Desc" },
                CategoryId = 1,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            });
            var httpClient = new System.Net.Http.HttpClient();
            var controller = new ProductController(repo, httpClient);

            var result = await controller.GetProductByName("Test");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<ProductModel>(okResult.Value);
            Assert.Equal("Test", returnedProduct.Name);
        }

        [Fact]
        public async Task GetProductByName_ReturnsNotFound_WhenProductDoesNotExist()
        {
            var controller = GetControllerWithFakeRepo();

            var result = await controller.GetProductByName("NieMa");

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent()
        {
            var repo = new FakeProductRepository();
            repo.Products.Add(new ProductModel
            {
                Id = 1,
                Name = "Test",
                Description = "Opis",
                Price = 10,
                Quantity = 1,
                Category = new CategoryModel { Name = "Cat", Description = "Desc" },
                CategoryId = 1,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            });
            var httpClient = new System.Net.Http.HttpClient();
            var controller = new ProductController(repo, httpClient);

            var result = await controller.DeleteProduct(1);

            Assert.IsType<NoContentResult>(result);
        }
    }
}