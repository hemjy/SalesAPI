using SalesAPI.Application.Commons;
using SalesAPI.Domain.Exceptions;
using Serilog;
using Serilog.Configuration;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SaleAPI.Presentation.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                Log.Information(ex, ex.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await HandleExceptionAsync(httpContext, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Warning(ex, "Unauthorized access");
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await HandleExceptionAsync(httpContext, ex.Message);
            }
            catch (HttpBadForbiddenException ex)
            {
                Log.Information(ex, ex.Message);
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await HandleExceptionAsync(httpContext, ex.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "exception occurred");
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await HandleExceptionAsync(httpContext, "An unexpected error occurred");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";
            var response = Result<string>.Failure(message);
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
