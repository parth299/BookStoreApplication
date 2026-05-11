namespace BookStoreApplication.Web.Repositories.CartPurchaseInventory
{
    public interface IUserIsExistsRepository
    {
        Task<bool> UserExistsAsync(int userId);
    }
}
