using BookStoreApplication.Web.DTOs.Book_Publisher;
using BookStoreApplication.Web.Services.Book_Publisher;
using BookStoreApplication.Web.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers.Books_Publisher
{
    [ApiController]
    [Route("books")]
    [Authorize(Roles = "User,RegisteredUser,Manager,StoreOwner,Admin")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager,StoreOwner,Admin")]
        public async Task<ActionResult<ApiResponse<BookResponseDto>>> Create(BookCreateDto dto)
        {
            var created = await _bookService.CreateAsync(dto);

            var response = ApiResponse<BookResponseDto>.SuccessResponse(
                created,
                "Book created successfully.",
                StatusCodes.Status201Created);

            return CreatedAtAction(
                nameof(GetByIsbn),
                new { isbn = created.Isbn },
                response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<BookResponseDto>>>> GetAll()
        {
            var books = await _bookService.GetAllBooksAsync();

            return Ok(
                ApiResponse<IReadOnlyList<BookResponseDto>>.SuccessResponse(
                    books,
                    "Books fetched successfully.",
                    StatusCodes.Status200OK));
        }

        [HttpGet("{isbn}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<BookResponseDto>>> GetByIsbn(string isbn)
        {
            var book = await _bookService.GetByIsbnAsync(isbn);

            return Ok(
                ApiResponse<BookResponseDto>.SuccessResponse(
                    book,
                    "Book fetched successfully.",
                    StatusCodes.Status200OK));
        }

        [HttpPut("{isbn}")]
        [Authorize(Roles = "Manager,StoreOwner,Admin")]
        public async Task<ActionResult<ApiResponse<BookResponseDto>>> Update(
            string isbn,
            BookUpdateDto dto)
        {
            var updated = await _bookService.UpdateAsync(isbn, dto);

            return Ok(
                ApiResponse<BookResponseDto>.SuccessResponse(
                    updated,
                    "Book updated successfully.",
                    StatusCodes.Status200OK));
        }

        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<BookResponseDto>>>> Filter(
            [FromQuery] BookFilterDto filter)
        {
            var books = await _bookService.FilterAsync(filter);

            return Ok(
                ApiResponse<IReadOnlyList<BookResponseDto>>.SuccessResponse(
                    books,
                    "Filtered books fetched successfully.",
                    StatusCodes.Status200OK));
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<BookResponseDto>>>> Search(
            [FromQuery] string title)
        {
            var books = await _bookService.SearchAsync(title);

            return Ok(
                ApiResponse<IReadOnlyList<BookResponseDto>>.SuccessResponse(
                    books,
                    "Book search completed successfully.",
                    StatusCodes.Status200OK));
        }

        [HttpGet("top-rated")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<BookTopRatedDto>>>> GetTopRated(
            [FromQuery] int count = 10)
        {
            var books = await _bookService.GetTopRatedAsync(count);

            return Ok(
                ApiResponse<IReadOnlyList<BookTopRatedDto>>.SuccessResponse(
                    books,
                    "Top rated books fetched successfully.",
                    StatusCodes.Status200OK));
        }
    }
}