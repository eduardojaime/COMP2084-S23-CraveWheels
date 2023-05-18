using System.ComponentModel.DataAnnotations;

namespace CraveWheels.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        [Range(0.01, 10000)]
        public decimal Price { get; set; }
    }
}
