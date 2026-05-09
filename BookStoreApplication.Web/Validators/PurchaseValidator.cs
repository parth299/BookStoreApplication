using BookStoreApplication.Web.DTOs;
using FluentValidation;

namespace BookStoreApplication.Web.Validators
{
    public class PurchaseValidator: AbstractValidator<PurchaseDto>
    {
        public PurchaseValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);
        }
    }
}
