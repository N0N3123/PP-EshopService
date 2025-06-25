using Eshop.Domain.Models;
using System.Threading.Tasks;

namespace Eshop.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task<CustomerModel> GetCustomerByIdAsync(int id);
        Task<CustomerModel> AddCustomerAsync(CustomerModel customer);
    }
}