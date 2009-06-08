using MockingFrameworksCompare.BrainSample;
using NMock2;

namespace NMock2Tests
{
    /// <summary>
    /// Those tests fail - the purpose is to assess error messages that come from the mocking framework.
    /// </summary>
    public class FailingTests
    {
        public void CallOnceExpectNever()
        {
            var mockery = new Mockery();
            var hand = (IHand)mockery.NewMock(typeof(IHand));
            var mouth = (IMouth)mockery.NewMock(typeof(IMouth));
            var iron = new Iron { IsHot = true };
            Expect.On(hand).Method("TouchIron").With(iron).Will(Throw.Exception(new BurnException()));
            //NMock2 creates strict mocks - we just remove the expectation, and the test should fail
            //Expect.On(mouth).Method("Yell"); 
            
            var brain = new Brain(hand, mouth);
            brain.TouchIron(iron);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        public void CallNeverExpectOnce()
        {
            var mockery = new Mockery();
            var hand = (IHand)mockery.NewMock(typeof(IHand));
            var mouth = (IMouth)mockery.NewMock(typeof(IMouth));
            var iron = new Iron { IsHot = true };
            Expect.On(hand).Method("TouchIron").With(iron); //we don't throw an exception, so mouth.Yell won't be called
            Expect.On(mouth).Method("Yell"); //but we expect it

            var brain = new Brain(hand, mouth);
            brain.TouchIron(iron);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        public void CallNeverExpectOnceCustom()
        {
            var mockery = new Mockery();
            var hand = (IHand)mockery.NewMock(typeof(IHand));
            var mouth = (IMouth)mockery.NewMock(typeof(IMouth));
            var iron = new Iron { IsHot = true };
            Expect.On(hand).Method("TouchIron").With(iron); //we don't throw an exception, so mouth.Yell won't be called
            Expect.On(mouth).Method("Yell").Comment("This is a meaningful custom message in case the expectation would fail, say, " +
                "'mouth should yell if we touch a hot iron'."); //but we expect it

            var brain = new Brain(hand, mouth);
            brain.TouchIron(iron);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        public void CallOnceExpectTwice()
        {
            var mockery = new Mockery();
            var hand = (IHand)mockery.NewMock(typeof(IHand));
            var mouth = (IMouth)mockery.NewMock(typeof(IMouth));
            var iron = new Iron { IsHot = true };
            Expect.On(hand).Method("TouchIron").With(iron).Will(Throw.Exception(new BurnException()));
            Expect.Exactly(2).On(mouth).Method("Yell"); 

            var brain = new Brain(hand, mouth);
            brain.TouchIron(iron);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
    }
}
