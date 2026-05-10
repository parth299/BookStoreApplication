namespace BookStoreApplication.Web.DTOs.ReviewsAndRatings
{
    public class BookReviewDto
    {
        public string Isbn { get; set; } = null!;
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; } = null!;
        public string? EmployedBy { get; set; }
        public int? Rating { get; set; }
        public string? Comments { get; set; }
    }
}
