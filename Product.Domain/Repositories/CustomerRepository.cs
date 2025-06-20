using Product.Domain.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Product.Domain.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _context;
        public CustomerRepository(DataContext context)
        {
            _context = context;
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