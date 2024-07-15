namespace bl.dto
{
    public class Sales
    {
        public Guid PartsID { get; set; } //// Manufature DB
        public int QuantitySold { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice
        {
            get
            {
                return QuantitySold * UnitPrice;
            }
        }
        public Guid CustomerID { get; set; } ///// user DB
        public DateTime DateSale { get; set; }
    }
}
