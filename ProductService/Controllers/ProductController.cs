using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Domain.Models;
using Product.Domain.Repositories;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly HttpClient _httpClient;
        public ProductController(IProductRepository productRepository, HttpClient httpClient)
        {
            _productRepository = productRepository;
            _httpClient = httpClient;
        }
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel product)
        {
            if (product == null)
            {
                return BadRequest("Product cannot be null");
            }
            var addedProduct = await _productRepository.AddProductAsync(product);
            return product != null ? Ok(addedProduct) : BadRequest("Failed to add product");
        }
    }
}
