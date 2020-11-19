using System;

namespace IntroductionMediatorCQRS.Controllers.Events
{
    public class EventModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public dynamic Payload { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}