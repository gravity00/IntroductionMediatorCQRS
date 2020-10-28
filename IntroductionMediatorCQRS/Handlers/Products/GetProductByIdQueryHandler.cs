using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Database;
using Microsoft.EntityFrameworkCore;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Product>
    {
        private readonly IQueryable<ProductEntity> _products;

        public GetProductByIdQueryHandler(ApiDbContext context)
        {
            _products = context.Set<ProductEntity>();
        }

        public async Task<Product> HandleAsync(GetProductByIdQuery query, CancellationToken ct)
        {
            var product = await _products.SingleOrDefaultAsync(p => p.ExternalId == query.ProductId, ct);

            if (product == null)
            {
                throw new InvalidOperationException($"Product '{query.ProductId}' not found");
            }

            return new Product
            {
                Id = product.ExternalId,
                Code = product.Code,
                Name = product.Name,
                Price = product.Price
            };
        }
    }
}