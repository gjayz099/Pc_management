namespace bl.dto
{
    public class TotalPriceBuy
    {
        public decimal TotalPrice { get; set; }
        public Guid CustomerId { get; set; }

        public string validate()
        {
            if (TotalPrice == 0) return "Totalprice dicemal is null";

            return "";
        }

        public static async Task<string> InsertAsync(bl.dto.TotalPriceBuy dto, Guid CusId)
        {   

            var error = dto.validate();
            if(!string.IsNullOrEmpty(error))  return error;
      
            await bl.data.TotalPriceBuy.InsertRequestAsync(dto, CusId);

            return "";
        }

    }
}
