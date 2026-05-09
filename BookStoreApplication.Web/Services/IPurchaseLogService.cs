using BookStoreApplication.Web.DTOs;

namespace BookStoreApplication.Web.Services
{
    public interface IPurchaseLogService
    {
        Task CheckoutAsync(PurchaseDto dto);

    }
}
