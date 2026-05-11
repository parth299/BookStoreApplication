using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Repositories.CartPurchaseInventory
{
    public interface IShoppingCartRepository
    {
        Task AddAsync(Shoppingcart cart);

        Task<IEnumerable<Shoppingcart>>GetCartAsync(int userId);

        Task<Shoppingcart?> GetItemAsync(int userId,string isbn);

        Task RemoveAsync(Shoppingcart cart);

        Task ClearAsync(int userId);

        Task SaveAsync();
    }
}
