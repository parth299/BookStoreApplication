namespace BookStoreApplication.Web.DTOs.ReviewsAndRatings
{
    public class CreateReviewerRequestDto
    {
        public int ReviewerId { get; set; }
        public string Name { get; set; } = null!;
        public string? EmployedBy { get; set; }
    }
}
