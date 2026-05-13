using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;
using FluentValidation;

namespace BookStoreApplication.MVC.Validators.RatingAndReviewers
{
    public class UpdateReviewRequestDtoValidator : AbstractValidator<UpdateReviewRequestDto>
    {
        public UpdateReviewRequestDtoValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 10)
                .WithMessage("Rating must be between 1 and 10.");

            RuleFor(x => x.Comments)
                .MaximumLength(255)
                .When(x => !string.IsNullOrWhiteSpace(x.Comments))
                .WithMessage("Comments cannot exceed 255 characters.");
        }
    }
}
