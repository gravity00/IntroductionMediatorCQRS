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

        public UpdateProductCommandHandler(ApiDbContext context)
        {
            _context = context;
        }

        public async Task HandleAsync(UpdateProductCommand cmd, CancellationToken ct)
        {
            var products = _context.Set<ProductEntity>();
            
            var product = await products.SingleOrDefaultAsync(p => p.ExternalId == cmd.ProductId, ct);

            if (product == null)
            {
                throw new InvalidOperationException($"Product '{cmd.ProductId}' not found");
            }

            product.Code = cmd.Code;
            product.Name = cmd.Name;
            product.Price = cmd.Price;

            await _context.SaveChangesAsync(ct);
        }
    }
}