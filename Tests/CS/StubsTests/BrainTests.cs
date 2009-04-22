using MockingFrameworksCompare.BrainSample;
using NUnit.Framework;
using MockingFrameworksCompare.BrainSample.Stubs;

namespace StubsTests
{
    /// <summary>
    /// Brain test implementation using Stubs.
    /// Stubs home page: http://research.microsoft.com/stubs
    /// </summary>
    [TestFixture]
    public class BrainTests
    {
        /// <summary>
        /// Verify that if hand throws an exception having touched a hot iron,  <see cref="IMouth.Yell"/> gets called.
        /// </summary>
        /// <remarks>
        /// Stubs can mock both interfaces and classes - however, only virtual methods 
        /// of a class can be mocked (try changing IHand/IMouth to Hand/Mouth).
        /// </remarks>
        [Test]
        public void TouchHotIron_Yell()
        {
            var hand = new SIHand();
            var mouth = new SIMouth();

            hand.TouchIron = (iron) =>
            {
                if (iron.IsHot)
                    throw new BurnException();
            };
            bool yelled = false;
            mouth.Yell = () => yelled = true;

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron { IsHot = true });

            Assert.IsTrue(yelled);
        }
    }
}
