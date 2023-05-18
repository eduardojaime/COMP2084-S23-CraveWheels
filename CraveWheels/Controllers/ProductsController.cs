using Microsoft.AspNetCore.Mvc;

namespace CraveWheels.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: /products/create => displays an empty Product form
        public IActionResult Create()
        {
            return View();
        }
    }
}
