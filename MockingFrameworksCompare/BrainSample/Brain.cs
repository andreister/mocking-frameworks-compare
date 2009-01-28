namespace MockingFrameworksCompare.BrainSample
{
    public class Brain
    {
        private readonly IHand _hand;
        private readonly IMouth _mouth;

        /// <summary>
        /// Creates the brain using Dependency Pattern, that is needed for most mocking frameworks to work.
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="mouth"></param>
        public Brain(IHand hand, IMouth mouth)
        {
            _hand = hand;
            _mouth = mouth;
        }

        /// <summary>
        /// Creates the brain with hand and mouth being created inside the constructor. Only Typemock Isolator
        /// can mock types that get created inside a constructor. 
        /// </summary>
        public Brain()
        {
            _hand = new Hand();
            _mouth = new Mouth();
        }

        public void TouchIron(Iron iron)
        {
            try
            {
                _hand.TouchIron(iron);
            }
            catch (BurnException)
            {
                _mouth.Yell();
            }
        }
    }
}