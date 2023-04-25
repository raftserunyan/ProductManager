using Microsoft.Extensions.DependencyInjection;
using ProductManager.Data.UnitOfWork;

namespace ProductManager.Data.Extensions
{
    public static class DataServicesInjector
    {
        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        }
    }
}
