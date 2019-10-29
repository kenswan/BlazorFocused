using System.Collections.Generic;
using System.Linq;

namespace BlazoRx.Demo.Model
{
    public class AppState
    {
        public AppState()
        {
            // initial state

            AppInitialized = false;

            AllCustomers = Enumerable.Empty<Customer>();

            AllOrders = Enumerable.Empty<Order>();

            AllProducts = Enumerable.Empty<Product>();
        }

       public bool AppInitialized { get; set; }

        public IEnumerable<Order> CurrentCustomerOrders { get; set; }

        public IEnumerable<Customer> AllCustomers { get; set; }

        public IEnumerable<Order> AllOrders { get; set; }

        public IEnumerable<Product> AllProducts { get; set; }
    }
}
