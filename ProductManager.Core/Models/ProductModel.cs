using ProductManager.Core.Models.Common;

namespace ProductManager.Core.Models
{
    public class ProductModel : BaseModel
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Barcode { get; set; }
        public int PLU { get; set; }
    }
}
