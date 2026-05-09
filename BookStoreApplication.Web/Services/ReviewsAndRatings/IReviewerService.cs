using BookStoreApplication.Web.DTOs.ReviewsAndRatings;

namespace BookStoreApplication.Web.Services.ReviewsAndRatings
{
    public interface IReviewerService
    {
        Task<ReviewerDto> CreateReviewerAsync(CreateReviewerRequestDto request);
        Task<List<ReviewerDto>> GetAllReviewersAsync();
    }
}
