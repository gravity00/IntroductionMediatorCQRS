using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Database;
using Microsoft.EntityFrameworkCore;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Events
{
    public class GetEventByIdQueryHandler : IQueryHandler<GetEventByIdQuery, Event>
    {
        private readonly IQueryable<EventEntity> _events;

        public GetEventByIdQueryHandler(ApiDbContext context)
        {
            _events = context.Set<EventEntity>();
        }

        public async Task<Event> HandleAsync(GetEventByIdQuery query, CancellationToken ct)
        {
            var @event = await _events.SingleOrDefaultAsync(e => e.ExternalId == query.EventId, ct);

            if (@event == null)
            {
                throw new InvalidOperationException($"Event '{query.EventId}' not found");
            }

            return new Event
            {
                Id = @event.ExternalId,
                Name = @event.Name,
                Payload = JsonSerializer.Deserialize<dynamic>(@event.Payload),
                CreatedOn = @event.CreatedOn,
                CreatedBy = @event.CreatedBy
            };
        }
    }
}