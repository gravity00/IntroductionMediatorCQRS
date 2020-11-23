using System;
using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Database;
using Microsoft.EntityFrameworkCore;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly ApiDbContext _context;
        private readonly IMediator _mediator;

        public UpdateProductCommandHandler(ApiDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task HandleAsync(UpdateProductCommand cmd, CancellationToken ct)
        {
            var products = _context.Set<ProductEntity>();
            
            var product = await products.SingleOrDefaultAsync(p => p.ExternalId == cmd.ProductId, ct);

            if (product == null)
            {
                throw new InvalidOperationException($"Product '{cmd.ProductId}' not found");
            }

            if (product.Code == cmd.Code &&
                product.Name == cmd.Name &&
                product.Price == cmd.Price)
            {
                return;
            }

            var previousCode = product.Code;
            var previousName = product.Name;
            var previousPrice = product.Price;

            product.Code = cmd.Code;
            product.Name = cmd.Name;
            product.Price = cmd.Price;

            await _mediator.BroadcastAsync(new UpdatedProductEvent
            {
                ProductId = cmd.ProductId,
                PreviousCode = previousCode,
                PreviousName = previousName,
                PreviousPrice = previousPrice,
                CurrentCode = product.Code,
                CurrentName = product.Name,
                CurrentPrice = product.Price,
                CreatedBy = cmd.CreatedBy
            }, ct);

            //await _context.SaveChangesAsync(ct);
        }
    }
}