using ProductManager.API.Middlewares;

namespace ProductManager.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlingMiddleware>();

            return app;
        }
    }
}
