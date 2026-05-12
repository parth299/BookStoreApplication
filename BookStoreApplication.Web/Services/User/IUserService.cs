using BookStoreApplication.Web.DTOs.Author;
using BookStoreApplication.Web.DTOs.User;
using BookStoreApplication.Web.Wrappers;
using UserEntity = BookStoreApplication.Web.Models.User;

namespace BookStoreApplication.Web.Services.User
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserEntity>>> GetUsersByRoleId(int roleId);

        Task<ApiResponse<List<UserEntity>>> GetUsersAsync();

        Task<ApiResponse<string>> ChangePasswordAsync(int id, string updatedPassword);

        Task<ApiResponse<UserEntity>> GetUserProfileAsync(int id);

        Task<ApiResponse<string>> RegisterAsync(RegisterUserDTO request);

        Task<ApiResponse<AuthResponseDTO>> LoginAsync(LoginUserDTO request);

        Task<ApiResponse<UserEntity>> UpdateUserAsync(UpdateUserDTO request);
    }
}