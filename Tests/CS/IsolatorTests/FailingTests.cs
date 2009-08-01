using MockingFrameworksCompare.BrainSample;
using MockingFrameworksCompare.ShoppingCartSample;
using NUnit.Framework;
using TypeMock.ArrangeActAssert;

namespace IsolatorTests
{
    /// <summary>
    /// Those tests fail - the purpose is to assess error messages that come from the mocking framework.
    /// </summary>
    public class FailingTests
    {
        /// <summary>
        /// Per our setup, BurnException is always thrown so "mouth.Yell" is always called. 
        /// Imagine it's not what we want (the setup is incorrect), and the test would fail. 
        /// Which error message would we get? 
        /// </summary>
        public void CallOnceExpectNever()
        {
            var hand = Isolate.Fake.Instance<Hand>();
            var mouth = Isolate.Fake.Instance<Mouth>();
            Isolate.WhenCalled(() => hand.TouchIron(null)).WillThrow(new BurnException());

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron());

            Isolate.Verify.WasNotCalled(() => mouth.Yell());
        }

        /// <summary>
        /// Per our setup, BurnException is never thrown so "mouth.Yell" is never called. 
        /// Imagine it's not what we want (the setup is incorrect), and the test would fail. 
        /// Which error message would we get? 
        /// </summary>
        public void CallNeverExpectOnce()
        {
            var hand = Isolate.Fake.Instance<Hand>();
            var mouth = Isolate.Fake.Instance<Mouth>();
            Isolate.WhenCalled(() => hand.TouchIron(null)); 

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron {IsHot = true});

            Isolate.Verify.WasCalledWithAnyArguments(() => mouth.Yell()); 
        }

        /// <summary>
        /// Per our setup, BurnException is never thrown so "mouth.Yell" is never called. 
        /// Imagine it's not what we want (the setup is incorrect), and the test would fail. 
        /// Can we provide a custom message for that? 
        /// </summary>
        public void FailWithCustomMessage()
        {
            Assert.Fail("Isolator does not allow custom messages.");
        }

        /// <summary>
        /// Per our setup, GetProducts is called with a different parameter than we expect.
        /// Which error message would we get?  
        /// </summary>
        public void CallExpectedWithWrongParameters()
        {
            const string expectedName = "nail";
            const string unexpectedName = "hammer";
            var warehouse = Isolate.Fake.Instance<IWarehouse>(); 
            
            var cart = new ShoppingCart();
            cart.AddProducts(unexpectedName, warehouse);

            Isolate.Verify.WasCalledWithExactArguments(() => warehouse.GetProducts(expectedName));
        }
    }
}
