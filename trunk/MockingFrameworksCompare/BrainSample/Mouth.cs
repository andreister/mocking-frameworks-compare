using System;

namespace MockingFrameworksCompare.BrainSample
{
    public interface IMouth
    {
        void Yell();
    }

    public class Mouth : IMouth
    {
        /// <summary>
        /// This method would be mocked out, so we left it not implemented.
        /// </summary>
        public void Yell()
        {
            throw new NotImplementedException();
        }
    }
}