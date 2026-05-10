using BookStoreApplication.Web.DTOs;

namespace BookStoreApplication.Web.Services
{
    public interface IShoppingCartService
    {
        Task AddAsync(CartItemDto dto);

        Task<IEnumerable<CartItemDto>>GetCartAsync(int userId);

        Task RemoveAsync(int userId, string isbn);

        Task ClearAsync(int userId);
    }
}
