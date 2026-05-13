namespace BookStoreApplication.MVC.DTOs.ReviewsAndRatings
{
    public class BookAverageRatingDto
    {
        public string Isbn { get; set; } = null!;
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
