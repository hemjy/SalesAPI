using Microsoft.AspNetCore.Mvc;
using SalesAPI.Application.Commons;

namespace SaleAPI.Presentation.Extensions
{
    public static class ActionResultExtensions
    {

      
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            return result.Succeeded ? new OkObjectResult(result) : new BadRequestObjectResult(result);
        }
    }
}

