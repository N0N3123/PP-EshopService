using Product.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Product.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryModel>> GetAllAsync();
        Task<CategoryModel> GetByNameAsync(string name);
        Task<CategoryModel> AddAsync(CategoryModel category);
        Task UpdateAsync(CategoryModel category);
        Task DeleteAsync(string name);
    }
}