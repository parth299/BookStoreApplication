using BookStoreApplication.Web.Data;
using BookStoreApplication.Web.DTOs.Book_Publisher;
using BookStoreApplication.Web.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApplication.Web.Repositories.Book_Publisher;
public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<Book?> GetBookByIsbnAsync(string isbn)
    {
        return _context.Books
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Isbn == isbn);
    }

    public Task<bool> ExistsAsync(string isbn)
    {
        return _context.Books.AnyAsync(b => b.Isbn == isbn);
    }

    public Task<bool> CategoryExistsAsync(int categoryId)
    {
        return _context.Categories.AnyAsync(c => c.CatId == categoryId);
    }

    public async Task<IReadOnlyList<Book>> GetAllBooksAsync()
    {
        return await _context.Books
            .Include(b => b.Publisher)
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Book>> FilterBooksAsync(BookFilterDto filter)
    {
        var query = _context.Books
            .Include(b => b.Publisher)
            .AsQueryable();

        if (filter.Category.HasValue)
        {
            query = query.Where(b => b.Category == filter.Category.Value);
        }

        if (filter.PublisherId.HasValue)
        {
            query = query.Where(b => b.PublisherId == filter.PublisherId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Edition))
        {
            var edition = filter.Edition.Trim();
            query = query.Where(b => b.Edition != null && b.Edition.Contains(edition));
        }

        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            var title = filter.Title.Trim();
            query = query.Where(b => b.Title.Contains(title));
        }

        return await query
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Book>> SearchByTitleAsync(string title)
    {
        return await _context.Books
            .Include(b => b.Publisher)
            .Where(b => b.Title.Contains(title))
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<BookTopRatedDto>> GetTopRatedAsync(int count)
    {
        return await _context.Books
            .Where(b => b.Bookreviews.Any(r => r.Rating.HasValue))
            .Select(b => new BookTopRatedDto
            {
                Isbn = b.Isbn,
                Title = b.Title,
                AverageRating = b.Bookreviews.Where(r => r.Rating.HasValue).Average(r => r.Rating!.Value),
                ReviewCount = b.Bookreviews.Count(r => r.Rating.HasValue)
            })
            .OrderByDescending(b => b.AverageRating)
            .ThenByDescending(b => b.ReviewCount)
            .Take(count)
            .ToListAsync();
    }

    public Task AddBookAsync(Book book)
    {
        return _context.Books.AddAsync(book).AsTask();
    }

    public void UpdateBook(Book book)
    {
        _context.Books.Update(book);
    }

    

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

}
