using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Database;
using Microsoft.EntityFrameworkCore;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Handlers.Products
{
    public class SearchProductsQueryHandler : IQueryHandler<SearchProductsQuery, IEnumerable<Product>>
    {
        private readonly IQueryable<ProductEntity> _products;

        public SearchProductsQueryHandler(ApiDbContext context)
        {
            _products = context.Set<ProductEntity>();
        }

        public async Task<IEnumerable<Product>> HandleAsync(SearchProductsQuery query, CancellationToken ct)
        {
            var filter = _products;

            if (!string.IsNullOrWhiteSpace(query.FilterQ))
            {
                var filterQ = query.FilterQ.Trim();

                filter = filter.Where(p =>
                    p.Code.Contains(filterQ) ||
                    p.Name.Contains(filterQ)
                );
            }

            var skip = query.Skip ?? 0;
            var take = query.Take ?? 20;

            return await filter.OrderBy(p => p.Code).Skip(skip).Take(take).Select(p => new Product
            {
                Id = p.ExternalId,
                Code = p.Code,
                Name = p.Name,
                Price = p.Price
            }).ToListAsync(ct);
        }
    }
}