using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleSoft.Mediator;

namespace IntroductionMediatorCQRS.Controllers.Products
{
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<ProductModel>> SearchAsync([FromQuery] string filterQ, [FromQuery] int? skip, [FromQuery] int? take, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        [HttpGet("id:guid")]
        public async Task<ProductModel> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<CreateProductResultModel> CreateAsync([FromBody] CreateProductModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        [HttpPut("id:guid")]
        public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateProductModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("id:guid")]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
