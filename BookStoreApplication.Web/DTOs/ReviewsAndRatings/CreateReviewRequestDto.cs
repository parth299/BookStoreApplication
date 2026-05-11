namespace BookStoreApplication.Web.DTOs.ReviewsAndRatings
{
    public class CreateReviewRequestDto
    {
        public string Isbn { get; set; } = null!;
        public int ReviewerId { get; set; }
        public string Name { get; set; } = null!;
        public string? EmployedBy { get; set; }
        public int Rating { get; set; }
        public string? Comments { get; set; }
    }
}
