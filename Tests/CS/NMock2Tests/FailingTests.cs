using System.Collections.Generic;
using MockingFrameworksCompare.BrainSample;
using MockingFrameworksCompare.ShoppingCartSample;
using NMock2;

namespace NMock2Tests
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
            var mockery = new Mockery();
            var hand = mockery.NewMock<IHand>(MockStyle.Stub);
            var mouth = mockery.NewMock<IMouth>(); 
            Expect.On(hand).Method("TouchIron").With(Is.Anything).Will(Throw.Exception(new BurnException()));
            Expect.Never.On(mouth).Method("Yell");
            
            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron());

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Per our setup, BurnException is never thrown so "mouth.Yell" is never called. 
        /// Imagine it's not what we want (the setup is incorrect), and the test would fail. 
        /// Which error message would we get? 
        /// </summary>
        public void CallNeverExpectOnce()
        {
            var mockery = new Mockery();
            var hand = mockery.NewMock<IHand>(MockStyle.Stub);
            var mouth = mockery.NewMock<IMouth>(MockStyle.Stub); 
            Expect.On(hand).Method("TouchIron").With(Is.Anything); //we don't throw an exception, so mouth.Yell won't be called
            Expect.On(mouth).Method("Yell"); //but we expect it

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron {IsHot = true});

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Per our setup, BurnException is never thrown so "mouth.Yell" is never called. 
        /// Imagine it's not what we want (the setup is incorrect), and the test would fail. 
        /// Can we provide a custom message for that? 
        /// </summary>
        public void FailWithCustomMessage()
        {
            var mockery = new Mockery();
            var hand = mockery.NewMock<IHand>(MockStyle.Stub);
            var mouth = mockery.NewMock<IMouth>(MockStyle.Stub);
            Expect.On(hand).Method("TouchIron").With(Is.Anything); 
            Expect.On(mouth).Method("Yell").Comment("This is a meaningful custom message in case the expectation would fail."); 

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron());

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Per our setup, GetProducts is called with a different parameter than we expect.
        /// Which error message would we get?  
        /// </summary>
        public void CallExpectedWithWrongParameters()
        {
            const string expectedName = "nail";
            const string unexpectedName = "hammer";
            var mockery = new Mockery();
            var warehouse = mockery.NewMock<IWarehouse>(MockStyle.Stub);
            Expect.On(warehouse).Method("GetProducts").With(expectedName).Will(Return.Value(new List<Product>()));

            var cart = new ShoppingCart();
            cart.AddProducts(unexpectedName, warehouse);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
    }
}
