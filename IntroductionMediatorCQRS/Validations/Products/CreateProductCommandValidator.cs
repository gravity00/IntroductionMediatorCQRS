using FluentValidation;
using IntroductionMediatorCQRS.Handlers.Products;

namespace IntroductionMediatorCQRS.Validations.Products
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(e => e.Code)
                .NotEmpty()
                .Length(8)
                .Matches("^[0-9a-zA-Z]*$");

            RuleFor(e => e.Name)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(e => e.Price)
                .GreaterThanOrEqualTo(0);
        }
    }
}
