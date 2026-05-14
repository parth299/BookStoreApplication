using BookStoreApplication.MVC.DTOs.Inventory;
using BookStoreApplication.MVC.Services.Api;

namespace BookStoreApplication.MVC.Services.Inventory_Cart_Purchase;

public class ShoppingCartService : IShoppingCartService
{
    private readonly ApiClient _api;
    public ShoppingCartService(ApiClient api) => _api = api;
    public async Task AddAsync(CartItemDto dto) => await _api.PostAsync<object>("api/ShoppingCart", dto);
    public async Task<IEnumerable<CartItemDto>> GetCartAsync(int userId) => await _api.GetListAsync<CartItemDto>($"api/ShoppingCart/{userId}");
    public async Task RemoveAsync(int userId, string isbn) => await _api.DeleteAsync($"api/ShoppingCart/{userId}/{Uri.EscapeDataString(isbn)}");
    public async Task ClearAsync(int userId) => await _api.DeleteAsync($"api/ShoppingCart/{userId}");
}
