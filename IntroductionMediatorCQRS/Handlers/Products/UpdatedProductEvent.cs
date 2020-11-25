using System;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class UpdatedProductEvent : Event
    {
        public Guid ProductId { get; set; }

        public string PreviousCode { get; set; }

        public string PreviousName { get; set; }

        public decimal PreviousPrice { get; set; }

        public string CurrentCode { get; set; }

        public string CurrentName { get; set; }

        public decimal CurrentPrice { get; set; }
    }
}