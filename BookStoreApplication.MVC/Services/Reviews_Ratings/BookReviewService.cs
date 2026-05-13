using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;
using BookStoreApplication.MVC.Services.Api;

namespace BookStoreApplication.MVC.Services.Reviews_Ratings;

public class BookReviewService : IBookReviewService
{
    private readonly ApiClient _api;
    public BookReviewService(ApiClient api) => _api = api;
    public async Task<CreateReviewResponseDto> CreateReviewAsync(CreateReviewRequestDto request) => await _api.PostAsync<CreateReviewResponseDto>("api/reviews", request) ?? new CreateReviewResponseDto { Isbn = request.Isbn, ReviewerId = request.ReviewerId, ReviewerName = request.Name, EmployedBy = request.EmployedBy, Rating = request.Rating, Comments = request.Comments, Message = "Review submitted" };
    public async Task<List<BookReviewDto>> GetAllReviewsAsync(int page, int pageSize) => await _api.GetListAsync<BookReviewDto>($"api/reviews?page={page}&pageSize={pageSize}");
    public async Task<List<BookReviewDto>> GetBookReviewsByIsbnAsync(string isbn) => await _api.GetListAsync<BookReviewDto>($"api/reviews/book/{Uri.EscapeDataString(isbn)}");
    public async Task<List<BookReviewDto>> GetBookReviewsByReviewerIdAsync(int reviewerId) => await _api.GetListAsync<BookReviewDto>($"api/reviews/reviewer/{reviewerId}");
    public async Task<BookAverageRatingDto> GetAverageRatingByIsbnAsync(string isbn) => await _api.GetAsync<BookAverageRatingDto>($"api/books/{Uri.EscapeDataString(isbn)}/avg-rating") ?? new BookAverageRatingDto { Isbn = isbn };
    public async Task<BookReviewDto?> UpdateReviewAsync(string isbn, int reviewerId, UpdateReviewRequestDto request) => await _api.PutAsync<BookReviewDto>($"api/reviews/reviews/book/{Uri.EscapeDataString(isbn)}/reviewer/{reviewerId}", request);
}
