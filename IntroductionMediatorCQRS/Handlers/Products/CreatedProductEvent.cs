using System;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class CreatedProductEvent : Event
    {
        public Guid ExternalId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}