using ProductManager.UI.Helpers;
using ProductManager.UI.Models;
using ProductManager.UI.RequestModels.Product;
using ProductManager.UI.Services.Http;

namespace ProductManager.UI.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IHttpService _httpService;

        public ProductService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task AddProduct(AddProductRequest addProductRequest)
        {
            var queryPath = $"products";

            await _httpService.MakeRequest<AddProductRequest, int>(HttpMethod.Post, queryPath, addProductRequest);
        }

        public async Task<ProductModel> GetById(int productId)
        {
            var queryPath = $"products/{productId}";
            var product = await _httpService.MakeRequest<string, ProductModel>(HttpMethod.Get, queryPath, null);

            return product;
        }

        public async Task<PagedList<ProductModel>> GetProductsPaged(int pageIndex = 1, int pageSize = 20)
        {
            var queryPath = $"products?pageSize={pageSize}&pageIndex={pageIndex}";

            var productsPaged = await _httpService.MakeRequest<string, PagedList<ProductModel>>(HttpMethod.Get, queryPath, null);

            return productsPaged;
        }

        public async Task RegenerateProducts()
        {
            var queryPath = $"products/generate";

            await _httpService.MakeRequest<string, string>(HttpMethod.Get, queryPath, null);
        }

        public async Task Remove(int productId)
        {
            var queryPath = $"products/{productId}";

            await _httpService.MakeRequest<string, string>(HttpMethod.Delete, queryPath, null);
        }

        public async Task UpdateProduct(ProductModel product)
        {
            var queryPath = $"products";

            await _httpService.MakeRequest<ProductModel, ProductModel>(HttpMethod.Put, queryPath, product);
        }
    }
}
