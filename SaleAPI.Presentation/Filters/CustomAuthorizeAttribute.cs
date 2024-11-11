using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc.Filters;
using SalesAPI.Domain.Exceptions;

namespace SalesAPI.Presentation.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var path = context.HttpContext.Request.Path.ToString().ToLower();
            bool isUnauthorised = !context.HttpContext.User.Identity.IsAuthenticated && !IsAllowAnonymous(context);
        

            if (isUnauthorised)
            {
                throw new UnauthorizedAccessException("You are not authorized.");
            }


          
        }

        private static bool IsAllowAnonymous(AuthorizationFilterContext context)
        {
            // Check if the action is decorated with AllowAnonymous attribute
            return context.ActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        }
    }
}
