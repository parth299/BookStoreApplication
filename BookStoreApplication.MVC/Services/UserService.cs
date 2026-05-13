using System.Net.Http.Headers;
using System.Net.Http.Json;
using BookStoreApplication.MVC.Models;
using BookStoreApplication.MVC.Models.Common;
using BookStoreApplication.MVC.Services.Interfaces;

namespace BookStoreApplication.MVC.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    private void AddAuthHeader()
    {
        var token = _httpContextAccessor.HttpContext?.User.FindFirst("JWToken")?.Value;
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<ApiResponse<string>?> RegisterAsync(RegisterRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users/register", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
    }

    public async Task<ApiResponse<AuthResponseDto>?> LoginAsync(LoginRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users/login", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDto>>();
    }

    public async Task<ApiResponse<UserDto>?> GetUserProfileAsync(int id)
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync($"api/users/{id}");
        return await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
    }

    public async Task<ApiResponse<UserDto>?> UpdateUserAsync(UserDto request)
    {
        AddAuthHeader();
        var response = await _httpClient.PutAsJsonAsync("api/users", request);
        return await response.Content.ReadFromJsonAsync<ApiResponse<UserDto>>();
    }

    public async Task<ApiResponse<List<UserDto>>?> GetUsersAsync()
    {
        AddAuthHeader();
        var response = await _httpClient.GetAsync("api/users");
        return await response.Content.ReadFromJsonAsync<ApiResponse<List<UserDto>>>();
    }

    public async Task<ApiResponse<string>?> ChangePasswordAsync(int id, string newPassword)
    {
        AddAuthHeader();
        var response = await _httpClient.PostAsJsonAsync($"api/users/{id}", newPassword);
        return await response.Content.ReadFromJsonAsync<ApiResponse<string>>();
    }
}