using System;
using System.Collections.Generic;
using MockingFrameworksCompare.ShoppingCartSample;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace RhinoMocksTests
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
        /// Mock <see cref="IWarehouse.GetProducts"/> so that if called with the given parameter, 
        /// it would return the predefined list of products.
        /// </summary>
        [Test]
        public void Test1_MockedMethod()
        {
            var warehouse = MockRepository.GenerateMock<IWarehouse>();
            warehouse.Stub(x => x.GetProducts(null)).Constraints(Is.Equal("nail")).Return(DefaultProducts);

            var cart = new ShoppingCart();
            cart.AddProducts("nail", warehouse);

            Assert.AreEqual(DefaultProducts.Count, cart.GetProductsCount(), "All products added to the cart should be counted.");
        }

        /// <summary>
        /// Mock <see cref="IWarehouse.GetProducts"/> so that if called with any parameter, it would raise an event
        /// (that gets handled by shopping cart and updates its state).
        /// </summary>
        [Test]
        public void Test2_MockedEvent()
        {
            var warehouse = MockRepository.GenerateMock<IWarehouse>();
            warehouse.Stub(x => x.GetProducts(null)).Constraints(Is.Anything()).WhenCalled(RaiseBadRequest(warehouse)).Return(DefaultProducts);
            
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
            var warehouse = MockRepository.GenerateMock<IWarehouse>();
            warehouse.Stub(x => x.IsAvailable).Return(false);
            warehouse.Expect(x => x.GetProducts(null)).Constraints(Is.Anything()).Repeat.Never();

            var cart = new ShoppingCart();
            cart.AddProductsIfWarehouseAvailable("foo", warehouse);
        }

        /// <summary>
        /// Mock <see cref="IWarehouse.GetProducts"/> and verify that it was called precisely with the given argument.
        /// </summary>
        [Test]
        public void Test4_MockedArgument()
        {
            var warehouse = MockRepository.GenerateMock<IWarehouse>();
            warehouse.Expect(x => x.GetProducts(null)).Constraints(Is.Equal("foo")).Return(DefaultProducts);

            var cart = new ShoppingCart();
            cart.AddProducts("foo", warehouse);

            warehouse.VerifyAllExpectations();
        }

        /// <summary>
        /// Mock <see cref="ShoppingCart"/> so that any call to it would invoke the original implementation, 
        /// but some explicitly specified methods would be mocked. (<see cref="IWarehouse"/> is mocked 
        /// here as well, just as a "byproduct").
        /// </summary>
        [Test]
        public void Test5_PartialMocks()
        {
            var warehouse = MockRepository.GenerateMock<IWarehouse>();
            warehouse.Expect(x => x.GetProducts(null)).Constraints(Is.Anything()).Return(DefaultProducts);
            var mocks = new MockRepository();
            var cart = mocks.PartialMock<ShoppingCart>();
            cart.Expect(x => x.IsRed).Return(true);
            mocks.ReplayAll();

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
            var user = MockRepository.GenerateMock<User>();
            user.Expect(x => x.ContactDetails.Name).Return(userName);

            var cart = new ShoppingCart { User = user };
            string thankYouMessage = cart.ThankYou();

            Assert.IsTrue(thankYouMessage.Contains(userName), "'Thank you' message on the cart should contain user name.");
        }

        /// <summary>
        /// You can write code directly in the expectation setup. A separate method is just 
        /// to make tests less verbose - since callback expectations tend to be quite long in all mocking frameworks.
        /// </summary>
        /// <param name="warehouse"></param>
        /// <returns></returns>
        private static Action<MethodInvocation> RaiseBadRequest(IWarehouse warehouse)
        {
            return y => warehouse.Raise(it => it.SomethingWentWrong += null, null, new WarehouseEventArgs { BadRequest = true });
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
