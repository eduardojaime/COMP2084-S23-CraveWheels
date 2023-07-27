using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CraveWheels.Data;
using CraveWheels.Models;
using Microsoft.AspNetCore.Authorization;

namespace CraveWheels.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public IActionResult Index()
        {
            if (User.IsInRole("Administrator"))
            {
                // show all orders in system
                var orders = _context.Orders.OrderByDescending(o => o.OrderDate).ToList();
                return View(orders);
            }
            else {
                // show orders filtered by customerId (user name)
                var filteredOrders = _context.Orders
                    .Where(o => o.CustomerId == User.Identity.Name)
                    .OrderByDescending(o => o.OrderDate)
                    .ToList();
                return View(filteredOrders);
            }
        }

        // GET: Orders/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = _context.Orders // SELECT * FROM Orders o
                .Include(o => o.OrderDetails) // JOIN OrderDetails d ON o.OrderId == ...
                .ThenInclude(d => d.Product) // JOIND Products p ON d.ProductId == ...
                .ThenInclude(p => p.Restaurant) // JOIN Restaurant on p.RestaurantId = ...
                .FirstOrDefault(o => o.OrderId == id); // WHERE o.OrderId = @id
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // REMOVE EDIT, and DELETE
    }
}
