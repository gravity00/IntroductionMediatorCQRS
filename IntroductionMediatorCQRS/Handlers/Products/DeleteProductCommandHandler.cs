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

        public DeleteProductCommandHandler(ApiDbContext context)
        {
            _context = context;
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

            await _context.SaveChangesAsync(ct);
        }
    }
}