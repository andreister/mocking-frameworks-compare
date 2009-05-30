using System.Collections.Generic;
using MockingFrameworksCompare.ShoppingCartSample;
using NUnit.Framework;
using TypeMock;
using TypeMock.ArrangeActAssert;

namespace IsolatorTests
{
    /// <summary>
    /// As far as the goal is to compare the frameworks, the tests are named accordingly to a framework feature 
    /// they present, not to the actual scenario they test. The names are the same for all fixtures for all 
    /// frameworks, so that it's easy to compare the same test written with NMock2 or Moq or Rhino Mocks or Isolator.
    /// </summary>
    /// <remarks>
    /// Mostly, we mock <see cref="IWarehouse"/>. If instead we decided to mock a (not existing at the moment) 
    /// CLASS that implemented <see cref="IWarehouse"/>, we would have to make the methods virtual. The only 
    /// framework that DOES NOT have this requirement - Typemock Isolator.
    /// </remarks>
    [TestFixture]
    public class ShoppingCartTests
    {
        /// <summary>
        /// Mock IWharehouse.GetProducts() so that if called with the given parameter, 
        /// it would return the predefined list of products.
        /// </summary>
        [Test, Isolated]
        public void Test1_MockedMethod()
        {
            var warehouse = Isolate.Fake.Instance<IWarehouse>();
            Isolate.WhenCalled(() => warehouse.GetProducts("nail")).WillReturn(DefaultProducts);

            var cart = new ShoppingCart();
            cart.AddProducts("nail", warehouse);

            Assert.AreEqual(DefaultProducts.Count, cart.GetProductsCount(), "All products added to the cart should be counted.");
        }

        /// <summary>
        /// Mock IWharehouse.GetProducts() so that if called with any parameter, it would raise an event.
        /// </summary>
        [Test, Isolated]
        public void Test2_MockedEvent()
        {
            var warehouse = MockManager.MockObject<IWarehouse>();
            var alarm = warehouse.ExpectAddEvent("SomethingWentWrong");
            warehouse.ExpectRemoveEvent("SomethingWentWrong");
            warehouse.ExpectAndReturn("GetProducts", FireAndReturn(alarm, DefaultProducts));

            var cart = new ShoppingCart();
            cart.AddProducts("foo", warehouse.MockedInstance);

            Assert.IsTrue(cart.IsRed, "Cart should go to the 'red' state if there was a bad request to the wharehouse.");
        }

        /// <summary>
        /// Mock IWharehouse.IsAvailable and make sure GetProducts is never called in this case.
        /// </summary>
        [Test, Isolated]
        public void Test3_MockedProperty()
        {
            var warehouse = Isolate.Fake.Instance<IWarehouse>();
            Isolate.WhenCalled(() => warehouse.IsAvailable).WillReturn(false);

            var cart = new ShoppingCart();
            cart.AddProductsIfWarehouseAvailable("foo", warehouse);

            Isolate.Verify.WasNotCalled(() => warehouse.GetProducts(null));
        }

        /// <summary>
        /// Mock IWharehouse.GetProducts() and verify that it was called with the given parameter.
        /// </summary>
        [Test, Isolated]
        public void Test4_MockedArgument()
        {
            var warehouse = Isolate.Fake.Instance<IWarehouse>();
            Isolate.WhenCalled(() => warehouse.GetProducts(null)).WillReturn(new List<Product>());

            var cart = new ShoppingCart();
            cart.AddProducts("foo", warehouse);

            Isolate.Verify.WasCalledWithExactArguments(() => warehouse.GetProducts("foo"));
        }

        /// <summary>
        /// Mock ShoppingCart so that any call to it would invoke the original implementation, 
        /// but some explicitly specified methods would be mocked. (IWharehouse is mocked 
        /// here as well, just as a "byproduct").
        /// </summary>
        [Test]
        public void Test5_PartialMocks()
        {
            var warehouse = Isolate.Fake.Instance<IWarehouse>();
            var cart = Isolate.Fake.Instance<ShoppingCart>(Members.CallOriginal);
            Isolate.WhenCalled(() => warehouse.GetProducts(null)).WillReturn(DefaultProducts);
            Isolate.WhenCalled(() => cart.IsRed).WillReturn(true);

            cart.AddProducts("foo", warehouse);

            Assert.AreEqual(0, cart.GetProductsCount(), "No products can be added to a cart that is in the 'red' state.");
        }

        /// <summary>
        /// Mock ShoppingCart so that the specified call chain is mocked, without having to 
        /// mock all types that gets created along the way.
        /// </summary>
        [Test]
        public void Test6_RecursiveMocks()
        {
            string userName = "Andrew";
            var user = Isolate.Fake.Instance<User>(Members.ReturnRecursiveFakes);
            Isolate.WhenCalled(() => user.ContactDetails.Name).WillReturn(userName);

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

        /// <summary>
        /// You can write code directly in the expectation setup. A separate method is just 
        /// to make tests less verbose - since callback expectations tend to be quite long in all mocking frameworks.
        /// </summary>
        /// <param name="alarm">Event to fire during the call.</param>
        /// <param name="result">Result of the mocked call.</param>
        /// <returns></returns>
        private static DynamicReturnValue FireAndReturn(MockedEvent alarm, List<Product> result)
        {
            return (args, obj) => {
                alarm.Fire(null, new WarehouseEventArgs { BadRequest = true });
                return result;
            };
        }
    }
}
