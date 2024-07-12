using bl.data;

namespace bl.model
{
    public class Parts
    {
        public Guid Id { get; set; }
        public Guid NanufactureId { get; set; }
        public string Description { get; set; }


        public class PartManufacture
        {
            public Guid Id { get; set; }
            public string ManufactureName { get; set; }
            public string Specifation { get; set; }
            public string CategoreName { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public string Description { get; set; }
        }

        public static async Task<List<bl.model.Parts.PartManufacture>> GetAllAsync()
        {
            // Call ExecuteQueryAsync to get the list of PartManufacture objects asynchronously
            List<bl.model.Parts.PartManufacture> ret;
              ret = await bl.data.Parts.ExecuteQueryAsync();


            // Return the list of formatted strings
            return ret;
        }

    }
}
