namespace bl.model
{
    public class Sales
    {
        public Guid Id { get; set; }
        public string PartsID { get; set; }
        public int QuantitySold { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateSale { get; set; }
    }
}
