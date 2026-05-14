using BookStoreApplication.MVC.DTOs.Inventory;
using BookStoreApplication.MVC.Services.Api;

namespace BookStoreApplication.MVC.Services.Inventory_Cart_Purchase;

public class PurchaseLogService : IPurchaseLogService
{
    private readonly ApiClient _api;
    public PurchaseLogService(ApiClient api) => _api = api;
    public async Task CheckoutAsync(PurchaseDto dto) => await _api.PostAsync<object>("api/PurchaseLog", dto);
}
