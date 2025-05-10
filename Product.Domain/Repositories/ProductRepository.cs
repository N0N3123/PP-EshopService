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
        public async Task<ProductModel> AddProductAsync(ProductModel product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductModel> GetProductByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductModel>> GetProductsByCategoryAsync(string categoryName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(ProductModel product)
        {
            throw new NotImplementedException();
        }
    }
}
