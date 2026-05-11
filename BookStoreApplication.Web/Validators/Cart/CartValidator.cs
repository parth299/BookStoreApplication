using BookStoreApplication.Web.DTOs.Inventory;
using FluentValidation;

namespace BookStoreApplication.Web.Validators.Cart
{
    public class CartValidator: AbstractValidator<CartItemDto>
    {
        public CartValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);

            RuleFor(x => x.ISBN)
                .NotEmpty();

            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}
