using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Database;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class CreatedProductEventHandler : IEventHandler<CreatedProductEvent>
    {
        private readonly ApiDbContext _context;

        public CreatedProductEventHandler(ApiDbContext context)
        {
            _context = context;
        }

        public Task HandleAsync(CreatedProductEvent evt, CancellationToken ct)
        {
            //await _context.Set<EventEntity>().AddAsync(new EventEntity
            //{
            //    ExternalId = evt.Id,
            //    Name = nameof(CreatedProductEvent),
            //    Payload = JsonSerializer.Serialize(evt),
            //    CreatedOn = evt.CreatedOn,
            //    CreatedBy = evt.CreatedBy
            //}, ct);

            return Task.CompletedTask;
        }
    }
}