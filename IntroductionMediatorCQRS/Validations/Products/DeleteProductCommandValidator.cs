using FluentValidation;
using IntroductionMediatorCQRS.Handlers.Products;

namespace IntroductionMediatorCQRS.Validations.Products
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(e => e.ProductId)
                .NotEmpty();
        }
    }
}
