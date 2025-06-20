using Product.Domain.Models;
using System.Threading.Tasks;

namespace Product.Domain.Repositories
{
    public interface ICartRepository
    {
        Task<CartModel> GetCartByCustomerIdAsync(int customerId);
        Task AddItemAsync(int cartId, CartItemModel item);
        Task RemoveItemAsync(int cartId, int itemId);
        Task UpdateItemAsync(int cartId, CartItemModel item);
        Task ClearCartAsync(int cartId);
    }
}