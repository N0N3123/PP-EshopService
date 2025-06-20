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
        [HttpGet("getProductByName/{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Product name cannot be null or empty");
            }
            var product = await _productRepository.GetProductByNameAsync(name);
            return product != null ? Ok(product) : NotFound("Product not found");
        }
        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductModel product)
        {
            if (product == null || product.Id <= 0)
            {
                return BadRequest("Invalid product data");
            }
            await _productRepository.UpdateProductAsync(product);
            return NoContent();
        }
        [HttpDelete("deleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid product ID");
            }
            await _productRepository.DeleteProductAsync(id);
            return NoContent();
        }

    }
}
