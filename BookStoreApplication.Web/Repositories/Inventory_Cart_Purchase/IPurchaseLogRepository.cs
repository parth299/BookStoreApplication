using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Repositories.Inventory_Cart_Purchase
{
    public interface IPurchaseLogRepository
    {
        Task AddAsync(Purchaselog log);

        Task<IEnumerable<Purchaselog>>GetByUserAsync(int userId);

        Task SaveAsync();
    }
}
