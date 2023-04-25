using AutoMapper;
using ProductManager.Core.Models;
using ProductManager.Data.Entities;
using ProductManager.Shared.Helpers;

namespace ProductManager.Core.MapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductEntity, ProductModel>().ReverseMap();
            CreateMap<ProductEntity, ProductEntity>();
            CreateMap<PagedList<ProductEntity>, PagedList<ProductModel>>();
        }
    }
}
