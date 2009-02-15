using System.Collections.Generic;

namespace MockingFrameworksCompare.ShoppingCartSample
{
    public class ShoppingCart 
    {
        private readonly List<Product> _products = new List<Product>();

        /// <summary>
        /// 'Red' state of the cart - indicates that something went wrong along the way.
        /// </summary>
        public virtual bool IsRed { get; set; }

        /// <summary>
        /// Shopping cart owner.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Adds all products with names similar to the given one.
        /// </summary>
        /// <param name="name">Rough name of a product we're looking for.</param>
        /// <param name="warehouse">Wharehouse to get the products from.</param> 
        public virtual void AddProducts(string name, IWarehouse warehouse)
        {
            if (IsRed)
                return;

            try {
                warehouse.SomethingWentWrong += Alarm;
                
                List<Product> products = warehouse.GetProducts(name);
                products.ForEach(p => _products.Add(p));
            }
            finally {
                warehouse.SomethingWentWrong -= Alarm;
            }
        }

        /// <summary>
        /// Adds all products with names similar to the given one, if warehouse is available.
        /// </summary>
        /// <param name="name">Rough name of a product we're looking for.</param>
        /// <param name="warehouse">Wharehouse to get the products from.</param> 
        public virtual void AddProductsIfWarehouseAvailable(string name, IWarehouse warehouse)
        {
            if (warehouse.IsAvailable)
                AddProducts(name, warehouse);
        }

        /// <summary>
        /// Returns the number of products in the cart.
        /// </summary>
        /// <returns></returns>
        public virtual int GetProductsCount()
        {
            return _products.Count;
        }

        public string ThankYou()
        {
            return "Thank you, " + User.ContactDetails.Name;
        }

        private void Alarm(object sender, WarehouseEventArgs args)
        {
            if (args.BadRequest)
                IsRed = true;
        }
    }
}
