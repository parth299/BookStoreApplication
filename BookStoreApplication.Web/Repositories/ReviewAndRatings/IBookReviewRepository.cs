using BookStoreApplication.Web.DTOs.ReviewsAndRatings;

namespace BookStoreApplication.Web.Repositories.ReviewAndRatings
{
    public interface IBookReviewRepository
    {
        Task<CreateReviewResponseDto> CreateReviewAsync(CreateReviewRequestDto request);
        Task<List<BookReviewDto>> GetAllReviewsAsync(int page, int pageSize);
        Task<List<BookReviewDto>> GetBookReviewsByIsbnAsync(string isbn);
        Task<List<BookReviewDto>> GetBookReviewsByReviewerIdAsync(int reviewerId);
        Task<BookAverageRatingDto> GetAverageRatingByIsbnAsync(string isbn);
        Task<BookReviewDto?> UpdateReviewAsync(string isbn, int reviewerId, UpdateReviewRequestDto request);

    }
}
