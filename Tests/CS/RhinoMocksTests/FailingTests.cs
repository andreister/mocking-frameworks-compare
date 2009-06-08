using MockingFrameworksCompare.BrainSample;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace RhinoMocksTests
{
    /// <summary>
    /// Those tests fail - the purpose is to assess error messages that come from the mocking framework.
    /// </summary>
    public class FailingTests
    {
        public void CallOnceExpectNever()
        {
            var hand = MockRepository.GenerateStub<IHand>();
            var mouth = MockRepository.GenerateMock<IMouth>();
            hand.Stub(h => h.TouchIron(null)).Constraints(Is.Matching<Iron>(i => i.IsHot)).Throw(new BurnException());
            mouth.Expect(m => m.Yell()).Repeat.Never();

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron { IsHot = true });

            mouth.VerifyAllExpectations();
        }

        public void CallNeverExpectOnce()
        {
            var hand = MockRepository.GenerateStub<IHand>();
            var mouth = MockRepository.GenerateMock<IMouth>();
            hand.Stub(h => h.TouchIron(null)); //we don't throw an exception, so mouth.Yell won't be called
            mouth.Expect(m => m.Yell()); //but we expect it

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron { IsHot = true });

            mouth.VerifyAllExpectations();
        }

        public void CallNeverExpectOnceCustom()
        {
            var hand = MockRepository.GenerateStub<IHand>();
            var mouth = MockRepository.GenerateMock<IMouth>();
            hand.Stub(h => h.TouchIron(null)); //we don't throw an exception, so mouth.Yell won't be called
            mouth.Expect(m => m.Yell()).Message("This is a meaningful custom message in case the expectation would fail, say, " +
                "'mouth should yell if we touch a hot iron'."); //but we expect it

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron { IsHot = true });

            mouth.VerifyAllExpectations();
        }

        public void CallOnceExpectTwice()
        {
            var hand = MockRepository.GenerateStub<IHand>();
            var mouth = MockRepository.GenerateMock<IMouth>();
            hand.Stub(h => h.TouchIron(null)).Constraints(Is.Matching<Iron>(i => i.IsHot)).Throw(new BurnException());
            mouth.Expect(m => m.Yell()).Repeat.Twice();

            var brain = new Brain(hand, mouth);
            brain.TouchIron(new Iron { IsHot = true });

            mouth.VerifyAllExpectations();
        }
    }
}
