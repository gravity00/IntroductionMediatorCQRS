using FluentValidation;
using IntroductionMediatorCQRS.Handlers.Products;

namespace IntroductionMediatorCQRS.Validations.Products
{
    public class UpdatedProductEventValidator : AbstractValidator<UpdatedProductEvent>
    {
        public UpdatedProductEventValidator()
        {
            RuleFor(e => e.ProductId)
                .NotEmpty();

            RuleFor(e => e.PreviousCode)
                .NotEmpty()
                .Length(8)
                .Matches("^[0-9a-zA-Z]*$");

            RuleFor(e => e.PreviousName)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(e => e.PreviousPrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(e => e.CurrentCode)
                .NotEmpty()
                .Length(8)
                .Matches("^[0-9a-zA-Z]*$");

            RuleFor(e => e.CurrentName)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(e => e.CurrentPrice)
                .GreaterThanOrEqualTo(0);
        }
    }
}