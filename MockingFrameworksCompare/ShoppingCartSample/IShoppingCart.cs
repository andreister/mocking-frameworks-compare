namespace MockingFrameworksCompare.ShoppingCartSample
{
    public interface IShoppingCart
    {
        /// <summary>
        /// Adds all products with names similar to the given one.
        /// </summary>
        /// <param name="name">Rough name of a product we're looking for.</param>
        /// <param name="warehouse">Wharehouse to get the products from.</param> 
        void AddProducts(string name, IWarehouse warehouse);

        /// <summary>
        /// Adds all products with names similar to the given one, if warehouse is available.
        /// </summary>
        /// <param name="name">Rough name of a product we're looking for.</param>
        /// <param name="warehouse">Wharehouse to get the products from.</param> 
        void AddProductsIfWarehouseAvailable(string name, IWarehouse warehouse);

        /// <summary>
        /// Returns the number of products in the cart.
        /// </summary>
        /// <returns></returns>
        int GetProductsCount();
    }
}
