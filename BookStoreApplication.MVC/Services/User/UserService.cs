using BookStoreApplication.MVC.DTOs.Author;
using BookStoreApplication.MVC.DTOs.User;
using BookStoreApplication.MVC.Services.Api;
using BookStoreApplication.MVC.Wrappers;
using BookStoreApplication.MVC.Models.Enums;

namespace BookStoreApplication.MVC.Services.User;

public class UserService : IUserService
{
    private readonly ApiClient _api;
    public UserService(ApiClient api) => _api = api;

    public async Task<ApiResponse<List<UserResponseDTO>>> GetUsersByRoleId(int roleId)
        => ApiResponse<List<UserResponseDTO>>.SuccessResponse(await _api.GetListAsync<UserResponseDTO>($"api/Users/role/{roleId}"));

    public async Task<ApiResponse<List<UserResponseDTO>>> GetUsersAsync()
        => ApiResponse<List<UserResponseDTO>>.SuccessResponse(await _api.GetListAsync<UserResponseDTO>("api/Users"));

    public async Task<ApiResponse<string>> ChangePasswordAsync(int id, string updatedPassword)
        => await _api.PostAsync<ApiResponse<string>>($"api/Users/{id}", updatedPassword) ?? ApiResponse<string>.MessageResponse("Password update requested");

    public async Task<ApiResponse<UserResponseDTO>> GetUserProfileAsync(int id)
        => ApiResponse<UserResponseDTO>.SuccessResponse(await _api.GetAsync<UserResponseDTO>($"api/Users/{id}") ?? new UserResponseDTO { UserId = id, UserName = "User", FirstName = "Unknown", LastName = "User", RoleName = "User" });

    public async Task<ApiResponse<string>> RegisterAsync(RegisterUserDTO request)
    {
        request.RoleNumber = (int)UserRole.User;
        return await _api.PostAsync<ApiResponse<string>>("api/Users/register", request)
            ?? ApiResponse<string>.FailResponse("Registration API is unavailable. Start BookstoreApplication.Web first.");
    }

    public async Task<ApiResponse<AuthResponseDTO>> LoginAsync(LoginUserDTO request)
        => await _api.PostAsync<ApiResponse<AuthResponseDTO>>("api/Users/login", request) ?? ApiResponse<AuthResponseDTO>.FailResponse("Login API is unavailable. Start BookstoreApplication.Web first.");

    public async Task<ApiResponse<UserResponseDTO>> UpdateUserAsync(UpdateUserDTO request)
        => await _api.PutAsync<ApiResponse<UserResponseDTO>>("api/Users", request) ?? ApiResponse<UserResponseDTO>.FailResponse("Update API is unavailable.");
}
