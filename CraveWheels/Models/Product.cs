using System.ComponentModel.DataAnnotations;

namespace CraveWheels.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        [Range(0.01, 10000)]
        [DisplayFormat(DataFormatString = "{0:c}")]  // uses MS currency format
        public decimal Price { get; set; }

        public string? Photo { get; set; }

        // Parent Reference
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; } = default!;

        // Child Reference
        public List<CartItem>? CartItems { get; set; }
    }
}
