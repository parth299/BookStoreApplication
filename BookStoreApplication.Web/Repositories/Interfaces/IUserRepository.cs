namespace BookStoreApplication.Web.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(int userId);
    }
}
