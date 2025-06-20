
namespace Product.Domain.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<CartItemModel> Items { get; set; } = new();
    }
}