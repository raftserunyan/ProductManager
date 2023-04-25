namespace ProductManager.API.ViewModels.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Barcode { get; set; }
        public int PLU { get; set; }
    }
}
