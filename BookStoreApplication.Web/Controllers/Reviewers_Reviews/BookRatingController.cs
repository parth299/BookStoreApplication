using BookStoreApplication.Web.Services.Reviews_Ratings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreApplication.Web.Controllers.Reviewers_Reviews
{
    [Route("api/books")]
    [ApiController]
    [Authorize(Roles = "Guest,RegisteredUser,StoreOwner,Admin")]
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
