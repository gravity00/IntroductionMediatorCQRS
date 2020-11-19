using System;

namespace IntroductionMediatorCQRS.Controllers.Events
{
    public class SearchEventsItemModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string CreatedBy { get; set; }
    }
}