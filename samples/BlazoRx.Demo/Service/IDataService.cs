using System.Collections.Generic;
using BlazoRx.Demo.Model;

namespace BlazoRx.Demo.Service
{
    public interface IDataService
    {
        public IEnumerable<Customer> GetCustomers(int count);

        public IEnumerable<Order> GetOrders(int count);

        public IEnumerable<Product> GetProducts(int count);
    }
}
