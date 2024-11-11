using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleAPI.Presentation.Controllers;
using SaleAPI.Presentation.Extensions;
using SalesAPI.Application.Features.Auth.Commands;

namespace SalesAPI.Presentation.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseApiController
    {
        [HttpPost("login")]
        public async Task<IActionResult> Create([FromBody] LoginCommand model, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(model, cancellationToken);
            return response.ToActionResult();
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToke([FromBody] RefreshTokenCommand model, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(model, cancellationToken);
            return response.ToActionResult();
        }
    }
}
