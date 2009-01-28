using System.Collections.Generic;

namespace MockingFrameworksCompare.ShoppingCartSample
{
    public class ShoppingCart : IShoppingCart
    {
        private readonly List<Product> _products = new List<Product>();
        public virtual bool IsRed { get; set; }
        public virtual User User { get; set; }

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

        public virtual void AddProductsIfWarehouseAvailable(string name, IWarehouse warehouse)
        {
            if (warehouse.IsAvailable)
                AddProducts(name, warehouse);
        }

        public void Alarm(object sender, WarehouseEventArgs args)
        {
            if (args.BadRequest)
                IsRed = true;
        }

        public string ThankYou()
        {
            return "Thank you, " + User.ContactDetails.Name;
        }

        public virtual int GetProductsCount()
        {
            return _products.Count;
        }
    }
}
