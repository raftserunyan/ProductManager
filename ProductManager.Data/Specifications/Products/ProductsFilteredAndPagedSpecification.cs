using ProductManager.Data.Entities;
using ProductManager.Data.Specifications.Common;

namespace ProductManager.Data.Specifications.Products
{
    public class ProductsFilteredAndPagedSpecification : CommonSpecification<ProductEntity>, ICommonSpecification<ProductEntity>
    {
        public ProductsFilteredAndPagedSpecification(int skip, int take, string searchText) : base(x => x.Barcode.Contains(searchText) || x.PLU.ToString().Contains(searchText))
        {
            ApplyPaging(skip, take);
        }
    }
}
