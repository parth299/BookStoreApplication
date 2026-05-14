namespace BookStoreApplication.MVC.DTOs.ReviewsAndRatings
{
    public class ApiErrorResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = null!;
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
