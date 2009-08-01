using System.Collections.Generic;
using MockingFrameworksCompare.BrainSample;
using MockingFrameworksCompare.ShoppingCartSample;
using Moq;

namespace MoqTests
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
            var hand = new Mock<IHand>();
            var mouth = new Mock<IMouth>();
            hand.Setup(x => x.TouchIron(It.IsAny<Iron>())).Throws(new BurnException());

            var brain = new Brain(hand.Object, mouth.Object);
            brain.TouchIron(new Iron());

            mouth.Verify(x => x.Yell(), Times.Never());    
        }

        /// <summary>
        /// Per our setup, BurnException is never thrown so "mouth.Yell" is never called. 
        /// Imagine it's not what we want (the setup is incorrect), and the test would fail. 
        /// Which error message would we get? 
        /// </summary>
        public void CallNeverExpectOnce()
        {
            var hand = new Mock<IHand>();
            var mouth = new Mock<IMouth>();
            hand.Setup(x => x.TouchIron(It.IsAny<Iron>())); 

            var brain = new Brain(hand.Object, mouth.Object);
            brain.TouchIron(new Iron {IsHot = true});

            mouth.Verify(x => x.Yell());
        }

        /// <summary>
        /// Per our setup, BurnException is never thrown so "mouth.Yell" is never called. 
        /// Imagine it's not what we want (the setup is incorrect), and the test would fail. 
        /// Can we provide a custom message for that? 
        /// </summary>
        public void FailWithCustomMessage()
        {
            var hand = new Mock<IHand>();
            var mouth = new Mock<IMouth>();
            hand.Setup(x => x.TouchIron(It.IsAny<Iron>())); 

            var brain = new Brain(hand.Object, mouth.Object);
            brain.TouchIron(new Iron());

            mouth.Verify(x => x.Yell(), "This is a meaningful custom message in case the expectation would fail."); 
        }

        /// <summary>
        /// Per our setup, GetProducts is called with a different parameter than we expect.
        /// Which error message would we get?  
        /// </summary>
        public void CallExpectedWithWrongParameters()
        {
            const string expectedName = "nail";
            const string unexpectedName = "hammer";
            var warehouse = new Mock<IWarehouse>();
            warehouse.Setup(x => x.GetProducts(expectedName)).Returns(new List<Product>());

            var cart = new ShoppingCart();
            cart.AddProducts(unexpectedName, warehouse.Object);

            warehouse.Verify(x => x.GetProducts(expectedName));
        }
    }
}
