using System.Collections.Generic;
using MockingFrameworksCompare.ShoppingCartSample;
using NMock2;
using NMock2.Actions;
using NUnit.Framework;

namespace NMock2Tests
{
    /// <summary>
    /// As far as the goal is to compare the frameworks, the tests are named accordingly to a framework feature 
    /// they present, not to the actual scenario they test. The names are the same for all fixtures for all 
    /// frameworks, so that it's easy to compare the same test written with NMock2 or Moq or Rhino Mocks or Isolator.
    /// </summary>
    /// <remarks>
    /// Here we mock interfaces only - NMock2 cannot mock classes (yet). Also, as soon as NMock2 allows *only* Strict 
    /// mocks (yet), *all* calls must be specified, so tests sometimes end up knowing a lot about inner implementation
    /// of the system.
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
            var warehouse = (IWarehouse)mockery.NewMock(typeof(IWarehouse));
            Expect.On(warehouse).Method("GetProducts").With("nail").Will(Return.Value(DefaultProducts));
            Expect.On(warehouse).EventAdd("SomethingWentWrong"); 
            Expect.On(warehouse).EventRemove("SomethingWentWrong"); 

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
            var warehouse = (IWarehouse) mockery.NewMock(typeof(IWarehouse));
            var fireBadRequest = new FireAction("SomethingWentWrong", null, new WarehouseEventArgs { BadRequest = true });
            Expect.On(warehouse).Method("GetProducts").WithAnyArguments().Will(fireBadRequest, Return.Value(DefaultProducts));
            Expect.On(warehouse).EventAdd("SomethingWentWrong");
            Expect.On(warehouse).EventRemove("SomethingWentWrong"); 
            Fire.Event("SomethingWentWrong").On(warehouse).With(new WarehouseEventArgs {BadRequest = true});

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
            var warehouse = (IWarehouse)mockery.NewMock(typeof(IWarehouse));
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
            var warehouse = (IWarehouse)mockery.NewMock(typeof(IWarehouse));
            Expect.On(warehouse).Method("GetProducts").With(Is.EqualTo("foo")).Will(Return.Value(DefaultProducts));
            Expect.On(warehouse).EventAdd("SomethingWentWrong");
            Expect.On(warehouse).EventRemove("SomethingWentWrong"); 

            var cart = new ShoppingCart();
            cart.AddProducts("foo", warehouse);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Mock <see cref="ShoppingCart"/> so that any call to it would invoke the original implementation, 
        /// but some explicitly specified methods would be mocked. 
        /// </summary>
        [Test]
        public void Test5_PartialMocks()
        {
            //It's impossible to do with NMock2.
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
