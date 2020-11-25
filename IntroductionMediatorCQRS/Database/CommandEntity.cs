using System;

namespace IntroductionMediatorCQRS.Database
{
    public class CommandEntity
    {
        public long Id { get; set; }

        public Guid ExternalId { get; set; }

        public string Name { get; set; }

        public string Payload { get; set; }

        public string Result { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public TimeSpan ExecutionTime { get; set; }
    }
}