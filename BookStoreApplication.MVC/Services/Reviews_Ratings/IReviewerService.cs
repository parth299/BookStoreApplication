using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;

namespace BookStoreApplication.MVC.Services.Reviews_Ratings
{
    public interface IReviewerService
    {
        Task<ReviewerDto> CreateReviewerAsync(CreateReviewerRequestDto request);
        Task<List<ReviewerDto>> GetAllReviewersAsync();
    }
}
