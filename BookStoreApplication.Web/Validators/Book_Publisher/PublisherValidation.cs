using BookStoreApplication.Web.DTOs.Book_Publisher;
using FluentValidation;

namespace BookStoreApplication.Web.Validators.Book_Publisher;

public class PublisherCreateDtoValidator : AbstractValidator<PublisherCreateDto>
{
    public PublisherCreateDtoValidator()
    {
        RuleFor(x => x.PublisherId)
            .GreaterThan(0).WithMessage("Publisher id must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Publisher name is required.")
            .MaximumLength(50).WithMessage("Publisher name cannot exceed 50 characters.");

        RuleFor(x => x.City)
            .MaximumLength(30).WithMessage("City cannot exceed 30 characters.");

        RuleFor(x => x.StateCode)
            .Must(stateCode => string.IsNullOrWhiteSpace(stateCode) || stateCode.Trim().Length == 2)
            .WithMessage("State code must be exactly 2 characters when provided.");
    }
}


public class PublisherUpdateDtoValidator : AbstractValidator<PublisherUpdateDto>
{
    public PublisherUpdateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Publisher name is required.")
            .MaximumLength(50).WithMessage("Publisher name cannot exceed 50 characters.");

        RuleFor(x => x.City)
            .MaximumLength(30).WithMessage("City cannot exceed 30 characters.");

        RuleFor(x => x.StateCode)
            .Must(stateCode => string.IsNullOrWhiteSpace(stateCode) || stateCode.Trim().Length == 2)
            .WithMessage("State code must be exactly 2 characters when provided.");
    }
}
