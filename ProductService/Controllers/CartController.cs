using Microsoft.AspNetCore.Mvc;
using Product.Domain.Models;
using Product.Domain.Repositories;
using System.Threading.Tasks;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        [HttpPost("addCart")]
        public async Task<IActionResult> AddCart([FromBody] CartModel cart)
        {
            if (cart == null)
                return BadRequest("Cart cannot be null");

            var createdCart = await _cartRepository.AddCartAsync(cart);
            return CreatedAtAction(nameof(GetCart), new { customerId = createdCart.CustomerId }, createdCart);
        }
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCart(int customerId)
        {
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
            return cart != null ? Ok(cart) : NotFound();
        }

        [HttpPost("{cartId}/addItem")]
        public async Task<IActionResult> AddItem(int cartId, [FromBody] CartItemModel item)
        {
            await _cartRepository.AddItemAsync(cartId, item);
            return Ok();
        }

        [HttpPut("{cartId}/updateItem")]
        public async Task<IActionResult> UpdateItem(int cartId, [FromBody] CartItemModel item)
        {
            await _cartRepository.UpdateItemAsync(cartId, item);
            return Ok();
        }

        [HttpDelete("{cartId}/removeItem/{itemId}")]
        public async Task<IActionResult> RemoveItem(int cartId, int itemId)
        {
            await _cartRepository.RemoveItemAsync(cartId, itemId);
            return Ok();
        }

        [HttpDelete("{cartId}/clear")]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            await _cartRepository.ClearCartAsync(cartId);
            return Ok();
        }
    }
}