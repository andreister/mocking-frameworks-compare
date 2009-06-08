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
        public void CallOnceExpectNever()
        {
            var hand = Isolate.Fake.Instance<Hand>();
            var mouth = Isolate.Fake.Instance<Mouth>();
            var iron = new Iron { IsHot = true };
            Isolate.WhenCalled(() => hand.TouchIron(iron)).WillThrow(new BurnException());

            var brain = new Brain(hand, mouth);
            brain.TouchIron(iron);

            Isolate.Verify.WasNotCalled(() => mouth.Yell());
        }

        public void CallNeverExpectOnce()
        {
            var hand = Isolate.Fake.Instance<Hand>();
            var mouth = Isolate.Fake.Instance<Mouth>();
            var iron = new Iron { IsHot = true };
            Isolate.WhenCalled(() => hand.TouchIron(iron)); //we don't throw an exception, so mouth.Yell won't be called

            var brain = new Brain(hand, mouth);
            brain.TouchIron(iron);

            Isolate.Verify.WasCalledWithAnyArguments(() => mouth.Yell()); //but we expect it
        }

        public void CallNeverExpectOnceCustom()
        {
            Assert.Fail("Isolator does not allow custom messages.");
        }

        public void CallExpectedWithWrongParameters()
        {
            string expectedName = "nail";
            string unexpectedName = "hammer";

            var warehouse = Isolate.Fake.Instance<IWarehouse>(); 
            
            var cart = new ShoppingCart();
            cart.AddProducts(unexpectedName, warehouse);

            Isolate.Verify.WasCalledWithExactArguments(() => warehouse.GetProducts(expectedName));
        }
    }
}
