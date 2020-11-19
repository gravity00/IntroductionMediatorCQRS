using System.Collections.Generic;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Events
{
    public class SearchEventsQuery : Query<IEnumerable<SearchEventsItem>>
    {
        public string FilterQ { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}
