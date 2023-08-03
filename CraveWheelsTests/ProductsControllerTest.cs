using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraveWheelsTests
{
    [TestClass]
    public class ProductsControllerTest
    {
        // set up properties/objects/variables
        private ProductsController _controller; // null
        private ApplicationDbContext _context;
        private List<Product> _products;
        // Initialize mock data (class level Arrage step)
        [TestInitialize]
        public void TestInitialize()
        {
            // initialize in-memory db context
            // similar to registering it in Program.cs
            // install Microsoft.EntityFrameworkCore.InMemory
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            // mock some data (1 restaurant and 3 products)
            var restaurant = new Restaurant { Id = 1, Name = "Beertown" };
            var product1 = new Product { ProductId = 1, Name = "3xTacos", Price = 10, Restaurant = restaurant };
            var product2 = new Product { ProductId = 2, Name = "Quesadilla", Price = 8, Restaurant = restaurant };
            var product3 = new Product { ProductId = 3, Name = "Beer and Chips", Price = 20, Restaurant = restaurant };
            // add mock data to mock db
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();
            _context.Products.Add(product1);
            _context.Products.Add(product2);
            _context.Products.Add(product3);
            _context.SaveChanges();
            // add products to local list to compare when making modifications
            _products = new List<Product>();
            _products.Add(product1);
            _products.Add(product2);
            _products.Add(product3);
            // initialize controller object
            _controller = new ProductsController(_context);
        }

        // Add test methods
        // Test 1 > make sure index loads something
        [TestMethod]
        public void IndexReturnsView() {
            // skip Arrange step since TestInitialize() takes care of this
            var result = _controller.Index();
            Assert.IsNotNull(result);
        }
        // Test 2 > Index() loads data from all restaurant
        [TestMethod]
        public void IndexReturnsProductData() {
            // skip Arrange, data is mocked in TestInitialize()
            // Act > call index() and access model data
            var result = _controller.Index();
            // extract view
            var view = (ViewResult)result.Result;
            List<Product> model = (List<Product>)view.Model;
            // what's in the mock db is the same as what's in my local list for comparison
            Assert.AreEqual(_products.Count, model.Count);
        }
        // Test 3 > making sure I get a NotFound result if I try to get details from a non-existant ID
        [TestMethod]
        public void DetailsReturnsNotFoundIfInvalidID() {
            var testId = 100;
            var result = _controller.Details(testId);
            var notFoundResult = (NotFoundResult)result.Result;
            // not found is code 404
            Assert.AreEqual(404, notFoundResult.StatusCode); // all results include a status code
        }
        // Test 4 > making sure I can add a new product
        [TestMethod]
        public void CreateAddsProductToDB() {
            var newProduct = new Product
            {
                ProductId = 4,
                Name = "Test",
                Price = 1,
                Restaurant = new Restaurant { Id = 2, Name = "FoodHall" }
            };
            _controller.Create(newProduct, null);
            // At this point the count of products in my db should be 4
            Assert.AreEqual(4, _context.Products.ToList().Count);
        }
    }
}
