namespace SaleAPI.Presentation.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using SaleAPI.Presentation.Middlewares;

    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }

}
