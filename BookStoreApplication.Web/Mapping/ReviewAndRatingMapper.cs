using BookStoreApplication.Web.DTOs.ReviewsAndRatings;
using BookStoreApplication.Web.Models;

namespace BookStoreApplication.Web.Mapping
{
    public static class ReviewAndRatingMapper
    {
        public static BookReviewDto ToBookReviewDto(Bookreview review)
        {
            return new BookReviewDto
            {
                Isbn = review.Isbn,
                ReviewerId = review.ReviewerId,
                ReviewerName = review.Reviewer.Name,
                EmployedBy = review.Reviewer.EmployedBy,
                Rating = review.Rating,
                Comments = review.Comments
            };
        }

        public static ReviewerDto ToReviewerDto(Reviewer reviewer)
        {
            return new ReviewerDto
            {
                ReviewerId = reviewer.ReviewerId,
                Name = reviewer.Name,
                EmployedBy = reviewer.EmployedBy
            };
        }

        public static CreateReviewResponseDto ToCreateReviewResponseDto(Bookreview review, Reviewer reviewer)
        {
            return new CreateReviewResponseDto
            {
                Isbn = review.Isbn,
                ReviewerId = reviewer.ReviewerId,
                ReviewerName = reviewer.Name,
                EmployedBy = reviewer.EmployedBy,
                Rating = review.Rating ?? 0,
                Comments = review.Comments,
                Message = "Review created successfully."
            };
        }
    }
}
