using BookStoreApplication.Web.DTOs.User;
using FluentValidation;

namespace BookStoreApplication.Web.Validators.Auth
{
    public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterUserDTOValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required")
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required")
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Username is required")
                .MinimumLength(4)
                .MaximumLength(20);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required")
                .Matches(@"^[0-9]{10}$")
                .WithMessage("Phone number must be 10 digits");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]")
                .WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]")
                .WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("Password must contain at least one special character");

            RuleFor(x => x.RoleNumber)
                .GreaterThan(0)
                .WithMessage("Invalid role number");
        }
    }
}