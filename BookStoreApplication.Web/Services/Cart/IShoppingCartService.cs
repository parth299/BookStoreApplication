using BookStoreApplication.Web.DTOs.Inventory;

namespace BookStoreApplication.Web.Services.Cart
{
    public interface IShoppingCartService
    {
        Task AddAsync(CartItemDto dto);

        Task<IEnumerable<CartItemDto>>
            GetCartAsync(int userId);

        Task RemoveAsync(
            int userId,
            string isbn);

        Task ClearAsync(int userId);
    }
}
