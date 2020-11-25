using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Handlers.Audits;
using Microsoft.AspNetCore.Mvc;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Controllers.Audits
{
    [Route("audits")]
    public class AuditsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuditsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<AuditSearchItemModel>> SearchAsync([FromQuery] string filterQ, [FromQuery] int? skip, [FromQuery] int? take, CancellationToken ct)
        {
            var result = await _mediator.FetchAsync(new SearchAuditsQuery
            {
                FilterQ = filterQ,
                Skip = skip,
                Take = take,
                CreatedBy = User.Identity.Name
            }, ct);

            return result.Select(r => new AuditSearchItemModel
            {
                Id = r.Id,
                Name = r.Name,
                CreatedOn = r.CreatedOn,
                CreatedBy = r.CreatedBy,
                ExecutionTimeInMs = r.ExecutionTimeInMs
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<AuditModel> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await _mediator.FetchAsync(new GetAuditByIdQuery
            {
                AuditId = id,
                CreatedBy = User.Identity.Name
            }, ct);

            return new AuditModel
            {
                Id = result.Id,
                Name = result.Name,
                Payload = result.Payload,
                Result = result.Result,
                CreatedOn = result.CreatedOn,
                CreatedBy = result.CreatedBy,
                ExecutionTimeInMs = result.ExecutionTimeInMs,
                Events = result.Events.Select(e => new AuditEventModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Payload = e.Payload,
                    CreatedOn = e.CreatedOn
                })
            };
        }
    }
}
