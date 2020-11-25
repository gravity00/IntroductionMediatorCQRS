using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Database;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class UpdatedProductEventHandler : IEventHandler<UpdatedProductEvent>
    {
        private readonly ApiDbContext _context;

        public UpdatedProductEventHandler(ApiDbContext context)
        {
            _context = context;
        }

        public Task HandleAsync(UpdatedProductEvent evt, CancellationToken ct)
        {
            //await _context.Set<EventEntity>().AddAsync(new EventEntity
            //{
            //    ExternalId = evt.Id,
            //    Name = nameof(UpdatedProductEvent),
            //    Payload = JsonSerializer.Serialize(evt),
            //    CreatedOn = evt.CreatedOn,
            //    CreatedBy = evt.CreatedBy
            //}, ct);

            return Task.CompletedTask;
        }
    }
}