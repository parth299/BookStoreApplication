namespace BookStoreApplication.Web.Repositories.Inventory_Cart_Purchase
{
    public interface IUserIsExistsRepository
    {
        Task<bool> UserExistsAsync(int userId);
    }
}
