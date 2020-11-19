using System;

namespace IntroductionMediatorCQRS.Handlers.Events
{
    public class Event
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public dynamic Payload { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}