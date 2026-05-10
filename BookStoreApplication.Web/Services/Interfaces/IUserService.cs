using BookStoreApplication.Web.DTOs.Author;
using BookStoreApplication.Web.DTOs.User;
using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.services
{
    public interface IUserService
    {
        Task<RegisterUserDTO?> RegisterAsync (RegisterUserDTO request);
        Task<AuthResponseDTO> LoginAsync(LoginUserDTO request);
        Task<List<User>> GetUsersAsync();
        Task<User?> GetUserProfileAsync(int id);
        Task<User?> UpdateUserAsync(UpdateUserDTO user);
        Task<User?> ChangePasswordAsync(int id, string updated_password);
        Task<List<User>> GetUsersByRoleId(int roleId);
    }
}