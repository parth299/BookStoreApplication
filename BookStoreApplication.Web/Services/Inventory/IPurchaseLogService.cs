using BookStoreApplication.Web.DTOs.Inventory;

namespace BookStoreApplication.Web.Services.Inventory
{
    public interface IPurchaseLogService
    {
        Task CheckoutAsync(PurchaseDto dto);

    }
}
