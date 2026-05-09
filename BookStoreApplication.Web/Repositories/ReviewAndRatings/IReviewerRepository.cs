using BookStoreApplication.Web.DTOs.ReviewsAndRatings;

namespace BookStoreApplication.Web.Repositories.ReviewAndRatings
{
    public interface IReviewerRepository
    {
        Task<ReviewerDto> CreateReviewerAsync(CreateReviewerRequestDto request);
        Task<List<ReviewerDto>> GetAllReviewersAsync();
    }
}
