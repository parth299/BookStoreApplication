using BookStoreApplication.MVC.DTOs.Author;
using BookStoreApplication.MVC.DTOs.User;
using BookStoreApplication.MVC.Wrappers;

namespace BookStoreApplication.MVC.Services.User
{
    public interface IUserService
    {
        Task<ApiResponse<List<UserResponseDTO>>> GetUsersByRoleId(int roleId);

        Task<ApiResponse<List<UserResponseDTO>>> GetUsersAsync();

        Task<ApiResponse<string>> ChangePasswordAsync(int id, string updatedPassword);

        Task<ApiResponse<UserResponseDTO>> GetUserProfileAsync(int id);

        Task<ApiResponse<string>> RegisterAsync(RegisterUserDTO request);

        Task<ApiResponse<AuthResponseDTO>> LoginAsync(LoginUserDTO request);

        Task<ApiResponse<UserResponseDTO>> UpdateUserAsync(UpdateUserDTO request);
    }
}