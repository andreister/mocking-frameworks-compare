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
        public void CallOnceExpectNever()
        {
            var hand = new SIHand();
            var mouth = new SIMouth();

            hand.TouchIron = (iron) => { throw new BurnException(); };
            bool yelled = false;
            mouth.Yell = (() => yelled = true);

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron()); //BurnException gets always thrown and mouth.Yell gets always called

            Assert.IsFalse(yelled, "Mouth yelled but we didn't expect that.");
        }

        public void CallNeverExpectOnce()
        {
            var hand = new SIHand();
            var mouth = new SIMouth();

            hand.TouchIron = (iron) => { if (iron.IsHot) throw new BurnException(); };
            bool yelled = false;
            mouth.Yell = (() => yelled = true);

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron()); //iron.IsHot is false, BurnException doesn't get thrown

            Assert.IsTrue(yelled, "Mouth hasn't yelled although we expected it to.");
        }

        public void CallNeverExpectOnceCustom()
        {
            Assert.Fail("All Stubs exceptions are custom ones.");
        }

        public void CallExpectedWithWrongParameters()
        {
            string expectedName = "nail";
            string unexpectedName = "hammer";

            var warehouse = new SIWarehouse();
            warehouse.GetProducts = (productName) =>
            {
                Assert.AreEqual(expectedName, productName, "GetProducts was called with incorect parameter");
                return new List<Product>();
            };

            var cart = new ShoppingCart();
            cart.AddProducts(unexpectedName, warehouse);
        }
    }
}
