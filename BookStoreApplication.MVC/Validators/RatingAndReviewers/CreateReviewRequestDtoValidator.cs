using BookStoreApplication.MVC.DTOs.ReviewsAndRatings;
using FluentValidation;

namespace BookStoreApplication.MVC.Validators.RatingAndReviewers
{
    public class CreateReviewRequestDtoValidator : AbstractValidator<CreateReviewRequestDto>
    {
        public CreateReviewRequestDtoValidator()
        {
            RuleFor(x => x.Isbn)
                .NotEmpty()
                .Length(13)
                .WithMessage("ISBN must be exactly 13 characters.");

            RuleFor(x => x.ReviewerId)
                .GreaterThan(0)
                .WithMessage("ReviewerId must be a positive number.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(20)
                .Matches("^[A-Za-z ]+$")
                .WithMessage("Name must contain only letters.");

            RuleFor(x => x.EmployedBy)
                .MaximumLength(30)
                .Matches("^[A-Za-z ]+$")
                .When(x => !string.IsNullOrWhiteSpace(x.EmployedBy))
                .WithMessage("EmployedBy must contain only letters.");

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
