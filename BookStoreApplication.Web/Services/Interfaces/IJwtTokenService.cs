using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Services
{
    public interface IJwtTokenService
    {
        Task<string> GenerateToken(User user, string role);
    }
}
