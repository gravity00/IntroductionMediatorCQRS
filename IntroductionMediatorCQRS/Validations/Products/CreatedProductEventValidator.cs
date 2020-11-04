using FluentValidation;
using IntroductionMediatorCQRS.Handlers.Products;

namespace IntroductionMediatorCQRS.Validations.Products
{
    public class CreatedProductEventValidator : AbstractValidator<CreatedProductEvent>
    {
        public CreatedProductEventValidator()
        {
            RuleFor(e => e.ExternalId)
                .NotEmpty();

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