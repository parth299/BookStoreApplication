using BookStoreApplication.Web.DTOs.ReviewsAndRatings;
using BookStoreApplication.Web.Services.ReviewsAndRatings;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers.ReviewAndRatings
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IBookReviewService _bookReviewService;

        public ReviewController(IBookReviewService bookReviewService)
        {
            _bookReviewService = bookReviewService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReviewAsync([FromBody] CreateReviewRequestDto request)
        {
            var result = await _bookReviewService.CreateReviewAsync(request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviewsAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _bookReviewService.GetAllReviewsAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("book/{isbn}")]
        public async Task<IActionResult> GetBookReviewsByIsbnAsync(string isbn)
        {
            var result = await _bookReviewService.GetBookReviewsByIsbnAsync(isbn);
            return Ok(result);
        }

        [HttpGet("reviewer/{reviewerId:int}")]
        public async Task<IActionResult> GetBookReviewsByReviewerIdAsync(int reviewerId)
        {
            var result = await _bookReviewService.GetBookReviewsByReviewerIdAsync(reviewerId);
            return Ok(result);
        }
        [HttpPut("reviews/book/{isbn}/reviewer/{reviewerId:int}")]
        public async Task<IActionResult> UpdateReview(string isbn,int reviewerId,[FromBody] UpdateReviewRequestDto request)
        {
            var result = await _bookReviewService.UpdateReviewAsync(isbn, reviewerId, request);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Review not found for this ISBN and ReviewerId."
                });
            }

            return Ok(result);
        }
        }
    }

