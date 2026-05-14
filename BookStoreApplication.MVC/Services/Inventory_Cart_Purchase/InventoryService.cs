using BookStoreApplication.MVC.DTOs.Inventory;
using BookStoreApplication.MVC.Services.Api;

namespace BookStoreApplication.MVC.Services.Inventory_Cart_Purchase;

public class InventoryService : IInventoryService
{
    private readonly ApiClient _api;
    public InventoryService(ApiClient api) => _api = api;
    public async Task<int> CreateAsync(CreateInventoryDto dto)
    {
        var id = await _api.PostAsync<int>("api/Inventory", dto);
        return id;
    }
    public async Task PatchAsync(int id, InventoryPatchDto dto) => await _api.PatchAsync($"api/Inventory/{id}", dto);
    public async Task<IEnumerable<InventoryDto>> GetAllAsync(string? isbn)
    {
        var url = string.IsNullOrWhiteSpace(isbn) ? "api/Inventory" : $"api/Inventory?isbn={Uri.EscapeDataString(isbn)}";
        return await _api.GetListAsync<InventoryDto>(url);
    }
    public async Task<InventoryDto> GetByIdAsync(int id) => await _api.GetAsync<InventoryDto>($"api/Inventory/{id}") ?? new InventoryDto { InventoryId = id, ISBN = string.Empty };
    public async Task UpdateAsync(int id, InventoryDto dto) => await _api.PutAsync<InventoryDto>($"api/Inventory/{id}", dto);
    public async Task<IEnumerable<InventoryDto>> GetAvailableInventoryAsync() => await _api.GetListAsync<InventoryDto>("api/Inventory/available");
    public async Task<IEnumerable<LowStockDto>> GetLowStockAsync() => await _api.GetListAsync<LowStockDto>("api/Inventory/low-stock");
}
