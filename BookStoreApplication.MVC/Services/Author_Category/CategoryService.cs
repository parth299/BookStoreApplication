using BookStoreApplication.MVC.DTOs.Category;
using BookStoreApplication.MVC.Services.Api;

namespace BookStoreApplication.MVC.Services.Author_Category;

public class CategoryService : ICategoryService
{
    private readonly ApiClient _api;
    public CategoryService(ApiClient api) => _api = api;
    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync() => await _api.GetListAsync<CategoryResponseDto>("api/Category");
    public async Task<CategoryResponseDto?> GetByIdAsync(int id) => await _api.GetAsync<CategoryResponseDto>($"api/Category/{id}");
    public async Task<CategoryResponseDto> CreateAsync(CategoryRequestDto dto) => await _api.PostAsync<CategoryResponseDto>("api/Category", dto) ?? new CategoryResponseDto { CatDescription = dto.CatDescription };
    public async Task<CategoryResponseDto?> UpdateAsync(int id, CategoryRequestDto dto) => await _api.PutAsync<CategoryResponseDto>($"api/Category/{id}", dto) ?? new CategoryResponseDto { CatId = id, CatDescription = dto.CatDescription };
    public async Task<bool> DeleteAsync(int id) => await _api.DeleteAsync($"api/Category/{id}");
}
