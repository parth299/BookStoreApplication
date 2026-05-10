namespace BookStoreApplication.Web.Services
{
    public interface IReservationService
    {
        Task ReserveAsync(int inventoryId, int userId);

        Task ReleaseAsync(int inventoryId);

        bool IsReserved(int inventoryId);
    }
}
