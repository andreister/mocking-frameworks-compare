using MockingFrameworksCompare.BrainSample;
using NUnit.Framework;
using TypeMock.ArrangeActAssert;

namespace IsolatorTests
{
    [TestFixture]
    public class BrainTests
    {
        /// <summary>
        /// Can mock both classes and interfaces, can mock private/static classes etc.
        /// </summary>
        [Test, Isolated]
        public void TouchHotIron_Yell()
        {
            var hand = Isolate.Fake.Instance<Hand>();
            var mouth = Isolate.Fake.Instance<Mouth>();
            var iron = new Iron { IsHot = true };
            Isolate.WhenCalled(() => hand.TouchIron(iron)).WillThrow(new BurnException());

            var brain = new Brain(hand, mouth);
            brain.TouchIron(iron);

            Isolate.Verify.WasCalledWithAnyArguments(() => mouth.Yell());
        }

        /// <summary>
        /// Can mock objects WITHOUT DEPENDENCY INJECTION.
        /// </summary>
        [Test, Isolated]
        public void TouchHotIron_Yell_NoDependencyInjection()
        {
            var hand = Isolate.Fake.Instance<Hand>();
            var mouth = Isolate.Fake.Instance<Mouth>();
            Isolate.Swap.NextInstance<Hand>().With(hand);
            Isolate.Swap.NextInstance<Mouth>().With(mouth);
            var iron = new Iron { IsHot = true };
            Isolate.WhenCalled(() => hand.TouchIron(iron)).WillThrow(new BurnException());

            //notice we're not passing the mocked objects in.
            var brain = new Brain();
            brain.TouchIron(iron);

            Isolate.Verify.WasCalledWithAnyArguments(() => mouth.Yell());
        }
    }
}
