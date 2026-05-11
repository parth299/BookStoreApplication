using BookStoreApplication.Web.Models;
using UserEntity = BookStoreApplication.Web.Models.User;

namespace BookStoreApplication.Web.Services
{
    public interface IJwtTokenService
    {
        Task<string> GenerateToken(UserEntity user, string role);
    }
}
