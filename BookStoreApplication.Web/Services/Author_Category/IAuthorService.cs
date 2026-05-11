using BookStoreApplication.Web.DTOs.Author;
using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Services.Author
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorResponseDTO>> GetAllAsync(PaginationFilter filter);
        Task<AuthorResponseDTO?> GetByIdAsync(int id);
        Task<AuthorWithBooksResponseDTO?> GetByIdWithBooksAsync(int id);
        Task<IEnumerable<AuthorResponseDTO>> SearchByNameAsync(string name);
        Task<IEnumerable<AuthorResponseDTO>> GetByCountryAsync(string country);
        Task<AuthorResponseDTO> CreateAsync(AuthorRequestDTO dto);
        Task<AuthorResponseDTO?> UpdateAsync(int id, AuthorRequestDTO dto);
        Task<AuthorResponseDTO?> UpdatePhotoAsync(int id, string photoPath);
    }
    public class PaginationFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public PaginationFilter()
    {
    }

    public PaginationFilter(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize > 50 ? 50 : pageSize;
    }
}
}