using BookStoreApplication.Web.DTOs.Book_Publisher;
using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Repositories.Book_Publisher;

public interface IBookRepository
{
    Task<Book?> GetBookByIsbnAsync(string isbn);
    Task<bool> ExistsAsync(string isbn);
    Task<IReadOnlyList<Book>> GetAllBooksAsync();
    Task<IReadOnlyList<Book>> FilterBooksAsync(BookFilterDto filter);
    Task<bool> CategoryExistsAsync(int categoryId);
    Task<IReadOnlyList<Book>> SearchByTitleAsync(string title);
    Task<IReadOnlyList<BookTopRatedDto>> GetTopRatedAsync(int count);
    Task AddBookAsync(Book book);
    void UpdateBook(Book book);
    Task SaveChangesAsync();
}
