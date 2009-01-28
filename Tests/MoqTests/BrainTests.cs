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
            hand.Setup(x => x.TouchIron(It.Is<Iron>(i => i.IsHot))).Throws(new BurnException());

            var brain = new Brain(hand.Object, mouth.Object);
            brain.TouchIron(new Iron { IsHot = true });

            mouth.Verify(x => x.Yell());    
        }

        /// <summary>
        /// A separate property would be really helpful to make the tests less verbose,
        /// since parameter expectations tend to be quite long in Moq. Unfortunately,
        /// this approach is not possible.
        /// </summary>
        private static Iron HotIron
        {
            get { return It.Is<Iron>(i => i.IsHot); }
        }
    }

}
