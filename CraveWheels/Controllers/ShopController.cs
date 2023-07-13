using CraveWheels.Data;
using CraveWheels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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
            var products = _context.Products
                .Where(p => p.RestaurantId == id) // Category
                .OrderBy(p => p.Name).ToList();

            ViewData["Name"] = _context.Restaurants.Find(id).Name;
            return View(products);
        }

        // POST: /Shop/AddToCart
        public IActionResult AddToCart([FromForm] int ProductId, [FromForm] int Quantity) {
            // retrieve Id to identify the current user session
            var customerId = GetCustomerId();
            // retrieve price from db
            var price = _context.Products.Find(ProductId).Price;
            // create and save cart object
            var cart = new CartItem()
            {
                ProductId = ProductId,
                Quantity = Quantity,
                Price = price,
                CustomerId = customerId
            };
            // save changes to the db
            _context.CartItems.Add(cart);
            _context.SaveChanges();
            // redirect to /Shop/Cart
            return Redirect("Cart");
        }

        // Helper Method
        // Retrieves or generates ID to identify user
        private string GetCustomerId()
        {
            // variable to store the ID temporarily
            string customerId = string.Empty;
            // check the session object for a customer id
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("CustomerId")))
            {
                // if user is authenticated, use email
                if (User.Identity.IsAuthenticated)
                {
                    customerId = User.Identity.Name;
                }
                // else use a GUID
                else {
                    customerId = Guid.NewGuid().ToString();
                }
                // update session value
                HttpContext.Session.SetString("CustomerId", customerId);
            }
            // return whatever value is in the session object
            return HttpContext.Session.GetString("CustomerId");
        }
    }
}
