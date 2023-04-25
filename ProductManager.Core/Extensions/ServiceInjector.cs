using Microsoft.Extensions.DependencyInjection;
using ProductManager.Core.Services.Common;
using ProductManager.Core.Services.Products;

namespace ProductManager.Core.Extensions
{
    public static class ServiceInjector
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
