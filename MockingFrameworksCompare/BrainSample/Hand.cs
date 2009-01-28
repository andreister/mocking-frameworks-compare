using System;
using MockingFrameworksCompare.BrainSample;

namespace MockingFrameworksCompare.BrainSample
{
    public interface IHand
    {
        void TouchIron(Iron iron);
    }

    /// <summary>
    /// The methods here would be mocked out, so we left them not implemented.
    /// </summary>
    public class Hand : IHand
    {
        public void TouchIron(Iron iron)
        {
            throw new NotImplementedException();
        }
    }
}