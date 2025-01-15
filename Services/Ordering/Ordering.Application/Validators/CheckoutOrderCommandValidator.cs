using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators
{
    public class CheckoutOrderCommandValidator: AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
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
            RuleFor(p => p.EmailAddress)
                .NotEmpty()
                .WithMessage("{EmailAddress} is required.")
                .EmailAddress();
            RuleFor(p => p.FirstName)
                .NotEmpty()
                 .WithMessage("{FirstName} is required.");
            RuleFor(p => p.LastName)
                .NotEmpty()
                 .WithMessage("{LastName} is required.");
            RuleFor(p => p.AddressLine).NotEmpty();
        }
    }
}
