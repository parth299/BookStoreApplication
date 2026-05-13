using BookStoreApplication.MVC.DTOs.Inventory;

namespace BookStoreApplication.MVC.Services.Inventory_Cart_Purchase
{
    public interface IPurchaseLogService
    {
        Task CheckoutAsync(PurchaseDto dto);

    }
}
