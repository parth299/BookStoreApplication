using BookStoreApplication.MVC.Models;
using BookStoreApplication.MVC.Models.Common;

namespace BookStoreApplication.MVC.Services.Interfaces;

public interface IUserService
{
    Task<ApiResponse<string>?> RegisterAsync(RegisterRequestDto request);

    Task<ApiResponse<AuthResponseDto>?> LoginAsync(LoginRequestDto request);

    Task<ApiResponse<UserDto>?> GetUserProfileAsync(int id);

    Task<ApiResponse<UserDto>?> UpdateUserAsync(UserDto request);

    Task<ApiResponse<List<UserDto>>?> GetUsersAsync();

    Task<ApiResponse<string>?> ChangePasswordAsync(int id, string newPassword);
}