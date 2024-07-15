namespace bl.model
{
    public class Manufacturies
    {
        public Guid Id { get; set; }
        public string ManufactureName {  get; set; }
        public string Specification { get; set; } //ex 8 cores, 16 threads, 3.5 GHz base, 5.2 GHz turbo
        public Guid CategotyID { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }


        public class ManufacturiesWithCategories
        {
            public Guid Id { get; set; }
            public string ManufactureName { get; set; }
            public string Specification { get; set; } //ex 8 cores, 16 threads, 3.5 GHz base, 5.2 GHz turbo
            public string CategotyName { get; set; }
            public int Stock { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }

        }


        public static async Task<List<bl.model.Manufacturies.ManufacturiesWithCategories>> GetAllAsync()
        {
            var ret = await bl.data.Manufaturies.ExecuteQueryAsync();

            return ret.data;

        }

        public static async Task<bl.dto.Manufacturies> GetIDAsync(Guid? id)
        {
            // Fetch manufacturing details using ExecuteQueryIDAsync method
            bl.dto.Manufacturies ret = await bl.data.Manufaturies.ExecuteQueryIDAsync(id);

          

            return ret;
        }
    }

}
