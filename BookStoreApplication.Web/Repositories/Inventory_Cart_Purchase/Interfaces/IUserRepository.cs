namespace BookStoreApplication.Web.Repositories.Inventory_Cart_Purchase.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(int userId);
    }
}
