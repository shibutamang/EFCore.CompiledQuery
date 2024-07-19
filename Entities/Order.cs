namespace PreCompiledQuery.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }
        public int Qty { get; set; }
        public decimal ItemPrice { get; set; }
    }
}
