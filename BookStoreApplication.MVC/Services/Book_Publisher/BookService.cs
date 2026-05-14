using BookStoreApplication.MVC.DTOs.Book_Publisher;
using BookStoreApplication.MVC.Services.Api;

namespace BookStoreApplication.MVC.Services.Book_Publisher;

public class BookService : IBookService
{
    private readonly ApiClient _api;
    public BookService(ApiClient api) => _api = api;

    public async Task<BookResponseDto> CreateAsync(BookCreateDto dto)
        => await _api.PostAsync<BookResponseDto>("books", dto) ?? new BookResponseDto { Isbn = dto.Isbn, Title = dto.Title, Category = dto.Category, PublisherId = dto.PublisherId, Description = dto.Description, Edition = dto.Edition };

    public async Task<IReadOnlyList<BookResponseDto>> GetAllBooksAsync()
        => await _api.GetListAsync<BookResponseDto>("books");

    public async Task<BookResponseDto> GetByIsbnAsync(string isbn)
        => await _api.GetAsync<BookResponseDto>($"books/{Uri.EscapeDataString(isbn)}") ?? new BookResponseDto { Isbn = isbn, Title = "Book not found" };

    public async Task<BookResponseDto> UpdateAsync(string isbn, BookUpdateDto dto)
        => await _api.PutAsync<BookResponseDto>($"books/{Uri.EscapeDataString(isbn)}", dto) ?? new BookResponseDto { Isbn = isbn, Title = dto.Title, Description = dto.Description, Edition = dto.Edition, Category = dto.Category, PublisherId = dto.PublisherId };

    public async Task<IReadOnlyList<BookResponseDto>> FilterAsync(BookFilterDto filter)
    {
        var qs = new List<string>();
        if (filter.Category.HasValue) qs.Add($"category={filter.Category.Value}");
        if (filter.PublisherId.HasValue) qs.Add($"publisherId={filter.PublisherId.Value}");
        if (!string.IsNullOrWhiteSpace(filter.Edition)) qs.Add($"edition={Uri.EscapeDataString(filter.Edition)}");
        if (!string.IsNullOrWhiteSpace(filter.Title)) qs.Add($"title={Uri.EscapeDataString(filter.Title)}");
        var url = qs.Count == 0 ? "books" : "books/filter?" + string.Join("&", qs);
        return await _api.GetListAsync<BookResponseDto>(url);
    }

    public async Task<IReadOnlyList<BookResponseDto>> SearchAsync(string title)
        => await _api.GetListAsync<BookResponseDto>($"books/search?title={Uri.EscapeDataString(title)}");

    public async Task<IReadOnlyList<BookTopRatedDto>> GetTopRatedAsync(int count)
        => await _api.GetListAsync<BookTopRatedDto>($"books/top-rated?count={count}");
}
