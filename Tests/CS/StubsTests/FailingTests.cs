using System.Collections.Generic;
using MockingFrameworksCompare.BrainSample;
using MockingFrameworksCompare.BrainSample.Stubs;
using MockingFrameworksCompare.ShoppingCartSample;
using MockingFrameworksCompare.ShoppingCartSample.Stubs;
using NUnit.Framework;

namespace StubsTests
{
    public class FailingTests
    {
        /// <summary>
        /// Per our setup, BurnException is always thrown so "mouth.Yell" is always called. 
        /// Imagine it's not what we want (the setup is incorrect), and the test would fail. 
        /// Which error message would we get? 
        /// </summary>
        public void CallOnceExpectNever()
        {
            var hand = new SIHand {
                TouchIron = delegate { throw new BurnException(); }
            };
            var mouth = new SIMouth {
                //Stubs don't have an API so the Assert below is a way to detect whether a method gets called.
                Yell = delegate { Assert.Fail("Mouth yelled but we didn't expect it to (note it's a custom message)."); }
            };

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron()); 
        }

        /// <summary>
        /// Per our setup, BurnException is never thrown so "mouth.Yell" is never called. 
        /// Imagine it's not what we want (the setup is incorrect), and the test would fail. 
        /// Which error message would we get? 
        /// </summary>
        public void CallNeverExpectOnce()
        {
            var hand = new SIHand {
                TouchIron = delegate { /*do nothing*/ }
            };
            bool yelled = false; 
            var mouth = new SIMouth {
                //Stubs don't have an API so "yelled" flag is the way to detect whether a method gets called.
                Yell = delegate { yelled = true; }
            };

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron {IsHot = true}); 

            Assert.IsTrue(yelled, "Mouth hasn't yelled although we expected it to (note it's a custom message).");
        }

        /// <summary>
        /// Per our setup, BurnException is never thrown so "mouth.Yell" is never called. 
        /// Imagine it's not what we want (the setup is incorrect), and the test would fail. 
        /// Can we provide a custom message for that? 
        /// </summary>
        public void FailWithCustomMessage()
        {
            Assert.Fail("As you can see from above, all Stubs exceptions are custom ones.");
        }

        /// <summary>
        /// Per our setup, GetProducts is called with a different parameter than we expect.
        /// Which error message would we get?  
        /// </summary>
        public void CallExpectedWithWrongParameters()
        {
            const string expectedName = "nail";
            const string unexpectedName = "hammer";
            var warehouse = new SIWarehouse {
                GetProducts = delegate(string productName) {
                    //Stubs don't have an API so the Assert below is the way to control the parameters.
                    Assert.AreEqual(expectedName, productName, "GetProducts was called with incorect parameter (note it's a custom message).");
                    return new List<Product>();
                }
            };

            var cart = new ShoppingCart();
            cart.AddProducts(unexpectedName, warehouse);
        }
    }
}
