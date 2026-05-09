using BookStoreApplication.Web.DTOs.Inventory;
using FluentValidation;

namespace BookStoreApplication.Web.Validators.Cart
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
