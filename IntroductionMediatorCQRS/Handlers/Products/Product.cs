using System;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}