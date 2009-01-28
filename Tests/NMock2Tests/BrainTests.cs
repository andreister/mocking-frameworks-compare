using MockingFrameworksCompare.BrainSample;
using NMock2;
using NUnit.Framework;

namespace NMock2Tests
{
    [TestFixture]
    public class BrainTests
    {
        /// <summary>
        /// Verify that if hand throws an exception having touched a hot iron,  <see cref="IMouth.Yell"/> gets called.
        /// </summary>
        /// <remarks>
        /// NMock2 can mock only interfaces, calls are string based (and type unsafe). 
        /// </remarks>
        [Test]
        public void TouchHotIron_Yell()
        {
            var mockery = new Mockery();
            var hand = (IHand)mockery.NewMock(typeof(IHand));
            var mouth = (IMouth)mockery.NewMock(typeof(IMouth));
            var iron = new Iron { IsHot = true };
            Expect.On(hand).Method("TouchIron").With(iron).Will(Throw.Exception(new BurnException()));
            Expect.On(mouth).Method("Yell");

            var brain = new Brain(hand, mouth);
            brain.TouchIron(iron);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// We can extend NMock2 to make the calls type safe, providing the same user experience.
        /// However, this extension does not work in all cases, so guys from NMock2 still are 
        /// looking for the appropriate solution.
        /// </summary>
        /// <remarks>
        /// As you can see, <see cref="Mockery"/> implements IDisposable. However, putting it in 
        /// the <code>using</code> statements (so that <see cref="Mockery.VerifyAllExpectationsHaveBeenMet"/>
        /// would be called automatically) is discouraged - this approach could mangle the stack trace.
        /// (Say, InvalidOperationException may be hidden by the ExpectationException thrown at Dispose, etc.)
        /// </remarks>
        [Test]
        public void TouchHotIron_Yell_TypeSafe()
        {
            var mockery = new Mockery();
            var hand = mockery.NewMock<IHand>();
            var mouth = mockery.NewMock<IMouth>();
            var iron = new Iron { IsHot = true };
            Expect.On(hand).Method<Iron>(hand.TouchIron).With(iron).Will(Throw.Exception(new BurnException()));
            Expect.On(mouth).Method(mouth.Yell);

            var brain = new Brain(hand, mouth);
            brain.TouchIron(iron);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
    }
}
