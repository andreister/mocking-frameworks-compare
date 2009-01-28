using System;
using System.Collections.Generic;

namespace MockingFrameworksCompare.ShoppingCartSample
{
    public interface IWarehouse
    {
        /// <summary>
        /// Returns the list of all products available with names resembling the one provided.
        /// </summary>
        /// <param name="name">Name of a product we're looking for.</param> 
        /// <returns></returns>
        List<Product> GetProducts(string name);

        /// <summary>
        /// Returns true if warehouse is available, false otherwise.
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// Gets invoked when something went wrong at the warehouse side during the request.
        /// </summary>
        event EventHandler<WarehouseEventArgs> SomethingWentWrong;
    }

    public class WarehouseEventArgs : EventArgs
    {
        public bool BadRequest { get; set; }
    }
}
