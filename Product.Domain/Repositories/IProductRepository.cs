using Product.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<ProductModel> GetProductByNameAsync(string Name);
        Task<ProductModel> AddProductAsync(ProductModel product);
        Task UpdateProductAsync(ProductModel product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<ProductModel>> GetProductsByCategoryAsync(string categoryName);
    }
}
