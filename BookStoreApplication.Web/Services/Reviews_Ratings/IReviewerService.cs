using BookStoreApplication.Web.DTOs.ReviewsAndRatings;

namespace BookStoreApplication.Web.Services.Reviews_Ratings
{
    public interface IReviewerService
    {
        Task<ReviewerDto> CreateReviewerAsync(CreateReviewerRequestDto request);
        Task<List<ReviewerDto>> GetAllReviewersAsync();
    }
}
