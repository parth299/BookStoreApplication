namespace BookStoreApplication.MVC.DTOs.ReviewsAndRatings
{
    public class ReviewerDto
    {
        public int ReviewerId { get; set; }
        public string Name { get; set; } = null!;
        public string? EmployedBy { get; set; }
    }
}
