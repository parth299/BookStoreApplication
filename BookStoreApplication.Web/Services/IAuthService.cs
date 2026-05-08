using BookStoreApplication.Web.DTOs;

namespace BookStoreApplication.Web.services
{
    public interface IAuthService
    {
        Task<RegisterUserDTO?> RegisterAsync (RegisterUserDTO request);
        Task<string?> LoginAsync(LoginUserDTO request);
    }
}