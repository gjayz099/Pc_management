namespace bl.model
{
    public class TotalPriceBuy
    {
        public Guid Id { get; set; }
        public decimal TotalPrice {  get; set; }
        public Guid CustomerId { get; set; }
    }
}
