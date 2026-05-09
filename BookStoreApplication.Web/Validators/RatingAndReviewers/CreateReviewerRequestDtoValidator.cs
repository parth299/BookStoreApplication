using BookStoreApplication.Web.DTOs.ReviewsAndRatings;
using FluentValidation;

namespace BookStoreApplication.Web.Validators.RatingAndReviewers
{
    public class CreateReviewerRequestDtoValidator : AbstractValidator<CreateReviewerRequestDto>
    {
        public CreateReviewerRequestDtoValidator()
        {
            RuleFor(x => x.ReviewerId)
                .GreaterThan(0)
                .WithMessage("ReviewerId must be a positive number.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(20)
                .Matches("^[A-Za-z ]+$")
                .WithMessage("Name must contain only letters.");

            RuleFor(x => x.EmployedBy)
                .NotEmpty()
                .MaximumLength(30)
                .Matches("^[A-Za-z ]+$")
                .WithMessage("EmployedBy must contain only letters.");
        }
    }
}