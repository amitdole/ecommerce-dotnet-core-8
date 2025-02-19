using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators
{
    public class CheckoutOrderCommandValidatorV2 : AbstractValidator<CheckoutOrderCommandV2>
    {
        public CheckoutOrderCommandValidatorV2()
        {
            RuleFor(p => p.UserName)
                .NotEmpty()
                .WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(70)
                .WithMessage("{UserName} must not exceed 70 characters.");

            RuleFor(p => p.TotalPrice)
                .NotEmpty()
                .WithMessage("{TotalPrice} is required.")
                .GreaterThan(-1)
                .WithMessage("{TotalPrice} should not be negative.");
        }
    }
}
