using BookStoreApplication.Web.DTOs;
using FluentValidation;

namespace BookStoreApplication.Web.Validators;

public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
{
    public BookCreateDtoValidator()
    {
        RuleFor(x => x.Isbn)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("ISBN is required.")
            .Must(isbn => isbn.Trim().Length is >= 10 and <= 13)
            .WithMessage("ISBN must be between 10 and 13 characters.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(70).WithMessage("Title cannot exceed 70 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");

        RuleFor(x => x.Category)
            .GreaterThan(0).When(x => x.Category.HasValue)
            .WithMessage("Category must be greater than 0.");

        RuleFor(x => x.Edition)
            .MaximumLength(30).WithMessage("Edition cannot exceed 30 characters.");

        RuleFor(x => x.PublisherId)
            .GreaterThan(0).WithMessage("Publisher id must be greater than 0.");
    }
}


public class BookFilterDtoValidator : AbstractValidator<BookFilterDto>
{
    public BookFilterDtoValidator()
    {
        RuleFor(x => x.Category)
            .GreaterThan(0).When(x => x.Category.HasValue)
            .WithMessage("Category must be greater than 0.");

        RuleFor(x => x.PublisherId)
            .GreaterThan(0).When(x => x.PublisherId.HasValue)
            .WithMessage("Publisher id must be greater than 0.");

        RuleFor(x => x.Edition)
            .MaximumLength(30).WithMessage("Edition cannot exceed 30 characters.");

        RuleFor(x => x.Title)
            .MaximumLength(70).WithMessage("Title cannot exceed 70 characters.");
    }
}

public class BookUpdateDtoValidator : AbstractValidator<BookUpdateDto>
{
    public BookUpdateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(70).WithMessage("Title cannot exceed 70 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");

        RuleFor(x => x.Category)
            .GreaterThan(0).When(x => x.Category.HasValue)
            .WithMessage("Category must be greater than 0.");

        RuleFor(x => x.Edition)
            .MaximumLength(30).WithMessage("Edition cannot exceed 30 characters.");

        RuleFor(x => x.PublisherId)
            .GreaterThan(0).WithMessage("Publisher id must be greater than 0.");
    }
}