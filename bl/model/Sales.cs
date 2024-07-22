namespace bl.model
{
    public class Sales
    {
        public Guid Id { get; set; }
        public Guid PartsID { get; set; } //// Manufature DB
        public int QuantitySold { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid CustomerID { get; set; } ///// Custmer DB
        public DateTime DateSale { get; set; }

        public class SaleWithCustomer
        {
            public Guid Id { get; set; }
            public string Manufature  { get; set; }
            public string Category { get; set; }
            public string Specification { get; set; }
            public int QuantitySold { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal TotalPrice { get; set; }
            public string CustomerFullname { get; set; }
            public DateTime DateSale { get; set; }
        }

        public class Totalprice
        {
            public decimal Total_Price { get; set; } 
        }

        public static async Task<List<bl.model.Sales.SaleWithCustomer>> GetAllAsync()
        {

            var ret = await bl.data.Sales.ExecuteQueryAsync();

            return ret.data;
        }




    }
}
