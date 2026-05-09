using BookStoreApplication.Web.Services.ReviewsAndRatings;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApplication.Web.Controllers.ReviewAndRatings
{
    [Route("api/books")]
    [ApiController]
    public class BookRatingController : ControllerBase
    {
        private readonly IBookReviewService _bookReviewService;

        public BookRatingController(IBookReviewService bookReviewService)
        {
            _bookReviewService = bookReviewService;
        }

        [HttpGet("{isbn}/avg-rating")]
        public async Task<IActionResult> GetAverageRatingByIsbnAsync(string isbn)
        {
            var result = await _bookReviewService.GetAverageRatingByIsbnAsync(isbn);
            return Ok(result);
        }
    }
}
