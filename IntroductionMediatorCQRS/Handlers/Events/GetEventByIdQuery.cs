using System;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Events
{
    public class GetEventByIdQuery : Query<Event>
    {
        public Guid EventId { get; set; }
    }
}
