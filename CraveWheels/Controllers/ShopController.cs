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
        public IActionResult Restaurant()
        {
            return View();
        }
    }
}
