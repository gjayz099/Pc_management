namespace bl.model
{
    public class Parts
    {    
        public Guid Id { get; set; }
        public string PartName { get; set; } // CPU
        public string Nanufacturer { get; set; } /// ex Intel
        public string Specification { get; set; } //ex 8 cores, 16 threads, 3.5 GHz base, 5.2 GHz turbo
        public Guid CategoriesID { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; } 
    }
}
