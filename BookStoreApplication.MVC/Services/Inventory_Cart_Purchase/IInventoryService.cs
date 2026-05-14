using BookStoreApplication.MVC.DTOs.Inventory;

namespace BookStoreApplication.MVC.Services.Inventory_Cart_Purchase
{
    public interface IInventoryService
    {
        Task<int> CreateAsync(CreateInventoryDto dto);
        Task PatchAsync(int id,InventoryPatchDto dto);
        Task<IEnumerable<InventoryDto>>GetAllAsync(string? isbn);
        Task<InventoryDto> GetByIdAsync(int id);
        Task UpdateAsync(int id,InventoryDto dto);
        Task<IEnumerable<InventoryDto>>GetAvailableInventoryAsync();
        Task<IEnumerable<LowStockDto>>GetLowStockAsync();
    }
}
