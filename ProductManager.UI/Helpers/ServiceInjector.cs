using ProductManager.UI.Services.Http;
using ProductManager.UI.Services.Product;
using Radzen;

namespace ProductManager.UI.Helpers
{
    public static class ServiceInjector
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IHttpService, HttpService>();
            services.AddScoped<NotificationService>();
        }
    }
}
