using BookStoreApplication.Web.DTOs;
using BookStoreApplication.Web.Services;
using Microsoft.AspNetCore.Mvc;
using BookStoreApplication.Web.Wrapper;

namespace BookStoreApplication.Web.Controllers;

[ApiController]
[Route("books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BookResponseDto>>> Create(BookCreateDto dto)
    {
        var created = await _bookService.CreateAsync(dto);
        var response = ApiResponse<BookResponseDto>.Success(StatusCodes.Status201Created, "Book created successfully.", created);
        return CreatedAtAction(nameof(GetByIsbn), new { isbn = created.Isbn }, response);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BookResponseDto>>>> GetAll()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(ApiResponse<IReadOnlyList<BookResponseDto>>.Success(StatusCodes.Status200OK, "Books fetched successfully.", books));
    }

    [HttpGet("{isbn}")]
    public async Task<ActionResult<ApiResponse<BookResponseDto>>> GetByIsbn(string isbn)
    {
        var book = await _bookService.GetByIsbnAsync(isbn);
        return Ok(ApiResponse<BookResponseDto>.Success(StatusCodes.Status200OK, "Book fetched successfully.", book));
    }

    [HttpPut("{isbn}")]
    public async Task<ActionResult<ApiResponse<BookResponseDto>>> Update(string isbn, BookUpdateDto dto)
    {
        var updated = await _bookService.UpdateAsync(isbn, dto);
        return Ok(ApiResponse<BookResponseDto>.Success(StatusCodes.Status200OK, "Book updated successfully.", updated));
    }

    [HttpGet("filter")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BookResponseDto>>>> Filter([FromQuery] BookFilterDto filter)
    {
        var books = await _bookService.FilterAsync(filter);
        return Ok(ApiResponse<IReadOnlyList<BookResponseDto>>.Success(StatusCodes.Status200OK, "Filtered books fetched successfully.", books));
    }

    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BookResponseDto>>>> Search([FromQuery] string title)
    {
        var books = await _bookService.SearchAsync(title);
        return Ok(ApiResponse<IReadOnlyList<BookResponseDto>>.Success(StatusCodes.Status200OK, "Book search completed successfully.", books));
    }

    [HttpGet("top-rated")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BookTopRatedDto>>>> GetTopRated([FromQuery] int count = 10)
    {
        var books = await _bookService.GetTopRatedAsync(count);
        return Ok(ApiResponse<IReadOnlyList<BookTopRatedDto>>.Success(StatusCodes.Status200OK, "Top rated books fetched successfully.", books));
    }
}
