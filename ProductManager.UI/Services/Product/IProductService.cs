using ProductManager.UI.Helpers;
using ProductManager.UI.Models;
using ProductManager.UI.RequestModels.Product;

namespace ProductManager.UI.Services.Product
{
    public interface IProductService
    {
        Task<PagedList<ProductModel>> GetProductsPaged(int pageIndex, int pageSize, string searchText = null);
        Task<ProductModel> GetById(int productId);
        Task RegenerateProducts();
        Task Remove(int productId);
        Task UpdateProduct(ProductModel product);
        Task AddProduct(AddProductRequest addProductRequest);
    }
}
