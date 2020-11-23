using System;
using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Database;
using Microsoft.EntityFrameworkCore;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        private readonly ApiDbContext _context;
        private readonly IMediator _mediator;

        public DeleteProductCommandHandler(ApiDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task HandleAsync(DeleteProductCommand cmd, CancellationToken ct)
        {
            var products = _context.Set<ProductEntity>();

            var product = await products.SingleOrDefaultAsync(p => p.ExternalId == cmd.ProductId, ct);

            if (product == null)
            {
                throw new InvalidOperationException($"Product '{cmd.ProductId}' not found");
            }

            products.Remove(product);

            await _mediator.BroadcastAsync(new DeletedProductEvent
            {
                ProductId = cmd.ProductId,
                CreatedBy = cmd.CreatedBy
            }, ct);

            //await _context.SaveChangesAsync(ct);
        }
    }
}