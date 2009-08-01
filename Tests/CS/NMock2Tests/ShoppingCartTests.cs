using System.Collections.Generic;
using MockingFrameworksCompare.ShoppingCartSample;
using NMock2;
using NMock2.Actions;
using NUnit.Framework;
using Is=NUnit.Framework.Is;

namespace NMock2Tests
{
    /// <summary>
    /// As far as the goal is to compare the frameworks, the tests are named accordingly to a framework feature 
    /// they present, not to the actual scenario they test. The names are the same for all fixtures for all 
    /// frameworks, so that it's easy to compare the same test written with NMock2 or Moq or Rhino Mocks or Isolator.
    /// </summary>
    /// <remarks>
    /// Here we mock interfaces only - NMock2 cannot mock classes (yet). 
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
            var mockery = new Mockery();
            var warehouse = mockery.NewMock<IWarehouse>(MockStyle.Stub);
            Expect.On(warehouse).Method("GetProducts").With("nail").Will(Return.Value(DefaultProducts));

            var cart = new ShoppingCart();
            cart.AddProducts("nail", warehouse);

            Assert.AreEqual(DefaultProducts.Count, cart.GetProductsCount(), "All products added to the cart should be counted.");
        }

        /// <summary>
        /// Mock <see cref="IWarehouse.GetProducts"/> so that if called with any parameter, it would raise an event.
        /// </summary>
        [Test]
        public void Test2_MockedEvent()
        {
            var mockery = new Mockery();
            var warehouse = mockery.NewMock<IWarehouse>(MockStyle.Stub);
            var fireBadRequest = new FireAction("SomethingWentWrong", null, new WarehouseEventArgs { BadRequest = true });
            Expect.On(warehouse).Method("GetProducts").WithAnyArguments().Will(fireBadRequest, Return.Value(DefaultProducts));

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
            var mockery = new Mockery();
            var warehouse = mockery.NewMock<IWarehouse>(MockStyle.Stub);
            Expect.On(warehouse).GetProperty("IsAvailable").Will(Return.Value(false)); 
            Expect.Never.On(warehouse).Method("GetProducts").WithAnyArguments();

            var cart = new ShoppingCart();
            cart.AddProductsIfWarehouseAvailable("foo", warehouse);
        }

        /// <summary>
        /// Mock <see cref="IWarehouse.GetProducts"/> and verify that it was called precisely with the given argument.
        /// </summary>
        [Test]
        public void Test4_MockedArgument()
        {
            var mockery = new Mockery();
            var warehouse = mockery.NewMock<IWarehouse>(MockStyle.Stub);
            Expect.On(warehouse).Method("GetProducts").With(Is.EqualTo("foo")).Will(Return.Value(DefaultProducts));

            var cart = new ShoppingCart();
            cart.AddProducts("foo", warehouse);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Mock <see cref="ShoppingCart"/> so that any call to it would invoke the original implementation, 
        /// but some explicitly specified methods would be mocked. (<see cref="IWarehouse"/> is mocked 
        /// here as well, just as a "byproduct").
        /// </summary>
        [Test]
        public void Test5_PartialMocks()
        {
            var mockery = new Mockery();
            var warehouse = mockery.NewMock<IWarehouse>(MockStyle.Stub);
            var cart = mockery.NewMock<ShoppingCart>(MockStyle.Transparent);

            Expect.On(warehouse).Method("GetProducts").With(Is.Anything).Will(Return.Value(DefaultProducts));
            Expect.On(cart).GetProperty("IsRed").Will(Return.Value(true));
            
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
            //It's impossible to do with NMock2.
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
