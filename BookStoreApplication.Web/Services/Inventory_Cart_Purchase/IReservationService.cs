namespace BookStoreApplication.Web.Services.Inventory_Cart_Purchase
{
    public interface IReservationService
    {
        Task ReserveAsync(int inventoryId, int userId);

        Task ReleaseAsync(int inventoryId);
        bool IsReservedByUser(int inventoryId, int userId);
        bool IsReserved(int inventoryId);

    }
}
