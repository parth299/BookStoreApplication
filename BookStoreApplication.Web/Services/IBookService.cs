using BookStoreApplication.Web.DTOs;

namespace BookStoreApplication.Web.Services;

public interface IBookService
{
    Task<BookResponseDto> CreateAsync(BookCreateDto dto);
    Task<IReadOnlyList<BookResponseDto>> GetAllBooksAsync();
    Task<BookResponseDto> GetByIsbnAsync(string isbn);
    Task<BookResponseDto> UpdateAsync(string isbn, BookUpdateDto dto);
    
    Task<IReadOnlyList<BookResponseDto>> FilterAsync(BookFilterDto filter);
    Task<IReadOnlyList<BookResponseDto>> SearchAsync(string title);
    Task<IReadOnlyList<BookTopRatedDto>> GetTopRatedAsync(int count);
}
