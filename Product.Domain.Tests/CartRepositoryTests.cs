using Microsoft.EntityFrameworkCore;
using Product.Domain.Models;
using Product.Domain.Repositories;
using Xunit;

namespace Product.Domain.Tests
{
    public class CartRepositoryTests
    {
        private DataContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new DataContext(options);
        }

        [Fact]
        public async Task GetCartByCustomerIdAsync_ReturnsCart()
        {
            using var context = GetInMemoryContext("GetCartByCustomerIdAsync_ReturnsCart");
            var cart = new CartModel { CustomerId = 123 };
            context.Carts.Add(cart);
            context.SaveChanges();

            var repo = new CartRepository(context);

            var result = await repo.GetCartByCustomerIdAsync(123);

            Assert.NotNull(result);
            Assert.Equal(123, result.CustomerId);
        }

        [Fact]
        public async Task AddItemAsync_AddsItemToCart()
        {
            using var context = GetInMemoryContext("AddItemAsync_AddsItemToCart");
            var cart = new CartModel { CustomerId = 1 };
            context.Carts.Add(cart);
            context.SaveChanges();

            var repo = new CartRepository(context);
            var item = new CartItemModel { ProductId = 10, Quantity = 2 };

            await repo.AddItemAsync(cart.Id, item);

            var updatedCart = context.Carts.Include(c => c.Items).First(c => c.Id == cart.Id);
            Assert.Single(updatedCart.Items);
            Assert.Equal(10, updatedCart.Items[0].ProductId);
        }

        [Fact]
        public async Task RemoveItemAsync_RemovesItemFromCart()
        {
            using var context = GetInMemoryContext("RemoveItemAsync_RemovesItemFromCart");
            var cart = new CartModel { CustomerId = 2, Items = new List<CartItemModel> { new CartItemModel { ProductId = 20, Quantity = 1 } } };
            context.Carts.Add(cart);
            context.SaveChanges();

            var repo = new CartRepository(context);
            var itemId = cart.Items[0].Id;

            await repo.RemoveItemAsync(cart.Id, itemId);

            var updatedCart = context.Carts.Include(c => c.Items).First(c => c.Id == cart.Id);
            Assert.Empty(updatedCart.Items);
        }

        [Fact]
        public async Task UpdateItemAsync_UpdatesItemInCart()
        {
            using var context = GetInMemoryContext("UpdateItemAsync_UpdatesItemInCart");
            var item = new CartItemModel { ProductId = 30, Quantity = 1 };
            var cart = new CartModel { CustomerId = 3, Items = new List<CartItemModel> { item } };
            context.Carts.Add(cart);
            context.SaveChanges();

            var repo = new CartRepository(context);
            var updatedItem = new CartItemModel { Id = item.Id, ProductId = 31, Quantity = 5 };

            await repo.UpdateItemAsync(cart.Id, updatedItem);

            var updatedCart = context.Carts.Include(c => c.Items).First(c => c.Id == cart.Id);
            Assert.Single(updatedCart.Items);
            Assert.Equal(31, updatedCart.Items[0].ProductId);
            Assert.Equal(5, updatedCart.Items[0].Quantity);
        }

        [Fact]
        public async Task ClearCartAsync_RemovesAllItems()
        {
            using var context = GetInMemoryContext("ClearCartAsync_RemovesAllItems");
            var cart = new CartModel
            {
                CustomerId = 4,
                Items = new List<CartItemModel>
                {
                    new CartItemModel { ProductId = 1, Quantity = 1 },
                    new CartItemModel { ProductId = 2, Quantity = 2 }
                }
            };
            context.Carts.Add(cart);
            context.SaveChanges();

            var repo = new CartRepository(context);

            await repo.ClearCartAsync(cart.Id);

            var updatedCart = context.Carts.Include(c => c.Items).First(c => c.Id == cart.Id);
            Assert.Empty(updatedCart.Items);
        }

        [Fact]
        public async Task GetCartByCustomerIdAsync_ThrowsIfNotFound()
        {
            using var context = GetInMemoryContext("GetCartByCustomerIdAsync_ThrowsIfNotFound");
            var repo = new CartRepository(context);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.GetCartByCustomerIdAsync(999));
        }
    }
}