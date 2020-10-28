using System;
using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Database;
using Microsoft.EntityFrameworkCore;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly ApiDbContext _context;

        public CreateProductCommandHandler(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<CreateProductResult> HandleAsync(CreateProductCommand cmd, CancellationToken ct)
        {
            var products = _context.Set<ProductEntity>();

            if (await products.AnyAsync(p => p.Code == cmd.Code, ct))
            {
                throw new InvalidOperationException($"Product code '{cmd.Code}' already exists");
            }

            var externalId = Guid.NewGuid();
            await products.AddAsync(new ProductEntity
            {
                ExternalId = externalId,
                Code = cmd.Code,
                Name = cmd.Name,
                Price = cmd.Price
            }, ct);

            await _context.SaveChangesAsync(ct);

            return new CreateProductResult
            {
                Id = externalId
            };
        }
    }
}