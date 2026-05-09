using BookStoreApplication.Web.DTOs.Inventory;

namespace BookStoreApplication.Web.Services.Inventory
{
    public interface IInventoryService
    {
        Task<int> CreateAsync(InventoryDto dto);
        Task PatchAsync(int id,InventoryPatchDto dto);

        Task<IEnumerable<InventoryDto>>GetAllAsync(string? isbn);

        Task<InventoryDto> GetByIdAsync(int id);

        Task UpdateAsync(int id,InventoryDto dto);
        Task<IEnumerable<InventoryDto>>GetAvailableInventoryAsync();

        Task<IEnumerable<LowStockDto>>GetLowStockAsync();
    }
}
