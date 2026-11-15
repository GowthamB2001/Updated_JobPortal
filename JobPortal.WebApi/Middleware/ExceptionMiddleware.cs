
using JobPortal.WebApi.Models;
using System.Net;
using System.Text.Json;

namespace JobPortal.WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log complete exception details
                Console.Error.WriteLine("========== API EXCEPTION ==========");
                Console.Error.WriteLine(ex.ToString());
                Console.Error.WriteLine("===================================");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            var response = new ErrorResponse
            {
                Success = false,
                Message = exception.Message,
                StatusCode = context.Response.StatusCode
            };

            return context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
