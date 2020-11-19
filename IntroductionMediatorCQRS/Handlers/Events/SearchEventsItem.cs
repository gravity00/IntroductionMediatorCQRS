using System;

namespace IntroductionMediatorCQRS.Handlers.Events
{
    public class SearchEventsItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}