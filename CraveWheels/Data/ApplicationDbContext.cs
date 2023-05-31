using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CraveWheels.Models;

namespace CraveWheels.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets to perform CRUD for each model
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Restaurant> Restaurants { get; set;} = default!;
        public DbSet<CartItem> CartItems { get; set; } = default!;
        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = default!;
    }
}