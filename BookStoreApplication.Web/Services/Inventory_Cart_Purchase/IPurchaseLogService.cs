using BookStoreApplication.Web.DTOs.Inventory;

namespace BookStoreApplication.Web.Services.Inventory_Cart_Purchase
{
    public interface IPurchaseLogService
    {
        Task CheckoutAsync(PurchaseDto dto);

    }
}
