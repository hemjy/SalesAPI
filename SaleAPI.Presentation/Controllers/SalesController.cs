using Microsoft.AspNetCore.Mvc;
using SaleAPI.Presentation.Extensions;
using SalesAPI.Application.DTOs.SalesOrders;
using SalesAPI.Application.Features.Products.Commands;
using SalesAPI.Application.Features.Products.Queries;

namespace SaleAPI.Presentation.Controllers
{
    [ApiVersion("1.0")]
    public class SalesController : BaseApiController
    {
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateSalesOrderCommand model, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(model, cancellationToken);
            return response.ToActionResult();


        } 


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(new DeleteSalesOrderCommand(id), cancellationToken);
            return response.ToActionResult();


        }


        [HttpGet()]
        public async Task<IActionResult> GetAllByFilter([FromQuery] GetSalesOrdersQuery model, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(model, cancellationToken);
            return response.ToActionResult();


        }
    }
}
