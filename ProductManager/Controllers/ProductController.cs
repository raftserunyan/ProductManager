using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductManager.API.RequestModels;
using ProductManager.API.ViewModels.Product;
using ProductManager.Core.Models;
using ProductManager.Core.Services.Products;
using ProductManager.Data.Entities;
using ProductManager.Data.Specifications.Common;
using ProductManager.Data.Specifications.Products;
using ProductManager.Shared.Helpers;

namespace ProductManager.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }


        [HttpGet("generate")]
        public async Task<IActionResult> GenerateProducts()
        {
            await _productService.GenerateProducts(50000);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaged([FromQuery] int pageSize = 20, [FromQuery] int pageIndex = 1, [FromQuery] string searchText = null)
        {
            ICommonSpecification<ProductEntity> specification;

            if (String.IsNullOrWhiteSpace(searchText))
            {
                specification = new ProductsPagedSpecification(pageSize * (pageIndex - 1), pageSize);
            }
            else
            {
                specification = new ProductsFilteredAndPagedSpecification(pageSize * (pageIndex - 1), pageSize, searchText);
            }

            var productModels = await _productService.GetAllPaged(specification);
            return Ok(_mapper.Map<PagedList<ProductViewModel>>(productModels));
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById(int productId)
        {
            var productModel = await _productService.GetById(productId);

            return Ok(_mapper.Map<ProductViewModel>(productModel));
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductCreationRequest productCreationRequest)
        {
            var productModel = _mapper.Map<ProductModel>(productCreationRequest);

            var productId = await _productService.Add(productModel);

            return Ok(productId);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteById(int productId)
        {
            await _productService.Delete(productId);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductUpdateRequest productUpdateRequest)
        {
            var productModel = _mapper.Map<ProductModel>(productUpdateRequest);
            await _productService.Update(productModel);

            return Ok(_mapper.Map<ProductViewModel>(productModel));
        }
    }
}
