using BookStoreApplication.MVC.DTOs.Author;
using BookStoreApplication.MVC.Services.Api;

namespace BookStoreApplication.MVC.Services.Author_Category;

public class AuthorService : IAuthorService
{
    private readonly ApiClient _api;
    public AuthorService(ApiClient api) => _api = api;
    public async Task<IEnumerable<AuthorResponseDTO>> GetAllAsync(PaginationFilter filter) => await _api.GetListAsync<AuthorResponseDTO>($"api/Auth?pageNumber={filter.PageNumber}&pageSize={filter.PageSize}");
    public async Task<AuthorResponseDTO?> GetByIdAsync(int id) => await _api.GetAsync<AuthorResponseDTO>($"api/Auth/{id}");
    public async Task<AuthorWithBooksResponseDTO?> GetByIdWithBooksAsync(int id) => await _api.GetAsync<AuthorWithBooksResponseDTO>($"api/Auth/{id}/books") ?? new AuthorWithBooksResponseDTO { AuthorId = id, FirstName = "Author", LastName = "Not found" };
    public async Task<IEnumerable<AuthorResponseDTO>> SearchByNameAsync(string name) => await _api.GetListAsync<AuthorResponseDTO>($"api/Auth/search?name={Uri.EscapeDataString(name)}");
    public async Task<IEnumerable<AuthorResponseDTO>> GetByCountryAsync(string country) => await _api.GetListAsync<AuthorResponseDTO>($"api/Auth/country/{Uri.EscapeDataString(country)}");
    public async Task<AuthorResponseDTO> CreateAsync(AuthorRequestDTO dto) => await _api.PostAsync<AuthorResponseDTO>("api/Auth", dto) ?? new AuthorResponseDTO { FirstName = dto.FirstName, LastName = dto.LastName, Photo = dto.Photo };
    public async Task<AuthorResponseDTO?> UpdateAsync(int id, AuthorRequestDTO dto) => await _api.PutAsync<AuthorResponseDTO>($"api/Auth/{id}", dto) ?? new AuthorResponseDTO { AuthorId = id, FirstName = dto.FirstName, LastName = dto.LastName, Photo = dto.Photo };
    public async Task<AuthorResponseDTO?> UpdatePhotoAsync(int id, string photoPath) => await _api.PostAsync<AuthorResponseDTO>($"api/Auth/{id}/photo", new { photoPath });
}
