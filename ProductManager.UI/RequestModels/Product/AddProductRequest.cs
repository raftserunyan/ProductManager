namespace ProductManager.UI.RequestModels.Product
{
    public class AddProductRequest
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Barcode { get; set; }
        public int PLU { get; set; }
    }
}
