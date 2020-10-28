using System;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class DeleteProductCommand : Command
    {
        public Guid ProductId { get; set; }
    }
}
