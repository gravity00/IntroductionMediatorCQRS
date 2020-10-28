using System;

namespace IntroductionMediatorCQRS.Database
{
    public class ProductEntity
    {
        public long Id { get; set; }

        public Guid ExternalId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}