using MockingFrameworksCompare.BrainSample;
using NUnit.Framework;
using Rhino.Mocks;
using Is=Rhino.Mocks.Constraints.Is;

namespace RhinoMocksTests
{
    [TestFixture]
    public class BrainTests
    {
        /// <summary>
        /// Verify that if hand throws an exception having touched a hot iron,  <see cref="IMouth.Yell"/> gets called.
        /// </summary>
        /// <remarks>
        /// Rhino Mocks can mock both interfaces and classes - however, only virtual methods 
        /// of a class can be mocked (try changing IHand/IMouth to Hand/Mouth).
        /// </remarks>
        [Test]
        public void TouchHotIron_Yell()
        {
            var hand = MockRepository.GenerateStub<IHand>();
            var mouth = MockRepository.GenerateMock<IMouth>();
            hand.Stub(h => h.TouchIron(null)).Constraints(Is.Matching<Iron>(i => i.IsHot)).Throw(new BurnException());
            mouth.Expect(m => m.Yell());

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron { IsHot = true });

            mouth.VerifyAllExpectations();
        }
    }


}
