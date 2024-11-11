using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleAPI.Presentation.Extensions;
using SalesAPI.Application.Features.SalesOrders.Queries;

namespace SaleAPI.Presentation.Controllers
{
    [ApiVersion("1.0")]
    public class DashboardController  : BaseApiController
    {
        [HttpGet("overview")]
        public async Task<IActionResult> GetAllByFilter( CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(new GetHighestSoldProductQuery(), cancellationToken);
            return response.ToActionResult();


        }
    }
}
