using BookStoreApplication.MVC.DTOs.Inventory;

namespace BookStoreApplication.MVC.Services.Inventory_Cart_Purchase
{
    public interface IShoppingCartService
    {
        Task AddAsync(CartItemDto dto);

        Task<IEnumerable<CartItemDto>>GetCartAsync(int userId);

        Task RemoveAsync(int userId, string isbn);

        Task ClearAsync(int userId);
    }
}
