using System.Collections.Generic;
using MockingFrameworksCompare.ShoppingCartSample;
using NUnit.Framework;

namespace TypemockIsolatorTests
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
    /// </remarks>
    [TestFixture]
    public class ShoppingCartTests
    {
        /// <summary>
        /// Mock IWharehouse.GetProducts() so that if called with the given parameter, 
        /// it would return the predefined list of products.
        /// </summary>
        [Test]
        public void Test1_MockedMethod()
        {
        }

        /// <summary>
        /// Mock IWharehouse.GetProducts() so that if called with any parameter, it would raise an event.
        /// </summary>
        [Test]
        public void Test2_MockedEvent()
        {
        }

        /// <summary>
        /// Mock IWharehouse.IsAvailable and make sure GetProducts is never called in this case.
        /// </summary>
        [Test]
        public void Test3_MockedProperty()
        {
        }

        /// <summary>
        /// Mock IWharehouse.GetProducts() and verify that it was called with the given parameter.
        /// </summary>
        [Test]
        public void Test4_MockedArgument()
        {
        }

        /// <summary>
        /// Mock ShoppingCart so that any call to it would invoke the original implementation, 
        /// but some explicitly specified methods would be mocked. (IWharehouse is mocked 
        /// here as well, just as a "byproduct").
        /// </summary>
        [Test]
        public void Test5_PartialMocks()
        {
        }

        /// <summary>
        /// Mock ShoppingCart so that the specified call chain is mocked, without having to 
        /// mock all types that gets created along the way.
        /// </summary>
        [Test]
        public void Test6_RecursiveMocks()
        {
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
