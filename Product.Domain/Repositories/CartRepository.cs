using Microsoft.EntityFrameworkCore;
using Product.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Product.Domain.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DataContext _context;

        public CartRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<CartModel> GetCartByCustomerIdAsync(int customerId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found");
            return cart;
        }

        public async Task AddItemAsync(int cartId, CartItemModel item)
        {
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found");

            cart.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(int cartId, int itemId)
        {
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found");

            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                throw new KeyNotFoundException("Item not found");

            cart.Items.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(int cartId, CartItemModel item)
        {
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found");

            var existingItem = cart.Items.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem == null)
                throw new KeyNotFoundException("Item not found");

            existingItem.ProductId = item.ProductId;
            existingItem.Quantity = item.Quantity;
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(int cartId)
        {
            var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found");

            cart.Items.Clear();
            await _context.SaveChangesAsync();
        }
    }
}