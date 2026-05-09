using BookStoreApplication.Web.DTOs.ReviewsAndRatings;
using BookStoreApplication.Web.Services.ReviewsAndRatings;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers.ReviewAndRatings
{
    [Route("api/reviewers")]
    [ApiController]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerService _reviewerService;

        public ReviewerController(IReviewerService reviewerService)
        {
            _reviewerService = reviewerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReviewerAsync([FromBody] CreateReviewerRequestDto request)
        {
            var result = await _reviewerService.CreateReviewerAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviewersAsync()
        {
            var result = await _reviewerService.GetAllReviewersAsync();
            return Ok(result);
        }
    }
}
