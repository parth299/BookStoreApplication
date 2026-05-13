using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;

namespace BookStoreApplication.MVC.Repositories.ReviewAndRatings
{
    public interface IReviewerRepository
    {
        Task<ReviewerDto> CreateReviewerAsync(CreateReviewerRequestDto request);
        Task<List<ReviewerDto>> GetAllReviewersAsync();
    }
}
