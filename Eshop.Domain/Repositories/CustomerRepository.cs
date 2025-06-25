using Eshop.Domain.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Domain.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly EshopDataContext _context;
        public CustomerRepository(EshopDataContext context)
        {
            _context = context;
        }
        public async Task<CustomerModel> AddCustomerAsync(CustomerModel customer)
        {
            _context.Set<CustomerModel>().Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
        public async Task<CustomerModel> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Set<CustomerModel>().FindAsync(id);
            if (customer == null)
                throw new KeyNotFoundException("Customer not found");
            return customer;
        }
    }
}