using Product.Domain.Models;
using System.Threading.Tasks;

namespace Product.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task<CustomerModel> GetCustomerByIdAsync(int id);
    }
}