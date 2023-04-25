using AutoMapper;
using ProductManager.API.RequestModels;
using ProductManager.API.ViewModels.Product;
using ProductManager.Core.Models;
using ProductManager.Shared.Helpers;

namespace ProductManager.API.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductViewModel, ProductModel>().ReverseMap();
            CreateMap<ProductCreationRequest, ProductModel>().ReverseMap();
            CreateMap<ProductUpdateRequest, ProductModel>().ReverseMap();
            CreateMap<PagedList<ProductModel>, PagedList<ProductViewModel>>();
        }
    }
}
