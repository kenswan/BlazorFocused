using BlazoRx.Demo.Model;
using System.Collections.Generic;
using System.Linq;
using Tynamix.ObjectFiller;

namespace BlazoRx.Demo.Service
{
    public class MockDataService : IDataService
    {
        public IEnumerable<Customer> GetCustomers(int count)
        {
            var customerFiller = new Filler<Customer>();

            customerFiller.Setup()
                .OnProperty(customer => customer.FirstName).Use(new RealNames(NameStyle.FirstName))
                .OnProperty(customer => customer.LastName).Use(new RealNames(NameStyle.LastName))
                .OnProperty(customer => customer.Email).Use(new EmailAddresses());

            return customerFiller.Create(count);
        }

        public IEnumerable<Order> GetOrders(int count)
        {
            Customer[] customers = GetCustomers(count).ToArray();
            Product[] products = GetProducts(count).ToArray();
            Order[] orders = new Filler<Order>().Create(count).ToArray();

            for(int i = 0; i < count; i++)
            {
                orders[i].Customer = customers[i];
                orders[i].Product = products[i];
            }

            return orders;
        }

        public IEnumerable<Product> GetProducts(int count)
        {
            var productFiller = new Filler<Product>();

            productFiller.Setup()
                .OnProperty(product => product.Name).Use(new MnemonicString())
                .OnProperty(product => product.Description).Use(new Lipsum(LipsumFlavor.LoremIpsum, 10, 10));

            return productFiller.Create(count);
        }
    }
}
