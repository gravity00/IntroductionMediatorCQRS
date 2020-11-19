using System;

namespace IntroductionMediatorCQRS.Database
{
    public class EventEntity
    {
        public long Id { get; set; }

        public Guid ExternalId { get; set; }

        public string Name { get; set; }

        public string Payload { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}