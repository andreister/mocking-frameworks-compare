using MockingFrameworksCompare.BrainSample;
using Moq;
using NUnit.Framework;

namespace MoqTests
{
    [TestFixture]
    public class BrainTests
    {
        /// <summary>
        /// Verify that if hand throws an exception having touched a hot iron,  <see cref="IMouth.Yell"/> gets called.
        /// </summary>
        /// <remarks>
        /// Moq can mock both interfaces and classes - however, only virtual methods 
        /// of a class can be mocked (try changing IHand/IMouth to Hand/Mouth).
        /// </remarks>
        [Test]
        public void TouchHotIron_Yell()
        {
            var hand = new Mock<IHand>();
            var mouth = new Mock<IMouth>();
            hand.Setup(x => x.TouchIron(HotIron())).Throws(new BurnException());

            var brain = new Brain(hand.Object, mouth.Object);
            brain.TouchIron(new Iron { IsHot = true });

            mouth.Verify(x => x.Yell());    
        }

        /// <summary>
        /// Parameter expectations tend to be quite verbose in Moq, so we provide a custom matcher.
        /// This needs a matcher method and a bool sibling method for evaluating the expectations.
        /// Calling this matcher is technically equivalent to <code>It.Is{Iron}(i => i.IsHot)</code>.
        /// </summary>
        [Matcher]
        private static Iron HotIron() { return null; }
        public static bool HotIron(Iron iron) { return iron.IsHot; }
    }

}
