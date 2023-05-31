using System.ComponentModel.DataAnnotations;

namespace CraveWheels.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
