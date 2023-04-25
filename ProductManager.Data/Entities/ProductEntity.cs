using ProductManager.Data.Entities.Common;

namespace ProductManager.Data.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Barcode { get; set; }
        public int PLU { get; set; }
    }
}
