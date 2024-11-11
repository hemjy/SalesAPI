using Microsoft.AspNetCore.Mvc;
using SaleAPI.Presentation.Extensions;
using SalesAPI.Application.Features.Products.Commands;
using SalesAPI.Application.Features.Products.Queries;

namespace SaleAPI.Presentation.Controllers
{
    [ApiVersion("1.0")]
    public class ProductsController : BaseApiController
    {
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand model, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(model, cancellationToken);
            return response.ToActionResult();
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllByFilter([FromQuery] GetProductQuery model, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(model, cancellationToken);
            return response.ToActionResult();


        }
    }
}
