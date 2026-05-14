using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;
using BookStoreApplication.MVC.Services.Api;

namespace BookStoreApplication.MVC.Services.Reviews_Ratings;

public class ReviewerService : IReviewerService
{
    private readonly ApiClient _api;
    public ReviewerService(ApiClient api) => _api = api;
    public async Task<ReviewerDto> CreateReviewerAsync(CreateReviewerRequestDto request) => await _api.PostAsync<ReviewerDto>("api/reviewers", request) ?? new ReviewerDto { ReviewerId = request.ReviewerId, Name = request.Name, EmployedBy = request.EmployedBy };
    public async Task<List<ReviewerDto>> GetAllReviewersAsync() => await _api.GetListAsync<ReviewerDto>("api/reviewers");
}
