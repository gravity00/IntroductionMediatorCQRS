using System;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class DeleteProductCommand : Command
    {
        public Guid ProductId { get; set; }
    }
}
