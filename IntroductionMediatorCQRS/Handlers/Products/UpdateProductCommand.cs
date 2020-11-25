using System;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class UpdateProductCommand : Command
    {
        public Guid ProductId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
