using BookStoreApplication.Web.DTOs.ReviewsAndRatings;
using BookStoreApplication.Web.Services.Reviews_Ratings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApplication.Web.Controllers.Reviewers_Reviews
{
    [Route("api/reviewers")]
    [ApiController]
    [Authorize(Roles = "Guest,RegisteredUser,StoreOwner,Admin")]
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerService _reviewerService;

        public ReviewerController(IReviewerService reviewerService)
        {
            _reviewerService = reviewerService;
        }

        [Authorize(Roles = "StoreOwner,Admin")]
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
