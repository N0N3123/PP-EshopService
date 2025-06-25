using Product.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<ProductModel> AddProductAsync(ProductModel product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public Task DeleteProductAsync(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                return _context.SaveChangesAsync();
            }
            throw new KeyNotFoundException("Product not found");

        }

        public Task<ProductModel> GetProductByNameAsync(string Name)
        {
            var product = _context.Products.FirstOrDefault(p => p.Name.ToUpper() == Name.ToUpper());
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            return Task.FromResult(product);
        }

        public Task<IEnumerable<ProductModel>> GetProductsByCategoryAsync(string categoryName)
        {
            var products = _context.Products.Where(p => p.Category.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!products.Any())
            {
                throw new KeyNotFoundException("No products found for the specified category");
            }
            return Task.FromResult<IEnumerable<ProductModel>>(products);
        }

        public Task UpdateProductAsync(ProductModel product)
        {
            var existingProduct = _context.Products.Find(product.Id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            existingProduct.Category = product.Category;
            existingProduct.UpdatedAt = DateTime.UtcNow; 
            return _context.SaveChangesAsync();
        }
    }
}
