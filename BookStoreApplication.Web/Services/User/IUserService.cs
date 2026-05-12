using BookStoreApplication.Web.DTOs.Author;
using BookStoreApplication.Web.DTOs.User;
using BookStoreApplication.Web.Models;
using UserEntity = BookStoreApplication.Web.Models.User;

namespace BookStoreApplication.Web.Services.User
{
    public interface IUserService
    {
        Task<RegisterUserDTO?> RegisterAsync (RegisterUserDTO request);
        Task<AuthResponseDTO> LoginAsync(LoginUserDTO request);
        Task<List<UserEntity>> GetUsersAsync();
        Task<UserEntity?> GetUserProfileAsync(int id);
        Task<UserEntity?> UpdateUserAsync(UpdateUserDTO user);
        Task<UserEntity?> ChangePasswordAsync(int id, string updated_password);
        Task<List<UserEntity>> GetUsersByRoleId(int roleId);
    }
}
