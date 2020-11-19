using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Database;
using Microsoft.EntityFrameworkCore;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Events
{
    public class SearchEventsQueryHandler : IQueryHandler<SearchEventsQuery, IEnumerable<SearchEventsItem>>
    {
        private readonly IQueryable<EventEntity> _events;

        public SearchEventsQueryHandler(ApiDbContext context)
        {
            _events = context.Set<EventEntity>();
        }

        public async Task<IEnumerable<SearchEventsItem>> HandleAsync(SearchEventsQuery query, CancellationToken ct)
        {
            var filter = _events;

            if (!string.IsNullOrWhiteSpace(query.FilterQ))
            {
                var filterQ = query.FilterQ.Trim();

                filter = filter.Where(e =>
                    e.Name.Contains(filterQ)
                );
            }

            var skip = query.Skip ?? 0;
            var take = query.Take ?? 20;

            return await filter
                .OrderByDescending(e => e.CreatedOn)
                .ThenByDescending(e => e.Id)
                .Skip(skip)
                .Take(take)
                .Select(e => new SearchEventsItem
                {
                    Id = e.ExternalId,
                    Name = e.Name,
                    CreatedOn = e.CreatedOn,
                    CreatedBy = e.CreatedBy
                }).ToListAsync(ct);
        }
    }
}