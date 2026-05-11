namespace BookStoreApplication.Web.DTOs.ReviewsAndRatings
{
    public class UpdateReviewResponseDto
    {
        public string Isbn { get; set; }
        public int ReviewerId { get; set; }
        public int? Rating { get; set; }
        public string? Comments { get; set; }
    }
}
