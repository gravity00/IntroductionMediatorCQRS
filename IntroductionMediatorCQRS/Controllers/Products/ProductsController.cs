using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IntroductionMediatorCQRS.Handlers.Products;
using Microsoft.AspNetCore.Mvc;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Controllers.Products
{
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductModel>> SearchAsync([FromQuery] string filterQ, [FromQuery] int? skip, [FromQuery] int? take, CancellationToken ct)
        {
            var result = await _mediator.FetchAsync(new SearchProductsQuery
            {
                FilterQ = filterQ,
                Skip = skip,
                Take = take,
                CreatedBy = User.Identity.Name
            }, ct);

            return result.Select(r => new ProductModel
            {
                Id = r.Id,
                Code = r.Code,
                Name = r.Name,
                Price = r.Price
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<ProductModel> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
        {
            var result = await _mediator.FetchAsync(new GetProductByIdQuery
            {
                ProductId = id,
                CreatedBy = User.Identity.Name
            }, ct);

            return new ProductModel
            {
                Id = result.Id,
                Code = result.Code,
                Name = result.Name,
                Price = result.Price
            };
        }

        [HttpPost]
        public async Task<CreateProductResultModel> CreateAsync([FromBody] CreateProductModel model, CancellationToken ct)
        {
            var result = await _mediator.SendAsync(new CreateProductCommand
            {
                Code = model.Code,
                Name = model.Name,
                Price = model.Price,
                CreatedBy = User.Identity.Name
            }, ct);

            return new CreateProductResultModel
            {
                Id = result.Id
            };
        }

        [HttpPut("{id:guid}")]
        public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateProductModel model, CancellationToken ct)
        {
            await _mediator.SendAsync(new UpdateProductCommand
            {
                ProductId = id,
                Code = model.Code,
                Name = model.Name,
                Price = model.Price,
                CreatedBy = User.Identity.Name
            }, ct);
        }

        [HttpDelete("{id:guid}")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken ct)
        {
            await _mediator.SendAsync(new DeleteProductCommand
            {
                ProductId = id,
                CreatedBy = User.Identity.Name
            }, ct);
        }
    }
}
