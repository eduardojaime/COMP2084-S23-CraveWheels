using System.ComponentModel.DataAnnotations;

namespace CraveWheels.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        // Parent
        public Order? Order { get; set; } = default!;
    }
}
