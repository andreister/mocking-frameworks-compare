using System;
using MockingFrameworksCompare.BrainSample;
using Moq;

namespace MoqTests
{
    /// <summary>
    /// Those tests fail - the purpose is to assess error messages that come from the mocking framework.
    /// </summary>
    public class FailingTests
    {
        public void CallOnceExpectNever()
        {
            var hand = new Mock<IHand>();
            var mouth = new Mock<IMouth>();
            hand.Setup(x => x.TouchIron(It.Is<Iron>(i => i.IsHot))).Throws(new BurnException());

            var brain = new Brain(hand.Object, mouth.Object);
            brain.TouchIron(new Iron { IsHot = true });

            mouth.Verify(x => x.Yell(), Times.Never());    
        }

        public void CallOnceExpectTwice()
        {
            var hand = new Mock<IHand>();
            var mouth = new Mock<IMouth>();
            hand.Setup(x => x.TouchIron(It.Is<Iron>(i => i.IsHot))).Throws(new BurnException());

            var brain = new Brain(hand.Object, mouth.Object);
            brain.TouchIron(new Iron { IsHot = true });

            mouth.Verify(x => x.Yell(), Times.Exactly(2));
        }

        public void CallNeverExpectOnce()
        {
            var hand = new Mock<IHand>();
            var mouth = new Mock<IMouth>();
            hand.Setup(x => x.TouchIron(It.IsAny<Iron>())); //we don't throw an exception, so mouth.Yell won't be called

            var brain = new Brain(hand.Object, mouth.Object);
            brain.TouchIron(new Iron());

            mouth.Verify(x => x.Yell()); //but we expect it
        }

        public void CallNeverExpectOnceCustom()
        {
            var hand = new Mock<IHand>();
            var mouth = new Mock<IMouth>();
            hand.Setup(x => x.TouchIron(It.IsAny<Iron>())); //we don't throw an exception, so mouth.Yell won't be called

            var brain = new Brain(hand.Object, mouth.Object);
            brain.TouchIron(new Iron());

            mouth.Verify(x => x.Yell(), "This is a meaningful custom message in case the expectation would fail, say, " +
                "'mouth should yell if we touch a hot iron'."); //but we expect it
        }
    }
}
