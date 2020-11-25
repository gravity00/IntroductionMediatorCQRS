using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Database;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class DeletedProductEventHandler : IEventHandler<DeletedProductEvent>
    {
        private readonly ApiDbContext _context;

        public DeletedProductEventHandler(ApiDbContext context)
        {
            _context = context;
        }

        public Task HandleAsync(DeletedProductEvent evt, CancellationToken ct)
        {
            //await _context.Set<EventEntity>().AddAsync(new EventEntity
            //{
            //    ExternalId = evt.Id,
            //    Name = nameof(DeletedProductEvent),
            //    Payload = JsonSerializer.Serialize(evt),
            //    CreatedOn = evt.CreatedOn,
            //    CreatedBy = evt.CreatedBy
            //}, ct);

            return Task.CompletedTask;
        }
    }
}