namespace BlazoRx.Demo.Model
{
    public class Order
    {
        public int Id { get; set; }

        public Customer Customer { get; set; }

        public Product Product { get; set; }
    }
}
