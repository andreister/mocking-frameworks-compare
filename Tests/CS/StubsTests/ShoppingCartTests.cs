using System.Collections.Generic;
using MockingFrameworksCompare.ShoppingCartSample;
using NUnit.Framework;
using MockingFrameworksCompare.ShoppingCartSample.Stubs;

namespace StubsTests
{
    /// <summary>
    /// As far as the goal is to compare the frameworks, the tests are named accordingly to a framework feature 
    /// they present, not to the actual scenario they test. The names are the same for all fixtures for all 
    /// frameworks, so that it's easy to compare the same test written with NMock2 or Moq or Rhino Mocks or Isolator.
    /// </summary>
    /// <remarks>
    /// Mostly, we mock <see cref="IWarehouse"/>. If instead we decided to mock a (not existing at the moment) 
    /// class that implemented <see cref="IWarehouse"/>, we would have to make the methods virtual. The only 
    /// framework that DOES NOT have this requirement - Typemock Isolator.
    /// 
    /// These tests use Stubs. Stubs home page: http://research.microsoft.com/stubs
    /// </remarks>
    [TestFixture]
    public class ShoppingCartTests
    {
        /// <summary>
        /// Mock <see cref="IWarehouse.GetProducts"/> so that if called with the given parameter, 
        /// it would return the predefined list of products.
        /// </summary>
        [Test]
        public void Test1_MockedMethod()
        {
            var warehouse = new SIWarehouse();
            warehouse.GetProducts = (productName) =>
                {
                    Assert.AreEqual("nail", productName);
                    return DefaultProducts;
                };

            var cart = new ShoppingCart();
            cart.AddProducts("nail", warehouse);

            Assert.AreEqual(
                DefaultProducts.Count, 
                cart.GetProductsCount(), "All products added to the cart should be counted.");
        }

        /// <summary>
        /// Mock <see cref="IWarehouse.GetProducts"/> so that if called with any parameter, it would raise an event
        /// (that gets handled by shopping cart and updates <see cref="ShoppingCart.IsRed"/> state).
        /// </summary>
        [Test]
        public void Test2_MockedEvent()
        {
            var warehouse = new SIWarehouse();
            warehouse.GetProducts = p =>
            {
                warehouse.SomethingWentWrong(warehouse, new WarehouseEventArgs {BadRequest = true });
                return new List<Product>();
            };

            var cart = new ShoppingCart();
            cart.AddProducts("foo", warehouse);

            Assert.IsTrue(cart.IsRed, "Cart should go to the 'red' state if there was a bad request to the wharehouse.");
        }

        /// <summary>
        /// Mock <see cref="IWarehouse.IsAvailable"/> and make sure <see cref="IWarehouse.GetProducts"/> is never called in this case.
        /// </summary>
        [Test]
        public void Test3_MockedProperty()
        {
            var warehouse = new SIWarehouse();
            bool isAvailableCalled = false;
            warehouse.IsAvailableGet = () =>
            {
                isAvailableCalled = true;
                return false;
            };
            warehouse.GetProducts = (p) =>
            {
                Assert.Fail("should never be called");
                return null;
            };

            var cart = new ShoppingCart();
            cart.AddProductsIfWarehouseAvailable("foo", warehouse);

            Assert.IsTrue(isAvailableCalled);
        }

        /// <summary>
        /// Mock <see cref="IWarehouse.GetProducts"/> and verify that it was called precisely with the given argument.
        /// </summary>
        [Test]
        public void Test4_MockedArgument()
        {
            var warehouse = new SIWarehouse();
            string name = "foo";
            bool getProductsCalled = false;
            warehouse.GetProducts = (p) =>
                {
                    getProductsCalled = true;
                    Assert.AreEqual(name, p);
                    return new List<Product>();
                };

            var cart = new ShoppingCart();
            cart.AddProducts("foo", warehouse);

            Assert.IsTrue(getProductsCalled);
        }

        /// <summary>
        /// Mock <see cref="ShoppingCart"/> so that any call to it would invoke the original implementation, 
        /// but some explicitly specified methods would be mocked. (<see cref="IWarehouse"/> is mocked 
        /// here as well, just as a "byproduct").
        /// </summary>
        [Test]
        [Ignore("not supported by stubs")]
        public void Test5_PartialMocks()
        {
            var warehouse = new SIWarehouse();
            var cart = new SShoppingCart();

            warehouse.GetProducts = (p) => DefaultProducts;
            cart.IsRedGet = () => true;

            cart.AddProducts("foo", warehouse);

            Assert.AreEqual(0, cart.GetProductsCount(), "No products can be added to a cart that is in the 'red' state.");
        }

        /// <summary>
        /// Mock <see cref="ShoppingCart"/> so that the specified call chain is mocked, without having to 
        /// mock all types that gets created along the way.
        /// </summary>
        [Test]
        public void Test6_RecursiveMocks()
        {
            string userName = "Andrew";
            var user = new SUser();

            user.ContactDetailsGetAsStub<SContactDetails>().NameGet = () => userName;

            var cart = new ShoppingCart { User = user };
            string thankYouMessage = cart.ThankYou();

            Assert.IsTrue(thankYouMessage.Contains(userName), "'Thank you' message on the cart should contain user name.");
        }

        /// <summary>
        /// You can write code directly in the expectation setup. A separate property is just 
        /// to make tests less verbose.
        /// </summary>
        private static List<Product> DefaultProducts
        {
            get { return new List<Product> { new Product("nail", 10), new Product("snail", 5) }; }
        }
    }
}