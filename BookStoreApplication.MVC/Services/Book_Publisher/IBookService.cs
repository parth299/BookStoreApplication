using BookStoreApplication.MVC.DTOs.Book_Publisher;

namespace BookStoreApplication.MVC.Services.Book_Publisher;

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
