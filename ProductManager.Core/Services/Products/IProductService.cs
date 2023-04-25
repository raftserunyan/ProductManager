using ProductManager.Core.Models;
using ProductManager.Core.Services.Common;
using ProductManager.Data.Entities;

namespace ProductManager.Core.Services.Products
{
    public interface IProductService : ICommonService<ProductModel, ProductEntity>
    {
        Task GenerateProducts(int count);
    }
}
