namespace CraveWheelsTests
{
    [TestClass]
    public class UnitTest1
    {
        // TODO: Add tests
        [TestMethod]
        public void Testing2plus2is4()
        {
            // Arrange
            int x = 2; int y = 2;
            // Act
            int result = x + y; // calling functionality involved
            // Assert
            Assert.AreEqual(4, result);
        }
    }
}