using CraveWheels.Data;
using CraveWheels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;
using CraveWheels.Extensions;

namespace CraveWheels.Controllers
{
    public class ShopController : Controller
    {
        // db connection
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        // constructor that requires an instance of the db connection
        public ShopController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; // Loads app settings values as an object
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
        public IActionResult AddToCart([FromForm] int ProductId, [FromForm] int Quantity)
        {
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

        // GET: /Shop/Cart
        public IActionResult Cart()
        {
            // get customerid
            var customerId = GetCustomerId();
            // get cart items as a list
            var cartItems = _context.CartItems // SELECT* FROM CartItems c
                .Include(c => c.Product) // JOIN Products p ON c.ProductId = p.ProductId
                                         // method chaining
                .Where(c => c.CustomerId == customerId) // WHERE CustomerId = @ 
                .OrderByDescending(c => c.Product.Name) // ORDER BY p.Name DESC
                .ToList();
            // return list to view
            // TODO: calculate total amount of cart and return to view
            // SELECT SUM(c.Price) FROM CartItems c
            var total = cartItems.Sum(c => c.Price).ToString("C");
            ViewBag.TotalAmount = total;

            return View(cartItems);
        }

        public IActionResult RemoveFromCart(int id)
        {
            // find cartitem 
            var cartItem = _context.CartItems.Find(id);
            // remove from db
            _context.CartItems.Remove(cartItem);
            // save changes
            _context.SaveChanges();
            // redirect back to cart page
            return RedirectToAction("Cart");
        }

        // TODO: Protected Checkout > only authenticated users can complete a purchase
        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken] // ties the form that was loaded in the GET version of this method
        // to this POST request
        public IActionResult Checkout(
            [Bind("FirstName, LastName, Address, City, Province, PostalCode")] Models.Order order)
        {
            // populate order date
            order.OrderDate = DateTime.UtcNow; // BEST PRACTICE
            order.CustomerId = GetCustomerId();
            // calculate totals
            var cartItems = _context.CartItems // SELECT* FROM CartItems c
                .Include(c => c.Product) // JOIN Products p ON c.ProductId = p.ProductId
                                         // method chaining
                .Where(c => c.CustomerId == order.CustomerId) // WHERE CustomerId = @ 
                .OrderByDescending(c => c.Product.Name) // ORDER BY p.Name DESC
                .ToList();
            // return list to view
            // TODO: calculate total amount of cart and return to view
            // SELECT SUM(c.Price) FROM CartItems c
            decimal total = cartItems.Sum(c => c.Price);
            order.OrderTotal = total;
            // store in session object, this will allow me to load this object after user pays
            HttpContext.Session.SetObject("Order", order); // this is a temporary value
            // redirect to payment page
            return RedirectToAction("Payment");
        }

        public IActionResult Payment() {
            // retrieve order to get total amount
            var order = HttpContext.Session.GetObject<Models.Order>("Order");
            // pass total amount to view
            ViewBag.Total = order.OrderTotal;
            // pass publishable key to view in order to use the JS code
            ViewBag.PublishableKey = _configuration["Payments:Stripe:PublishableKey"];
            // return view
            return View();
        }

        // TODO: Payment POST method > create payment session and return session id for js to redirect

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
                else
                {
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
