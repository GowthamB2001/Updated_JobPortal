using JobPortal.WebApi.Middleware;

namespace JobPortal.WebApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder
            UseCustomExceptionMiddleware(
                this IApplicationBuilder app)
        {
            return app.UseMiddleware<
                ExceptionMiddleware>();
        }
    }
}
