using FluentValidation;
using IntroductionMediatorCQRS.Handlers.Products;

namespace IntroductionMediatorCQRS.Validations.Products
{
    public class DeletedProductEventValidator : AbstractValidator<DeletedProductEvent>
    {
        public DeletedProductEventValidator()
        {
            RuleFor(e => e.ProductId)
                .NotEmpty();
        }
    }
}