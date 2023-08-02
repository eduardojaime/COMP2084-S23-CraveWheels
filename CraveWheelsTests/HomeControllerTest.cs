using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// local import > not recommended, move to Usings.cs to import globally
// using CraveWheels; 
// using CraveWheels.Controllers;

namespace CraveWheelsTests
{
    [TestClass]
    public class HomeControllerTest
    {
        // what to test? 100%???
        // what's important?
        [TestMethod]
        public void IndexReturnsResult() { 
            // Arrange
            var controller = new HomeController(null); // null logger instance
            // Act
            var result = controller.Index();
            // Assert > are we returning something?
            Assert.IsNotNull(result);
        }
    }
}
