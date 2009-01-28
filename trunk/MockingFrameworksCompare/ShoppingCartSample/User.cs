namespace MockingFrameworksCompare.ShoppingCartSample
{
    public class User
    {
        public virtual ContactDetails ContactDetails { get; set; }
    }

    public class ContactDetails
    {
        public virtual string Name { get; set; }
    }
}
