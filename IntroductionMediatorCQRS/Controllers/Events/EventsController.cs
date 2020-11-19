using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Handlers.Events;
using Microsoft.AspNetCore.Mvc;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Controllers.Events
{
    [Route("events")]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<SearchEventsItemModel>> SearchAsync([FromQuery] string filterQ, [FromQuery] int? skip, [FromQuery] int? take, CancellationToken ct)
        {
            var result = await _mediator.FetchAsync(new SearchEventsQuery
            {
                FilterQ = filterQ,
                Skip = skip,
                Take = take
            }, ct);

            return result.Select(r => new SearchEventsItemModel
            {
                Id = r.Id,
                Name = r.Name,
                CreatedOn = r.CreatedOn,
                CreatedBy = r.CreatedBy
            });
        }
    }
}
