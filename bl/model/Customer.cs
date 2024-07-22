namespace bl.model
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }


        public class CusToTotalBuy
        {
            public Guid Id { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }

            public decimal TotalPriceItem { get; set; }
        }

        public class CusToTotalBuyItme
        {
            public Guid SellId { get; set; }
            public string ManuName { get; set; }
            public string ItemName { get; set; }
            public decimal TotalPrice { get; set; }
            public int ItemSold { get; set; }
            public decimal TotalPriceItem { get; set; }
            public string Category { get; set; }
        }


        public static async Task<List<bl.model.Customer.CusToTotalBuy>> GetAllAsync()
        {

            var ret = await bl.data.Customer.ExecuteQueryAsync();

            return ret;

        }

        public static async Task<bl.model.Customer> GetNameAsync(string firstname)
        {

            var ret = await bl.data.Customer.ExecuteQueryNameAsync(firstname);

            return ret;

        }
        public static async Task<List<bl.model.Customer.CusToTotalBuyItme>> GetItemToBuyAsync(Guid Id)
        {
            var ret = await bl.data.Customer.BuyItemExecuteQueryAsync(Id);

            return ret.data;
        }


        public static async Task<bl.model.Customer.CusToTotalBuy> GetItemToBuyCustomerAsync(Guid Id)
        {
            var ret = await bl.data.Customer.ExecuteQueryCutomerBuyAsync(Id);

            return ret;
        }

    
    }

}
