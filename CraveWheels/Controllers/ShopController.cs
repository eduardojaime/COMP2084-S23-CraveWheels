using CraveWheels.Data;
using CraveWheels.Models;
using Microsoft.AspNetCore.Mvc;

namespace CraveWheels.Controllers
{
    public class ShopController : Controller
    {
        // db connection
        private readonly ApplicationDbContext _context;

        // constructor that requires an instance of the db connection
        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // fetch restaurants from db using query
            var restaurants = _context.Restaurants.OrderBy(r => r.Name).ToList();

            // pass query result to the view
            return View(restaurants);
        }

        // GET: /Shop/Restaurant/3
        public IActionResult Restaurant(int id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            // fetch products for selected Restaurant
            var products = _context.Products.Where(p => p.RestaurantId == id).OrderBy(p => p.Name).ToList();

            ViewData["Name"] = _context.Restaurants.Find(id).Name;
            return View(products);
        }

        //public IActionResult Restaurant(string Name)
        //{
        //    if (Name == null)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    // use the Product model to make a mock list of products to display
        //    var products = new List<Product>();

        //    for (var i = 1; i < 11; i++)
        //    {
        //        products.Add(new Product {  ProductId = i, Name = "Product " + i.ToString(), Price = 10 + i });
        //    }

        //    ViewData["Name"] = Name;
        //    // load the view and pass it the list of products we just made
        //    return View(products);
        //}
    }
}
