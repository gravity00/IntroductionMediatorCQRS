using System;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class DeletedProductEvent : Event
    {
        public Guid ProductId { get; set; }
    }
}