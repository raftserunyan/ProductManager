using System.ComponentModel.DataAnnotations;

namespace ProductManager.API.RequestModels
{
    public class ProductCreationRequest
    {
        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string Name { get; set; }

        [Required]
        [Range(10, 5000)]
        public double Price { get; set; }

        public string Barcode { get; set; }

        [Required]
        [Range(1, 99999)]
        public int PLU { get; set; }
    }
}
