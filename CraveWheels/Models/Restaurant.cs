using System.ComponentModel.DataAnnotations;

namespace CraveWheels.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // child reference to Products (1 Restaurant => Many Products)
        public List<Product>? Products { get; set; } = default!;
        public List<Order>? Orders { get; set; } = default!;
        
    }
}
