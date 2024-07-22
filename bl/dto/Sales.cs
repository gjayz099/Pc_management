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
        public Guid CustomerID { get; set; } ///// Custmer DB
        public DateTime DateSale { get; set; }



        public string Validate()
        {
            if (PartsID == Guid.Empty) return "Parts Name is empty";
            if (QuantitySold <= 0) return "Quantity Sold must be greater than zero";
            if (UnitPrice <= 0) return "Unit Price must be greater than zero";
            // Add more validation rules as needed

            return ""; // Return empty string if no validation errors found
        }



        public static async Task<string> InsertAsync(List<bl.dto.Sales> dtoList, Guid CusId)
        {
            foreach (var dto in dtoList)
            {
                string validationError = dto.Validate();
                if (!string.IsNullOrEmpty(validationError)) return validationError;

            }

            await bl.data.Sales.InsertRequestAsync(dtoList, CusId);

            return ""; 
        }


        public static async Task<decimal> GetSumSaleAsync()
        {

            var ret = await bl.data.Sales.SumSaleExecuteQueryAsync();

            return ret;
        }

    }
}
