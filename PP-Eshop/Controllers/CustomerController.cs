using Microsoft.AspNetCore.Mvc;
using Eshop.Domain.Models;
using Eshop.Domain.Repositories;
using System.Threading.Tasks;

namespace PP_Eshop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpPost("addCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerModel customer)
        {
            if (customer == null)
                return BadRequest("Customer cannot be null");

            var createdCustomer = await _customerRepository.AddCustomerAsync(customer);
            return CreatedAtAction(nameof(AddCustomer), new { id = createdCustomer.Id }, createdCustomer);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(id);
                return Ok(customer);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}