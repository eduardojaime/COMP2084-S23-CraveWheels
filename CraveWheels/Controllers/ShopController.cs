using CraveWheels.Models;
using Microsoft.AspNetCore.Mvc;

namespace CraveWheels.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Shop/Restaurant
        public IActionResult Restaurant(string Name)
        {
            if (Name == null)
            {
                return RedirectToAction("Index");
            }

            // use the Product model to make a mock list of products to display
            var products = new List<Product>();

            for (var i = 1; i < 11; i++)
            {
                products.Add(new Product {  ProductId = i, Name = "Product " + i.ToString(), Price = 10 + i });
            }

            ViewData["Name"] = Name;
            // load the view and pass it the list of products we just made
            return View(products);
        }
    }
}
