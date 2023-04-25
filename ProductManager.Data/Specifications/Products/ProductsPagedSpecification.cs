using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductManager.Data.Entities;
using ProductManager.Data.Specifications.Common;

namespace ProductManager.Data.Specifications.Products
{
    public class ProductsPagedSpecification : CommonSpecification<ProductEntity>
    {
        public ProductsPagedSpecification(int skip, int take)
        {
            ApplyPaging(skip, take);
        }
    }
}
