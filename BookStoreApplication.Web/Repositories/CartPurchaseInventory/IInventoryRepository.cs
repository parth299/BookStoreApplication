using BookStoreApplication.Web.DTOs.Inventory;
using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Repositories.CartPurchaseInventory
{
    public interface IInventoryRepository
    {
        Task AddAsync(Inventory inventory);

        Task<IEnumerable<Inventory>> GetAllAsync();

        Task<IEnumerable<Inventory>> FilterAsync(string? isbn);

        Task<Inventory?> GetByIdAsync(int id);

        Task<Inventory?> GetAvailableByISBNAsync(string isbn);

        Task<IEnumerable<Inventory>> GetAvailableInventoryAsync();

        Task<IEnumerable<LowStockDto>> GetLowStockAsync();

        void Update(Inventory inventory);

        Task SaveAsync();


    }
}
