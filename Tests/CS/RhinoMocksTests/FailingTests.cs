using System.Collections.Generic;
using MockingFrameworksCompare.BrainSample;
using MockingFrameworksCompare.ShoppingCartSample;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace RhinoMocksTests
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
            var hand = MockRepository.GenerateStub<IHand>();
            var mouth = MockRepository.GenerateMock<IMouth>();
            hand.Stub(h => h.TouchIron(null)).Constraints(Is.Anything()).Throw(new BurnException());
            mouth.Expect(m => m.Yell()).Repeat.Never();

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron());

            mouth.VerifyAllExpectations();
        }

        /// <summary>
        /// Per our setup, BurnException is never thrown so "mouth.Yell" is never called. 
        /// Imagine it's not what we want (the setup is incorrect), and the test would fail. 
        /// Which error message would we get? 
        /// </summary>
        public void CallNeverExpectOnce()
        {
            var hand = MockRepository.GenerateStub<IHand>();
            var mouth = MockRepository.GenerateMock<IMouth>();
            hand.Stub(h => h.TouchIron(null)); 
            mouth.Expect(m => m.Yell()); 

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron {IsHot = true});

            mouth.VerifyAllExpectations();
        }

        /// <summary>
        /// Per our setup, BurnException is never thrown so "mouth.Yell" is never called. 
        /// Imagine it's not what we want (the setup is incorrect), and the test would fail. 
        /// Can we provide a custom message for that? 
        /// </summary>
        public void FailWithCustomMessage()
        {
            var hand = MockRepository.GenerateStub<IHand>();
            var mouth = MockRepository.GenerateMock<IMouth>();
            hand.Stub(h => h.TouchIron(null)); 
            mouth.Expect(m => m.Yell()).Message("This is a meaningful custom message in case the expectation would fail."); //but we expect it

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron { IsHot = true });

            mouth.VerifyAllExpectations();
        }

        /// <summary>
        /// Per our setup, GetProducts is called with a different parameter than we expect.
        /// Which error message would we get?  
        /// </summary>
        public void CallExpectedWithWrongParameters()
        {
            const string expectedName = "nail";
            const string unexpectedName = "hammer";
            var warehouse = MockRepository.GenerateMock<IWarehouse>(); //if we have used GenerateStub, the test would have passed.
            warehouse.Expect(x => x.GetProducts(expectedName)).Return(new List<Product>());

            var cart = new ShoppingCart();
            cart.AddProducts(unexpectedName, warehouse);

            warehouse.VerifyAllExpectations();
        }
    }
}
